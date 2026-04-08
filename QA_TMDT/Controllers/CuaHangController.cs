using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Service;
using QA_TMDT.Utils;

namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuaHangController : ControllerBase
    {
        private readonly ICuaHangService _service;
        public CuaHangController(ICuaHangService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getallcuahangasync()
        {
            var result = await _service.GetAllCuaHang();
            return Ok(APIResponse<IEnumerable<CuaHangResponse>>.OK("Lấy danh sách thành công", result));
        }
    }
}
