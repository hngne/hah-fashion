using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Service;
using QA_TMDT.Utils;
using System.Security.Claims;
namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangService _service;
        public DonHangController(IDonHangService service)
        {
            _service = service;
        }

        private string GetUserID()
        {
            return User.FindFirst("MaTaiKhoan")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        }

        [HttpGet]
        [Route("Admin-getAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> getall([FromQuery] string? maDonHang, [FromQuery] string? trangThai)
        {
            var result = await _service.GetAll(maDonHang, trangThai);
            return Ok(APIResponse<IEnumerable<DonHangResponse>>.OK("Lấy danh sách đơn hàng thành công", result!));
        }
        [HttpGet]
        [Route("get-by/{matk}")]
        public async Task<IActionResult> getbymatk()
        {
            var matk = GetUserID();
            var result = await _service.GetDonHangByMaTK(matk);
            if(result == null)
            {
                return BadRequest(APIResponse<IEnumerable<DonHangResponse>>.Fail("Không có tài khoản này"));
            }
            return Ok(APIResponse<IEnumerable<DonHangResponse>>.OK("Lấy danh sách đơn hàng thành công", result));
        }
        [HttpGet]
        [Route("get-dh-by/{maDH}")]
        public async Task<IActionResult> getbymaDH(string maDH)
        {
            var result = await _service.GetDonHangByMaDH(maDH);
            if (result == null)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail("Không có tài khoản này"));
            }
            return Ok(APIResponse<DonHangResponse>.OK("Lấy thông tin đơn hàng thành công", result));
        }

        [HttpPost]
        public async Task<IActionResult> createdonhangasync(DatHangRequest request)
        {
            var maTaiKhoan = GetUserID();
            if (string.IsNullOrWhiteSpace(maTaiKhoan))
            {
                return Unauthorized(APIResponse<DonHangResponse>.Fail("Không xác định được tài khoản từ token đăng nhập"));
            }

            request.MaTaiKhoan = maTaiKhoan;
            var result = await _service.CreateDonHang(request);
            if (!result.success)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail(result.message));
            }
            return Ok(APIResponse<DonHangResponse>.OK(result.message, result.response!));
        }

        [HttpPut]
        [Route("trangthai/{maDonHang}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> updatestatusasync(string maDonHang, [FromBody] UpdateTrangThaiDonRequest request)
        {
            var result = await _service.UpdateTrangThaiDonHang(maDonHang, request);
            if (!result.Success)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DonHangResponse>.OK(result.Message, result.response!));
        }

        [HttpPut]
        [Route("huydon/{maDonHang}")]
        public async Task<IActionResult> canceldonhangasync(string maDonHang, [FromBody] HuyDonRequest request)
        {
            var result = await _service.HuyDonHang(maDonHang, request);
            if (!result.Success)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DonHangResponse>.OK(result.Message, result.response!));
        }

        [HttpPut]
        [Route("diachi/{maDonHang}")]
        public async Task<IActionResult> updatediachiasync(string maDonHang, [FromBody] UpdateDiaChiRequest request)
        {
            var result = await _service.UpdateDiaChiGiaoHang(maDonHang, request);
            if (!result.Success)
            {
                return BadRequest(APIResponse<DonHangResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<DonHangResponse>.OK(result.Message, result.response!));
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> deleteasync(string maDH)
        {
            var result = await _service.DeleteDonHang(maDH);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
