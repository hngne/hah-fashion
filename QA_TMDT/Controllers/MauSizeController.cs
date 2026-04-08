using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QA_TMDT.Model;
using QA_TMDT.Utils;

namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauSizeController : ControllerBase
    {
        private readonly QaTmdtContext _context;
        public MauSizeController(QaTmdtContext context)
        {
            _context = context;
        }

        // ========== MÀU SẮC ==========

        [HttpGet("mau")]
        public async Task<IActionResult> GetAllMau()
        {
            var data = await _context.MauSacs.AsNoTracking().ToListAsync();
            return Ok(APIResponse<IEnumerable<MauSac>>.OK("Lấy danh sách màu sắc thành công", data));
        }

        [HttpPost("mau")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateMau([FromBody] MauSac request)
        {
            if (string.IsNullOrWhiteSpace(request.TenMau))
                return BadRequest(APIResponse<string>.Fail("Tên màu không được để trống"));

            var exist = await _context.MauSacs.AnyAsync(m => m.TenMau.ToLower() == request.TenMau.ToLower());
            if (exist) return BadRequest(APIResponse<string>.Fail("Màu sắc này đã tồn tại"));

            var mau = new MauSac { TenMau = request.TenMau, MaHex = request.MaHex };
            _context.MauSacs.Add(mau);
            await _context.SaveChangesAsync();
            return Ok(APIResponse<MauSac>.OK("Thêm màu sắc thành công", mau));
        }

        [HttpPut("mau/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateMau(int id, [FromBody] MauSac request)
        {
            var mau = await _context.MauSacs.FindAsync(id);
            if (mau == null) return NotFound(APIResponse<string>.Fail("Không tìm thấy màu sắc"));

            mau.TenMau = request.TenMau;
            mau.MaHex = request.MaHex;
            await _context.SaveChangesAsync();
            return Ok(APIResponse<MauSac>.OK("Cập nhật màu sắc thành công", mau));
        }

        [HttpDelete("mau/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteMau(int id)
        {
            var mau = await _context.MauSacs.FindAsync(id);
            if (mau == null) return NotFound(APIResponse<string>.Fail("Không tìm thấy màu sắc"));

            var used = await _context.ChiTietSps.AnyAsync(ct => ct.MaMauSac == id);
            if (used) return BadRequest(APIResponse<string>.Fail("Không thể xóa, màu sắc đang được sử dụng trong sản phẩm"));

            _context.MauSacs.Remove(mau);
            await _context.SaveChangesAsync();
            return Ok(APIResponse<string>.OK("Xóa màu sắc thành công", ""));
        }

        // ========== KÍCH THƯỚC ==========

        [HttpGet("size")]
        public async Task<IActionResult> GetAllSize()
        {
            var data = await _context.KichThuocs.AsNoTracking().ToListAsync();
            return Ok(APIResponse<IEnumerable<KichThuoc>>.OK("Lấy danh sách kích thước thành công", data));
        }

        [HttpPost("size")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateSize([FromBody] KichThuoc request)
        {
            if (string.IsNullOrWhiteSpace(request.TenSize))
                return BadRequest(APIResponse<string>.Fail("Tên size không được để trống"));

            var exist = await _context.KichThuocs.AnyAsync(s => s.TenSize.ToLower() == request.TenSize.ToLower());
            if (exist) return BadRequest(APIResponse<string>.Fail("Kích thước này đã tồn tại"));

            var size = new KichThuoc { TenSize = request.TenSize };
            _context.KichThuocs.Add(size);
            await _context.SaveChangesAsync();
            return Ok(APIResponse<KichThuoc>.OK("Thêm kích thước thành công", size));
        }

        [HttpPut("size/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateSize(int id, [FromBody] KichThuoc request)
        {
            var size = await _context.KichThuocs.FindAsync(id);
            if (size == null) return NotFound(APIResponse<string>.Fail("Không tìm thấy kích thước"));

            size.TenSize = request.TenSize;
            await _context.SaveChangesAsync();
            return Ok(APIResponse<KichThuoc>.OK("Cập nhật kích thước thành công", size));
        }

        [HttpDelete("size/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            var size = await _context.KichThuocs.FindAsync(id);
            if (size == null) return NotFound(APIResponse<string>.Fail("Không tìm thấy kích thước"));

            var used = await _context.ChiTietSps.AnyAsync(ct => ct.MaKichThuoc == id);
            if (used) return BadRequest(APIResponse<string>.Fail("Không thể xóa, kích thước đang được sử dụng trong sản phẩm"));

            _context.KichThuocs.Remove(size);
            await _context.SaveChangesAsync();
            return Ok(APIResponse<string>.OK("Xóa kích thước thành công", ""));
        }
    }
}
