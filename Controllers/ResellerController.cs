using MechantInventory.Data;
using MechantInventory.Interface;
using MechantInventory.Model;
using MechantInventory.Model.Dto;
using MechantInventory.Repository.IRepository;
using MechantInventory.Services;
using MerchantInventory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MechantInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResellerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IResellerRepository _resellerRepository;

        private ApiResponse _response;
        public ResellerController(ApplicationDbContext db, IResellerRepository resellerRepository)
        {
            _db = db;

            _response = new ApiResponse();
            _resellerRepository = resellerRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllResellers([FromQuery] int pageSize = 10,
                [FromQuery] int pageNumber = 1)
        {
            var pagedResellers = await _resellerRepository.GetAllAsync(
                  includeProperties: "Transactions",
                  pageNumber: pageNumber,
                  pageSize: pageSize
                );

            var resellerDto = pagedResellers.Items.Select(r => new ResellerReadDto
            {
                ResellerId = r.ResellerId,
                Name = r.Name,
                Phone = r.Phone,
                Address = r.Address,


            }).ToList();
            var response = new ApiResponse
            {
                Result = resellerDto,
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                TotalCount = pagedResellers.TotalCount,
                PageNumber = pagedResellers.PageNumber,
                PageSize = pagedResellers.PageSize
            };
            return Ok(response);
        }

        [HttpGet("{id:int}",Name = "GetReseller")]
        public async Task<ActionResult<ApiResponse>> GetReseller(int id)
        {
            if (id == 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var reseller = await _resellerRepository.GetResellerByIdAsync(id);
            if (reseller == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return NotFound(_response);
            }
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = reseller;
            return Ok(_response);
        }
        //[HttpGet("transaction/{id}")]
        //public async Task<IActionResult> GetResellerTransactionById(int id)
        //{
        // var transaction = await _resellerRepository.GetTransactionByIdAsync(id);
        //  if (transaction == null)
        //  return NotFound();
        //  return Ok(transaction);
        //}


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateReseller([FromBody] ResellerCreateDto resellerCreateDto)
        {
            if(resellerCreateDto == null)
            {
                _response.IsSuccess=false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Reseller resellerToCreate = new()
            {
                Name = resellerCreateDto.Name,
                Phone = resellerCreateDto.Phone,
                Address = resellerCreateDto.Address,
              
            };
             await _db.AddAsync(resellerToCreate);
            await _db.SaveChangesAsync(); 
            _response.IsSuccess = true;
            _response.Result = resellerToCreate;
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetReseller", new { id = resellerToCreate.ResellerId }, _response);
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> CreateTransaction([FromBody] ResellerTransactionDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return BadRequest(new { message = "Transaction and items are required" });

            try
            {
                var createdTransaction = await _resellerRepository.AddTransactionAsync(dto);
                return CreatedAtAction(nameof(GetReseller),
                    new { id = dto.ResellerId }, createdTransaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("payments")]
        public async Task<IActionResult> AddPayment([FromBody] ResellerPaymentDto dto)
        {
            if (dto == null || dto.Amount <= 0)
                return BadRequest(new { message = "Valid payment details required" });

            try
            {
                var createdPayment = await _resellerRepository.AddPaymentAsync(dto);
                return Ok(createdPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("returns")]
        public async Task<IActionResult> AddReturn([FromBody] ResellerReturnDto dto)
        {
            if (dto == null || dto.Value <= 0)
                return BadRequest(new { message = "Return value must be > 0" });

            try
            {
                var createdReturn = await _resellerRepository.AddReturnAsync(dto);
                return Ok(createdReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


       

    }
}
