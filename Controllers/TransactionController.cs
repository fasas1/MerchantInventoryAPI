using Azure;
using MechantInventory.Data;
using MechantInventory.Interface;
using MechantInventory.Model;
using MechantInventory.Model.Dto;
using MerchantInventory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MechantInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStockRepository _stockRepository;
        private ApiResponse _response;
        public TransactionController(ITransactionRepository transactionRepository, IProductRepository productRepository,
                                              IStockRepository stockRepository, ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllTransactions()
        {
            var transactions = await _transactionRepository.GetAllAsync(includeProperties: "Product");

            var transactionList = transactions.Select(t => new TransactionReadDto
            {
                ProductId = t.ProductId,
                PerformedBy = t.PerformedBy,
                Quantity = t.QuantityChanged,
                ProductName = t.Product.Name,
                Type = t.TransactionType

            }).ToList();
            _response.Result = transactionList;
            _response.StatusCode = HttpStatusCode.OK;

            return _response;
        }

        [HttpGet("{id}", Name = "GetTransaction")]
        public async Task<ActionResult<ApiResponse>> GetTransaction(int id)
        {
            var transaction = await _transactionRepository.GetTransactionWithDetailsAsync(id);

            if (transaction == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Transaction not found.");
                return NotFound(_response);
            }

            var transactionDto = new TransactionReadDto
            {
                ProductId = transaction.ProductId,
                Quantity = transaction.QuantityChanged,
                Type = transaction.TransactionType,
                PerformedBy = transaction.PerformedBy,
                ProductName = transaction.Product?.Name // assuming Product is loaded and has a Name
            };

            _response.Result = transactionDto;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost("CreateTransaction")]
        public async Task<ActionResult<ApiResponse>> CreateTransaction([FromBody] TransactionCreateDto transactionDto)
        {
            try
            {
                //var product = await _productRepository.GetAsync(transactionDto.ProductId);
                //if (product == null)
                //{
                //    _response.IsSuccess = false;
                //    _response.ErrorMessages.Add("Product not found.");
                //    return NotFound(_response);
                //}

                var stock = await _stockRepository.GetAsync(s => s.ProductId == transactionDto.ProductId);
                if (stock == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Stock entry not found for this product.");
                    return NotFound(_response);
                }

                if (transactionDto.Type.ToLower() == "sale")
                {
                    if (stock.CurrentQuantity < transactionDto.Quantity)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Not enough stock to complete this sale.");
                        return BadRequest(_response);
                    }

                    stock.CurrentQuantity -= transactionDto.Quantity;
                }
                else if (transactionDto.Type.ToLower() == "restock")
                {
                    stock.CurrentQuantity += transactionDto.Quantity;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Invalid transaction type.");
                    return BadRequest(_response);
                }

                stock.LastUpdated = DateTime.UtcNow;

                var transaction = new Transaction
                {
                    ProductId = transactionDto.ProductId,
                    QuantityChanged = transactionDto.Quantity,
                    TransactionType = transactionDto.Type,
                    Timestamp = DateTime.UtcNow,
                    PerformedBy = transactionDto.PerformedBy ?? "System"
                };

                await _db.Transactions.AddAsync(transaction);
                await _db.SaveChangesAsync();

                _response.Result = transaction;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtAction(nameof(CreateTransaction), new { id = transaction.TransactionId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(500, _response);
            }
        }
    }
}
