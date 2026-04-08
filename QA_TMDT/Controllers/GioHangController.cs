using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Service;
using QA_TMDT.Utils;

namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GioHangController : ControllerBase
    {
        private readonly IGioHangService _service;
        public GioHangController(IGioHangService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getgiohang(string matk)
        {
            var data = await _service.GetGioHangAsync(matk);
            if(data == null)
            {
                return BadRequest(APIResponse<GioHangResponse>.Fail("Giỏ hàng không tồn tại"));
            }
            return Ok(APIResponse<GioHangResponse>.OK("Lấy thông tin giỏ hàng thành công", data));
        }
        [HttpPost]
        [Route("Add-to-cart")]
        public async Task<IActionResult> addorupdategiohang(string matk, string mactsp, int soluongMua)
        {
            var data = await _service.AddToCartAsync(matk,mactsp, soluongMua);
            if(!data.Success)
            {
                return BadRequest(APIResponse<GioHangResponse>.Fail(data.Message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.Message, data.response!));
        }
        [HttpPost]
        [Route("Decrease-cart")]
        public async Task<IActionResult> decreasegiohang(string matk, string mactsp, int soluongGiam)
        {
            var data = await _service.DecreaseItemAsync(matk, mactsp, soluongGiam);
            if (!data.Success)
            {
                return BadRequest(APIResponse<GioHangResponse>.Fail(data.Message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.Message, data.response!));
        }
        [HttpPost]
        [Route("Update-value-cart")]
        public async Task<IActionResult> updatevaluecart(string matk, string mactsp, int soluongMoi)
        {
            var data = await _service.UpdateItemAsync(matk, mactsp, soluongMoi);
            if (!data.Success)
            {
                return BadRequest(APIResponse<GioHangResponse>.Fail(data.Message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.Message, data.response!));
        }
        [HttpDelete]
        [Route("Remove-item-cart")]
        public async Task<IActionResult> removeitemcart(string matk, string mactsp)
        {
            var data = await _service.RemoveItemAsync(matk,mactsp);
            if (!data.Success)
            {
                return BadRequest(APIResponse<GioHangResponse>.Fail(data.Message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.Message, data.response!));
        }
        [HttpDelete]
        [Route("Remove-all-items")]
        public async Task<IActionResult> removeallitem(string matk)
        {
            var data = await _service.ClearCartAsync(matk);
            if (!data.Success)
            {
                return BadRequest(APIResponse<GioHangResponse>.Fail(data.Message));
            }
            return Ok(APIResponse<GioHangResponse>.OK(data.Message, data.response!));
        }
    }
}
