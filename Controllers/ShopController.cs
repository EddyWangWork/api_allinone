using AutoMapper;
using demoAPI.BLL.Common;
using demoAPI.BLL.Shop;
using demoAPI.Middleware;
using demoAPI.Model;
using demoAPI.Model.DS.Shops;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
    [ResponseCompressionAttribute]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IShopBLL _shopBLL;

        public ShopController(IMapper mapper, IShopBLL shopBLL)
        {
            _mapper = mapper;
            _shopBLL = shopBLL;
        }

        #region Shop Diary

        [HttpGet]
        [Route("getShopDiaries")]
        public async Task<IActionResult> GetShopDiaries()
        {            
            var response = await _shopBLL.GetShopDiaries();
            return Ok(response);
        }

        [HttpGet]
        [Route("getShopDiariesByShop/{shopId}")]
        public async Task<IActionResult> getShopDiariesByShop(int shopId)
        {
            var response = await _shopBLL.GetShopDiaries(shopId);
            return Ok(response);
        }

        [HttpPost]
        [Route("addShopDiary")]
        public async Task<IActionResult> AddShopDiary(ShopDiaryAddReq req)
        {
            var response = await _shopBLL.AddShopDiary(req);
            return Ok(response);
        }

        [HttpPut("updateShopDiary/{id}")]
        public async Task<IActionResult> UpdateShopDiary(int id, ShopDiaryAddReq req)
        {
            var response = await _shopBLL.UpdateShopDiary(id, req);
            return Ok(response);
        }

        [HttpDelete("deleteShopDiary/{id}")]
        public async Task<IActionResult> DeleteShopDiary(int id)
        {
            var response = await _shopBLL.DeleteShopDiary(id);
            return Ok(response);
        }

        #endregion

        #region Shop

        [HttpGet]
        [Route("getShops")]
        public async Task<IActionResult> GetShops()
        {
            var response = await _shopBLL.GetShops();
            return Ok(response);
        }

        [HttpPost]
        [Route("addShop")]
        public async Task<IActionResult> AddShop(ShopAddReq req)
        {
            var response = await _shopBLL.AddShop(req);
            return Ok(response);
        }

        [HttpPut("updateShop/{id}")]
        public async Task<IActionResult> UpdateShop(int id, ShopAddReq req)
        {
            var response = await _shopBLL.UpdateShop(id, req);
            return Ok(response);
        }

        [HttpDelete("deleteShop/{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var response = await _shopBLL.DeleteShop(id);
            return Ok(response);
        }

        #endregion

        #region Shop Type

        [HttpGet]
        [Route("getShopTypes")]
        public async Task<IActionResult> GetShopTypes()
        {
            var response = await _shopBLL.GetShopTypes();
            return Ok(response);
        }

        [HttpPost]
        [Route("addShopType")]
        public async Task<IActionResult> AddShopType(ShopTypeAddReq req)
        {
            var response = await _shopBLL.AddShopType(req);
            return Ok(response);
        }

        [HttpPut("updateShopType/{id}")]
        public async Task<IActionResult> UpdateShopType(int id, ShopTypeAddReq req)
        {
            var response = await _shopBLL.UpdateShopType(id, req);
            return Ok(response);
        }

        [HttpDelete("deleteShopType/{id}")]
        public async Task<IActionResult> DeleteShopType(int id)
        {
            var response = await _shopBLL.DeleteShopType(id);
            return Ok(response);
        }

        #endregion


    }
}
