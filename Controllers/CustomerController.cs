using Azure;
using MechantInventory.Data;
using MechantInventory.Interface;
using MechantInventory.Model;
using MechantInventory.Model.Dto;
using MechantInventory.Repository.IRepository;
using MerchantInventory.Models;
using MerchantInventory.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetCustomers()
        {
            var customerList = await _customerRepository.GetAllAsync();

            _response.Result = customerList;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
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
                _response.ErrorMessages = new List<string> { ex.Message };

            }
            return _response;
        }


        [HttpDelete("{id:guid}", Name = "DeleteCustomer")]
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
