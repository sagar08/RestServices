using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Dtos;
using JWTPolicyBasedAuthorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWTPolicyBasedAuthorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Policy = Constants.Policy.UserPolicy)]
    public class ProductsController : ControllerBase
    {
        #region Member Variables
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepo;

        #endregion

        #region Constructor
        public ProductsController(IMapper mapper, IProductRepository repo)
        {
            _mapper = mapper;
            _productRepo = repo;
        }
        #endregion

        #region API Methods
        [HttpGet]
        //[Authorize(Policy = Constants.Permission.CanView)]
        //[Route("")]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var products = await _productRepo.GetProducts();
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = await _productRepo.GetProductById(id);

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
        #endregion
    }
}