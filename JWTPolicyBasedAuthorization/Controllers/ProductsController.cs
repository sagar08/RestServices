using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Dtos;
using JWTPolicyBasedAuthorization.Infrastructure;
using JWTPolicyBasedAuthorization.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWTPolicyBasedAuthorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Policy = Constants.Policy.UserPolicy)]
    public class ProductsController : ControllerBase
    {
        #region Member Variables
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;

        #endregion

        #region Constructor
        public ProductsController(ILoggerManager logger, IMapper mapper, IUnitOfWork repo)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repo;
        }
        #endregion

        #region API Methods
        [HttpGet]
        //[Authorize(Policy = Constants.Permission.CanView)]
        //[Route("")]
        public async Task<IActionResult> Get()
        {
            var products = await _repository.Products.GetProducts();
            var data = _mapper.Map<IEnumerable<ProductDetailDto>>(products);

            return Ok(new ResponseDto<IEnumerable<ProductDetailDto>>
            {
                Data = data,
                Status = "OK",
                Message = $"Records found {products.Count}"
            });
        }

        [HttpGet]
        //[Authorize(Policy = Constants.Permission.CanView)]        
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repository.Products.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            var data = _mapper.Map<ProductDetailDto>(product);

            return Ok(new ResponseDto<ProductDetailDto>
            {
                Data = data,
                Status = "OK",
                Message = $"Records found {product.Name}"
            });
        }

        [HttpPost]
        [ServiceFilter(typeof(AsyncValidationBadRequestFilter))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            _repository.Products.CreateProduct(product);
            await _repository.SaveChangesAsync();

            //var productDetail = _repository.Products.GetProductById(product.Id);
            var productDetailDto = _mapper.Map<ProductDetailDto>(product);

            return Created("GetProduct",
                new ResponseDto<ProductDetailDto>
                {
                    Data = productDetailDto,
                    Status = "Success",
                    Message = "Product created successfully!"
                }
            );
        }

        [HttpPut]
        //[ServiceFilter(typeof(AsyncValidateEntityExistsFilter<ProductDto>), Order = 2)]
        [ServiceFilter(typeof(AsyncValidationBadRequestFilter), Order = 1)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            #region commented code
            // if (productDto == null)
            // {
            //     _logger.LogError("Object is null");
            //     return BadRequest("Object is null");
            // }

            // if (!ModelState.IsValid)
            // {
            //     _logger.LogError("Invalid product object sent from client.");
            //     return BadRequest("Invalid model object");
            // }

            #endregion
            
            var productExists = _repository.Products.GetProductById(productDto.Id);
            if(productExists == null)
            {
                return NotFound();
            }
            var product = _mapper.Map<Product>(productDto);

            _repository.Products.UpdateProduct(product);
            await _repository.SaveChangesAsync();

            var productDetail = _mapper.Map<ProductDetailDto>(product);

            return NoContent();
        }
        #endregion
    }
}