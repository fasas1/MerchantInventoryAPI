using Azure;
using MechantInventory.Data;
using MechantInventory.Model;
using MechantInventory.Services;
using MerchantInventory.Models.Dto;
using MerchantInventory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using MechantInventory.Model.Dto;
using MechantInventory.Interface;


namespace MechantInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IStockRepository _stockRepository;
        private ApiResponse _response;
        public StockController(ApplicationDbContext db, IStockRepository stockRepository)
        {
            _db = db;
            _response = new ApiResponse();
            _stockRepository = stockRepository; 
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllStocks()
        {

            var stocks = await _stockRepository.GetAllAsync(includeProperties: "Product");

            var stockList = stocks.Select(s => new StockReadDto
            {
                StockId = s.StockId,
                CurrentQuantity = s.CurrentQuantity,
                LastUpdated = s.LastUpdated,
                Threshold = s.Threshold,
                ProductId = s.ProductId,
                ProductName = s.Product?.Name
            }).ToList();

            _response.Result = stockList;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }


        [HttpGet("{id}", Name = "GetStock")]
        public async Task<ActionResult<ApiResponse>> GetStock(int id)
        {
            var stock = await _db.Stocks.Include(s => s.Product).FirstOrDefaultAsync(s => s.StockId == id);
            if (stock == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Stock not found.");
                return NotFound(_response);
            }

            var stockDto = new StockReadDto
            {
                StockId = stock.StockId,
                CurrentQuantity = stock.CurrentQuantity,
                LastUpdated = stock.LastUpdated,
                Threshold = stock.Threshold,
                ProductId = stock.ProductId,
                ProductName = stock.Product?.Name
            };

            _response.Result = stockDto;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("GetStockStatus/{id}")]
        public async Task<ActionResult<string>> GetStockStatus(int id)
        {
            var stock = await _db.Stocks.FirstOrDefaultAsync(s => s.StockId == id);

            if (stock == null)
                return NotFound("Stock not found.");

            if (stock.CurrentQuantity == 0)
                return "Out of Stock";
            else if (stock.CurrentQuantity < stock.Threshold)
                return "Low Stock - Threshold too low!";
            else
                return "In Stock";
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateStock([FromBody] StockCreateDto stockDto)
        {
            try
            {
                Stock stockToCreate = new()
                {
                    CurrentQuantity = stockDto.CurrentQuantity,
                    LastUpdated = stockDto.LastUpdated,
                    Threshold = stockDto.Threshold,
                    ProductId = stockDto.ProductId
                };

                await _db.Stocks.AddAsync(stockToCreate);
                await _db.SaveChangesAsync();

                await _db.Entry(stockToCreate)
                         .Reference(s => s.Product)
                         .LoadAsync();

                // Map to DTO to avoid circular reference
                var stockReadDto = new StockReadDto
                {
                  
                    CurrentQuantity = stockToCreate.CurrentQuantity,
                    LastUpdated = stockToCreate.LastUpdated,
                    Threshold = stockToCreate.Threshold,
                    ProductId = stockToCreate.ProductId,
                    ProductName = stockToCreate.Product?.Name
                };

                _response.Result = stockReadDto;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetStock", new { id = stockToCreate.StockId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return _response;
            }
        }
        // Endpoint to restock a product
        [HttpPut("restock/{productId}")]
        public async Task<IActionResult> RestockProduct(int productId, [FromBody] RestockDto restockDto)
        {
            var stock = await _db.Stocks.Include(s => s.Product)
                                        .FirstOrDefaultAsync(s => s.ProductId == productId);

            if (stock == null)
            {
                return NotFound("Product not found.");
            }

            stock.CurrentQuantity += restockDto.Quantity;
            stock.LastUpdated = DateTime.UtcNow;

            _db.Stocks.Update(stock);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                Message = "Stock updated successfully",
                ProductName = stock.Product?.Name,
                NewQuantity = stock.CurrentQuantity
            });
        }
    }

}

