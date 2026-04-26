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
    public class AnhController : ControllerBase
    {
        private readonly IAnhService _service;
        public AnhController(IAnhService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getallanh()
        {
            var data = await _service.GetAllAnh();
            return Ok(APIResponse<IEnumerable<AnhResponse>>.OK("Lấy danh sách ảnh thành công", data));
        }
        [HttpGet]
        [Route("by-masp")]
        public async Task<IActionResult> getanhbymaspquery([FromQuery(Name = "id")] string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(APIResponse<IEnumerable<AnhResponse>>.Fail("Mã sản phẩm không được để trống"));
            }

            var data = await _service.GetAnhByMaSp(id);
            if(data == null)
            {
                return NotFound(APIResponse<IEnumerable<AnhResponse>>.Fail("Sản phẩm không tồn tại"));
            }
            return Ok(APIResponse<IEnumerable<AnhResponse>>.OK("Lấy ảnh sản phẩm thành công", data));
        }
        [HttpGet]
        [Route("by-masp/{maSp}")]
        public async Task<IActionResult> getanhbymasp(string maSp)
        {
            var data = await _service.GetAnhByMaSp(maSp);
            if(data == null)
            {
                return NotFound(APIResponse<IEnumerable<AnhResponse>>.Fail("Sản phẩm không tồn tại"));
            }
            return Ok(APIResponse<IEnumerable<AnhResponse>>.OK("Lấy ảnh sản phẩm thành công", data));
        }
        [HttpPost]
        public async Task<IActionResult> addanh([FromForm] AnhRequest request)
        {
            var result = await _service.AddAnh(request);
            if (!result.success)
            {
                return BadRequest(APIResponse<IEnumerable<AnhResponse>>.Fail(result.message));
            }
            return Ok(APIResponse<IEnumerable<AnhResponse>>.OK(result.message, result.anhResponses));
        }
        [HttpDelete]
        public async Task<IActionResult> deleteanhbymaanh(int maAnh)
        {
            var result = await _service.DeleteAnhByMaAnh(maAnh);
            if (!result.success)
            {
                return BadRequest(APIResponse<string>.Fail(result.message));
            }
            return Ok(APIResponse<string>.OK(result.message, string.Empty));
        }
    }
}
