using Azure;
using MechantInventory.Data;
using MechantInventory.Interface;
using MechantInventory.Model;
using MechantInventory.Model.Dto;
using MechantInventory.Repository;
using MechantInventory.Repository.IRepository;
using MechantInventory.Utility;
using MerchantInventory.Models;
using MerchantInventory.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace MechantInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ApiResponse _response;
        private readonly ApplicationDbContext _db;
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository, ApplicationDbContext db)
        {
            _response = new ApiResponse();
            _customerRepository = customerRepository;
            _db =db;
        }
        [HttpGet]
        //[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
        public async Task<IActionResult> GetProducts(
                       [FromQuery] string search = "",
                       [FromQuery] int pageSize = 10,
                       [FromQuery] int pageNumber = 1
                    )
        {
            Expression<Func<Customer, bool>> filter = null;

            if (!string.IsNullOrEmpty(search))
            {
                filter = p => p.Name.ToLower().Contains(search.ToLower());

            }

            var pagedCustomers = await _customerRepository.GetAllAsync(

                    filter: filter,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );

            var customerDto = pagedCustomers.Items.Select(c => new CustomerDto
            {

                CustomerId = c.CustomerId,
                Name = c.Name,
                Address = c.Address,
                Notes = c.Notes,
                Email = c.Email,
                Phone  = c.Phone,
                CreatedAt = DateTime.Now,

            }).ToList();

            var response = new ApiResponse
            {
                Result = customerDto,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                TotalCount = pagedCustomers.TotalCount,
                PageSize = pagedCustomers.PageSize,
                PageNumber = pagedCustomers.PageNumber
            };
            return Ok(response);
        }



        [HttpGet("{id:guid}", Name ="GetCustomer")]
         public async Task<ActionResult<ApiResponse>> GetCustomer(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Customer customer = await _customerRepository.GetAsync(u => u.CustomerId == id);
                if (customer == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
            
                _response.IsSuccess = true;
                _response.Result = customer;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
              _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
              return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateCustomer([FromBody] CustomerCreateDto customerCreateDto)
        {
            try
            {
                if (customerCreateDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Customer customerToCreate = new()
                {
                    
                    Name = customerCreateDto.Name,
                    Email = customerCreateDto.Email,
                    Phone = customerCreateDto.Phone,
                    Address = customerCreateDto.Address,
                    Notes = customerCreateDto.Notes
                };
                await _customerRepository.CreateAsync(customerToCreate);
                await _db.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.Result = customerToCreate;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute(
                             "GetCustomer",
                     new { id = customerToCreate.CustomerId },
                           _response
                );
            }
            catch (Exception ex)
            {
                 _response.IsSuccess= false;
                _response.ErrorMessages = new List<string> { ex.Message };
             
            }
            return _response;
            
        }

        [HttpPut("{id:guid}", Name = "UpdateCustomer")]
        public async Task<ActionResult<ApiResponse>> UpdateCustomer(Guid id, [FromBody] CustomerUpdateDto customerUpdateDto)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Customer customerFromDb = await _customerRepository.GetAsync(c => c.CustomerId == id);
                if (customerFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                {
                    customerFromDb.Name = customerUpdateDto.Name;
                    customerFromDb.Email = customerUpdateDto.Email;
                    customerFromDb.Phone = customerUpdateDto.Phone;
                    customerFromDb.Address = customerUpdateDto.Address;
                    customerFromDb.Notes = customerUpdateDto.Notes;
                }

                await _customerRepository.UpdateAsync(customerFromDb);
               await _db.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                var inner = ex.InnerException?.Message ?? ex.Message;
                _response.ErrorMessages = new List<string> { inner };
                return StatusCode(500, _response);

            }
            
        }


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse>> DeleteCustomer(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Customer customerFromDb = await _customerRepository.GetAsync(c => c.CustomerId == id);
                if (customerFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                

                await _customerRepository.RemoveAsync(customerFromDb);
               
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };

            }
            return _response;
        }

    }
}
