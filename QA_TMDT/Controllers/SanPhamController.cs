using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Helper;
using QA_TMDT.Service;
using QA_TMDT.Utils;

namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamService _service;
        public SanPhamController(ISanPhamService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("GetAllSP")]
        public async Task<IActionResult> getallsp([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllSP(page, pageSize);
            return Ok(APIResponse<PageResult<SanPhamResponse>>.OK("Lấy danh sách sản phẩm thành công", result));
        }
        [HttpGet]
        [Route("Get-by-maDM/{maDM}")]
        public async Task<IActionResult> getbymadm([FromRoute] int maDM, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetByMaDM(maDM, page, pageSize);
            return Ok(APIResponse<PageResult<SanPhamResponse>>.OK("Lấy danh sách sản phẩm theo mã danh mục thành công", result));
        }
        [HttpGet]
        [Route("Get-by-tenSP/{tenSP}")]
        public async Task<IActionResult> getbytensp([FromRoute] string tenSP, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetByTenSP(tenSP, page, pageSize);
            return Ok(APIResponse<PageResult<SanPhamResponse>>.OK("Lấy danh sách sản phẩm theo tên sản phẩm thành công", result));
        }
        [HttpGet]
        [Route("Get-fullinfo-by-maSP/{maSP}")]
        public async Task<IActionResult> getfullinfobymasp([FromRoute] string maSP)
        {
            var result = await _service.GetFullInFoByMaSP(maSP);
            if (result == null)
            {
                return NotFound(APIResponse<SanPhamResponse>.Fail("Sản phẩm không tồn tại"));
            }
            return Ok(APIResponse<SanPhamResponse>.OK("Lấy thông tin sản phẩm thành công", result));
        }
        [HttpGet]
        [Route("Get-chi-tiet-sp-by-maCTSP/{maCTSP}")]
        public async Task<IActionResult> getctspbymactsp([FromRoute] string maCTSP)
        {
            var result = await _service.GetChiTietSPByMaCTSP(maCTSP);
            if (result == null)
            {
                return NotFound(APIResponse<ChiTietSanPhamResponse>.Fail("Chi tiết sản phẩm không tồn tại"));
            }
            return Ok(APIResponse<ChiTietSanPhamResponse>.OK("Lấy thông tin chi tiết sản phẩm thành công", result));
        }
        [HttpPost]
        [Route("Create-SP")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> createsp([FromForm] SanPhamRequest request)
        {
            var result = await _service.CreateSP(request);
            if (!result.success)
            {
                return BadRequest(APIResponse<SanPhamResponse>.Fail(result.message));
            }
            return Ok(APIResponse<SanPhamResponse>.OK(result.message, result.response!));
        }
        [HttpPut]
        [Route("Update-stock-ctsp/{maCTSP}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> updatestockctsp([FromRoute] string maCTSP, [FromQuery] int soluongthaydoi)
        {
            var result = await _service.UpdateStockCTSP(maCTSP, soluongthaydoi);
            if (!result.success)
            {
                return BadRequest(APIResponse<ChiTietSanPhamResponse>.Fail(result.message));
            }
            return Ok(APIResponse<ChiTietSanPhamResponse>.OK(result.message, result.response!));
        }
        [HttpPut]
        [Route("Update-CTSP")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> updatectsp([FromBody] UpdateCTSPRequest request)
        {
            var result = await _service.UpdateCTSP(request);
            if (!result.success)
            {
                return BadRequest(APIResponse<ChiTietSanPhamResponse>.Fail(result.message));
            }
            return Ok(APIResponse<ChiTietSanPhamResponse>.OK(result.message, result.response!));
        }
        [HttpPut]
        [Route("Update-SP")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> updatesp([FromForm] SanPhamRequest request)
        {
            var result = await _service.UpdateSP(request);
            if (!result.success)
            {
                return BadRequest(APIResponse<SanPhamResponse>.Fail(result.message));
            }
            return Ok(APIResponse<SanPhamResponse>.OK(result.message, result.response!));
        }
        [HttpDelete]
        [Route("Delete-SP/{maSP}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> deletesp([FromRoute] string maSP)
        {
            var result = await _service.DeleteSP(maSP);
            if (!result.success)
            {
                return BadRequest(APIResponse<string>.Fail(result.message));
            }
            return Ok(APIResponse<string>.OK(result.message, string.Empty));
        }
    }
}
