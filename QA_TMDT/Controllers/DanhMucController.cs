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
    public class DanhMucController : ControllerBase
    {
        private readonly IDanhMucService _service;
        public DanhMucController(IDanhMucService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getalldmasync()
        {
            var data = await _service.GetAllDM();
            return Ok(APIResponse<IEnumerable<DanhMucResponse>>.OK("Lấy danh sách danh mục thành công", data));
        }
        [HttpGet]
        [Route("by-madm")]
        public async Task<IActionResult> getdmbymadmqueryasync([FromQuery(Name = "id")] int? id)
        {
            if (id == null)
            {
                return BadRequest(APIResponse<DanhMucResponse>.Fail("Mã danh mục không được để trống"));
            }

            var data = await _service.GetDMByMaDM(id.Value);
            if(data == null)
            {
                return NotFound(APIResponse<DanhMucResponse>.Fail("Danh mục này không tồn tại"));
            }
            return Ok(APIResponse<DanhMucResponse>.OK("Lấy thông tin danh mục thành công", data));
        }
        [HttpGet]
        [Route("by-madm/{madm}")]
        public async Task<IActionResult> getdmbymadmasync(int madm)
        {
            var data = await _service.GetDMByMaDM(madm);
            if(data == null)
            {
                return NotFound(APIResponse<DanhMucResponse>.Fail("Danh mục này không tồn tại"));
            }
            return Ok(APIResponse<DanhMucResponse>.OK("Lấy thông tin danh mục thành công", data));
        }
        [HttpGet]
        [Route("by-tendm")]
        public async Task<IActionResult> getbytendmqueryasync([FromQuery(Name = "name")] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(APIResponse<IEnumerable<DanhMucResponse>>.Fail("Tên danh mục không được để trống"));
            }

            var data = await _service.GetDMByTenDM(name);
            if (data == null)
            {
                return NotFound(APIResponse<IEnumerable<DanhMucResponse>>.Fail("Không có danh mục nào"));
            }
            return Ok(APIResponse<IEnumerable<DanhMucResponse>>.OK("danh sách danh mục", data));
        }
        [HttpGet]
        [Route("by-tendm/{tendm}")]
        public async Task<IActionResult> getbytendmasync(string tendm)
        {
            var data = await _service.GetDMByTenDM(tendm);
            if (data == null)
            {
                return NotFound(APIResponse<IEnumerable<DanhMucResponse>>.Fail("Không có danh mục nào"));
            }
            return Ok(APIResponse<IEnumerable<DanhMucResponse>>.OK("danh sách danh mục", data));
        }
        [HttpPost]
        public async Task<IActionResult> postasync(DanhMucRequest request)
        {
            var data = await _service.PostDM(request);
            if (!data.success)
            {
                return BadRequest(APIResponse<DanhMucResponse>.Fail(data.message));
            }
            return Ok(APIResponse<DanhMucResponse>.OK(data.message, data.response));
        }
        [HttpPut]
        public async Task<IActionResult> updateasync(int madm,[FromBody] DanhMucRequest request)
        {
            var data = await _service.UpdateDM(madm, request);
            if (!data.success)
            {
                return BadRequest(APIResponse<DanhMucResponse>.Fail(data.message));
            }
            return Ok(APIResponse<DanhMucResponse>.OK(data.message, data.response));
        }
        [HttpDelete]
        public async Task<IActionResult> deleteasync(int madm)
        {
            var data = await _service.DeleteDM(madm);
            if (!data.success)
            {
                return NotFound(data.message);
            }
            return Ok(data.message);
        }
    }
}
