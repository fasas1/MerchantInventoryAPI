using MechantInventory.Data;
using MechantInventory.Interface;
using MechantInventory.Repository.IRepository;
using MerchantInventory.Models;
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
        public CustomerController(ICustomerRepository customerRepository)
        {
            _response = new ApiResponse();
           _customerRepository = customerRepository;
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

    }
}
