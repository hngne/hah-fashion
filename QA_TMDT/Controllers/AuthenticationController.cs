using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Service;
using QA_TMDT.Utils;

namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITaiKhoanService _service;
        public AuthenticationController(ITaiKhoanService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> DangNhap([FromBody] DangNhapRequest request)
        {
            var result = await _service.DangNhap(request);
            if(!result.success)
            {
                return BadRequest(APIResponse<DangNhapResponse>.Fail(result.message));
            }
            return Ok(APIResponse<DangNhapResponse>.OK(result.message, result.response!));
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> DangKy([FromBody] DangKyRequest request)
        {
            var result = await _service.DangKy(request);
            if (!result.success)
            {
                return BadRequest(APIResponse<DangNhapResponse>.Fail(result.message));
            }
            return Ok(APIResponse<DangNhapResponse>.OK(result.message, result.response!));
        }
    }
}
