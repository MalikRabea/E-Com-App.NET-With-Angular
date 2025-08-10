using AutoMapper;
using E_Com.API.Helper;
using E_Com.Core.Entites;
using E_Com.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Com.API.Controllers
{

    public class BasketsController : BaseController
    {
        public BasketsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-basket-item/{id}")]
        public async Task<ActionResult<CustomerBasket>> get(string id)
        {
            var basket = await work.CustomerBasket.GetBasketAsync(id);
            if (basket is null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(basket);
        }
        [HttpPost("update-basket")]
        public async Task<ActionResult<CustomerBasket>> add(CustomerBasket basket)
        {
            var _basket = await work.CustomerBasket.UpdateBasketAsync(basket);
            return Ok(basket);
        }
        [HttpDelete("delete-basket-item/{id}")]
        public async Task<ActionResult<bool>> delete(string id)
        {
            var result = await work.CustomerBasket.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200))
                : BadRequest(new ResponseAPI(400));
        }
    }
}
