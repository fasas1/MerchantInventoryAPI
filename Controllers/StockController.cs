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
using MechantInventory.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;


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
        public async Task<ActionResult<ApiResponse>> GetAllStocks([FromQuery] int pageSize = 10,
     [FromQuery] int pageNumber = 1)
        {
            var pagedStocks = await _stockRepository.GetAllAsync(
                 includeProperties:"Product",
                 pageNumber : pageNumber,
                 pageSize : pageSize 
                );

            var stockDto = pagedStocks.Items.Select(s => new StockReadDto
            {
                StockId = s.StockId,
                CurrentQuantity = s.CurrentQuantity,
                LastUpdated = s.LastUpdated,
                Threshold = s.Threshold,
                ProductId = s.ProductId,
                ProductName = s.Product?.Name,
                ProductPrice = s.Product?.Price
            }). ToList();

            var response = new ApiResponse
            {
                Result = stockDto,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                TotalCount = pagedStocks.TotalCount,
                PageSize = pagedStocks.PageSize,
                PageNumber = pagedStocks.PageNumber
            };

            return Ok(response);
        }


        [HttpGet("{id}", Name = "GetStock")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
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
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
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
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
        public async Task<ActionResult<ApiResponse>> CreateStock([FromBody] StockCreateDto stockDto)
        {
            try
            {
                var existingStock = await _db.Stocks.FirstOrDefaultAsync(s =>s.ProductId == stockDto.ProductId);
                if (existingStock != null)
                {
                    existingStock.CurrentQuantity = stockDto.CurrentQuantity;
                    existingStock.Threshold = stockDto.Threshold;
                    existingStock.LastUpdated = DateTime.UtcNow;
                    _db.Stocks.Update(existingStock);
                }
              
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

        [HttpPut("{id}")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] StockDto dto)
        {
            var stock = await _db.Stocks.FindAsync(id);

            if (stock == null)
                return NotFound("Stock not found.");

            stock.ProductId = dto.ProductId;
            stock.CurrentQuantity = dto.CurrentQuantity;
            stock.Threshold = dto.Threshold;
            stock.LastUpdated = dto.LastUpdated;

            _db.Stocks.Update(stock);
            await _db.SaveChangesAsync();

            return Ok(stock);
        }

        [HttpDelete("{id:int}", Name = "DeleteStock")]
  

        public async Task<ActionResult<ApiResponse>> DeleteStock(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Invalid product id." };
                    return BadRequest(_response);
                }
                Stock stockFromDb = await _db.Stocks.FindAsync(id);
                if (stockFromDb == null)
                {
                    return BadRequest();
                }
                _db.Stocks.Remove(stockFromDb);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest();
            }
            return _response;
        }
    }

}

