using MechantInventory.Data;
using MechantInventory.Interface;
using MechantInventory.Model;
using MechantInventory.Model.Dto;
using MechantInventory.Services;
using MechantInventory.Utility;
using MerchantInventory.Models;
using MerchantInventory.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace MechantApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IProductRepository _productRepository;
        private readonly CloudinaryService _cloudinaryService;
        private ApiResponse _response;
        public ProductController(CloudinaryService cloudinaryService,ApplicationDbContext db, IProductRepository productRepository)
        {
            _db = db;
            _cloudinaryService = cloudinaryService;
            _response = new ApiResponse();
            _productRepository = productRepository;
        }

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]       
        public async Task<IActionResult> GetProducts(
               [FromQuery] string search="",
               [FromQuery] int pageSize = 10,
               [FromQuery] int pageNumber = 1
            )
        {
            Expression<Func<Product, bool>> filter = null;

            if (!string.IsNullOrEmpty(search))
            {
                filter = p => p.Name.ToLower().Contains(search.ToLower());

            }

            var pagedProducts = await _productRepository.GetAllAsync(
                  
                    filter: filter,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );

            var productDto = pagedProducts.Items.Select(p => new ProductDto
            {
                
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                ImageUrl = p.ImageUrl,

            }).ToList();

            var response = new ApiResponse
            {
                Result = productDto,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                TotalCount = pagedProducts.TotalCount,
                PageSize = pagedProducts.PageSize,
                PageNumber = pagedProducts.PageNumber
            };
            return Ok(response);
        }






        [HttpGet("{id:int}", Name = "GetProduct")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
        public async Task<ActionResult<ApiResponse>> GetProduct(int id)
        {
           if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            Product product = await _productRepository.GetAsync(u => u.ProductId == id);
            if(product == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
               return NotFound(_response);
            }
            _response.Result = product;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }

       
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
        public async Task<ActionResult<ApiResponse>> CreateProduct([FromForm] ProductCreateDto productDto)
        {
            try
            {
                if (productDto.Image == null || productDto.Image.Length == 0)
                    return BadRequest("Image is required");

                var imageUrl = await _cloudinaryService.UploadImageAsync(productDto.Image);

                Product productToCreate = new()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Category = productDto.Category,
                    ImageUrl = imageUrl
                };

                await _db.Products.AddAsync(productToCreate);
                await _db.SaveChangesAsync();

                _response.Result = productToCreate;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetProduct", new { id = productToCreate.ProductId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return _response;
            }
        }

        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<ApiResponse>> UpdateProduct( int id, [FromForm] ProductUpdateDto productUpdateDTO)
        {
            try
            {
                if (productUpdateDTO == null || id != productUpdateDTO.ProductId)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var productFromDb = await _db.Products.FindAsync(id);
                if (productFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }

                // Upload new image to Cloudinary
                var imageUrl = await _cloudinaryService.UploadImageAsync(productUpdateDTO.Image);

                // Update fields
                productFromDb.Name = productUpdateDTO.Name;
                productFromDb.Price = productUpdateDTO.Price;
                productFromDb.Category = productUpdateDTO.Category;
                productFromDb.ImageUrl = imageUrl;

                _db.Products.Update(productFromDb);
                await _db.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }
        }


        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [Authorize(Roles = SD.Role_Admin)]

        public async Task<ActionResult<ApiResponse>> DeleteProduct(int id)
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
                Product productFromDb = await _db.Products.FindAsync(id);
                if (productFromDb == null)
                {
                    return BadRequest();
                }
                _db.Products.Remove(productFromDb);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest();
            }
               return _response;
        }
    }
}
