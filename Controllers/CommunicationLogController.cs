using MechantInventory.Data;
using MechantInventory.Model.Dto;
using MechantInventory.Model;
using MechantInventory.Repository;
using MechantInventory.Repository.IRepository;
using MerchantInventory.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MechantInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationLogController : ControllerBase
    {
        private ApiResponse _response;
        private readonly ApplicationDbContext _db;
        private readonly ICommunicationLogRepository _communicationLogRepository;

        public CommunicationLogController(ICommunicationLogRepository communicationLogRepository, ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
            _communicationLogRepository = communicationLogRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetCommunicationLog()
        {
            var communicationList = await _communicationLogRepository.GetAllAsync(includeProperties:"Customer");

            _response.Result = communicationList;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }

        [HttpGet("{id:guid}", Name = "GetCommunicationLog")]
        public async Task<ActionResult<ApiResponse>> GetCommunicationLog(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                CommunicationLog communicationLog = await _communicationLogRepository.GetAsync(u => u.CommunicationLogId == id);
                if (communicationLog == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.Result = communicationLog;
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
        public async Task<ActionResult<ApiResponse>> CreateCommunicationLog([FromBody] CommunicationCreateLogDto communicationLogDto)
        {
            try
            {
                if (communicationLogDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                CommunicationLog communicationToCreate = new()
                {

                    CustomerId = communicationLogDto.CustomerId,
                    Date = DateTime.UtcNow,
                    Type = communicationLogDto.Type,
                    Summary = communicationLogDto.Summary
                
                };
                await _communicationLogRepository.CreateAsync(communicationToCreate);
                await _db.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.Result = communicationToCreate;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute(
                             "GetCommunicationLog",
                     new { id = communicationToCreate.CommunicationLogId },
                           _response
                );
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
