using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Model;
using QA_TMDT.Service;
using QA_TMDT.Utils;

namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhuongThucVCController : ControllerBase
    {
        private readonly IPhuongThucVCService _service;
        public PhuongThucVCController(IPhuongThucVCService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPhuongThucVC()
        {
            var data = await _service.GetPTVC();
            return Ok(APIResponse<IEnumerable<PhuongThucVc>>.OK("Lấy danh sách phương thức vận chuyển thành công", data));
        }
    }
}
