using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;
using QA_TMDT.Service;
using QA_TMDT.Utils;

namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhuongThucTTController : ControllerBase
    {
        private readonly IPhuongThucTTService _service;
        public PhuongThucTTController(IPhuongThucTTService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPhuongThucTT()
        {
            var data = await _service.GetPTTT();
            return Ok(APIResponse<IEnumerable<PhuongThucTt>>.OK("Lấy danh sách phương thức thành công", data));
        }
    }
}
