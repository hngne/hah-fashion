using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class TaiKhoanController : ControllerBase
    {
        private readonly ITaiKhoanService _service;
        public TaiKhoanController(ITaiKhoanService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("admin-getall-acc")]
        public async Task<IActionResult> getalltk()
        {
            var result = await _service.GetAllTK();
            return Ok(APIResponse<IEnumerable<TaiKhoanResponse>>.OK("Lấy danh sách tài khoản thành công", result));
        }
        [HttpGet]
        [Route("getInfo-acc/{maTK}")]
        public async Task<IActionResult> gettkbymatk(string maTK)
        {
            var result = await _service.GetTKByMaTK(maTK);
            if(result == null)
            {
                return NotFound(APIResponse<TaiKhoanResponse>.Fail("Không có tài khoản này"));
            }
            return Ok(APIResponse<TaiKhoanResponse>.OK("Lấy thông tin tài khoản thành công", result));
        }
        [HttpPut]
        [Route("update-profile/{maTK}")]
        public async Task<IActionResult> updateprofile(string maTK, [FromBody] UpdateProfile request)
        {
            var result = await _service.UpdateProfile(maTK, request);
            if (!result.success)
            {
                return NotFound(APIResponse<TaiKhoanResponse>.Fail(result.message));
            }
            return Ok(APIResponse<TaiKhoanResponse>.OK(result.message, result.response!));
        }
        [HttpPut("change-password/{maTK}")]
        public async Task<IActionResult> ChangePassword(string maTK, [FromBody] ChangePasswordRequest req)
        {
            var result = await _service.ChangePassword(maTK, req);
            if (!result.success)
                return BadRequest(APIResponse<TaiKhoanResponse>.Fail(result.message));

            return Ok(APIResponse<TaiKhoanResponse>.OK(result.message, result.response!));
        }

        [HttpPut("admin/role/{maTK}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRole(string maTK, [FromQuery][Required(ErrorMessage = "Vai trò mới không được để trống")] string newRole)
        {
            var result = await _service.UpdateRole(maTK, newRole);
            if (!result.success)
                return BadRequest(APIResponse<TaiKhoanResponse>.Fail(result.message));

            return Ok(APIResponse<TaiKhoanResponse>.OK(result.message, result.response!));
        }

        [HttpDelete("admin/{maTK}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string maTK)
        {
            var result = await _service.DeleteOrLockTK(maTK);
            if (!result.success)
                return BadRequest(APIResponse<string>.Fail(result.meesage));

            return Ok(APIResponse<string>.OK(result.meesage, string.Empty));
        }
    }
}
