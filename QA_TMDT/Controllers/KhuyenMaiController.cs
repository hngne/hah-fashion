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
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiService _service;
        public KhuyenMaiController(Service.IKhuyenMaiService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getallkm()
        {
            var data = await _service.GetAllKM();
            return Ok(APIResponse<IEnumerable<KhuyenMaiResponse>>.OK("Lấy danh sách khuyến mãi thành công", data));
        }
        [HttpGet]
        [Route("by-makm")]
        public async Task<IActionResult> getkmbymakmquery([FromQuery(Name = "id")] string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(APIResponse<KhuyenMaiResponse>.Fail("Mã khuyến mãi không được để trống"));
            }

            var data = await _service.GetKMByMaKM(id);
            if (data == null)
            {
                return NotFound(APIResponse<KhuyenMaiResponse>.Fail($"Không tìm thấy khuyến mãi có mã {id}"));
            }
            return Ok(APIResponse<KhuyenMaiResponse>.OK("Đã tìm thấy khuyến mãi", data));
        }
        [HttpGet]
        [Route("by-makm/{makm}")]
        public async Task<IActionResult> getkmbymakm(string makm)
        {
            var data = await _service.GetKMByMaKM(makm);
            if (data == null)
            {
                return NotFound(APIResponse<KhuyenMaiResponse>.Fail($"Không tìm thấy khuyến mãi có mã {makm}"));
            }
            return Ok(APIResponse<KhuyenMaiResponse>.OK("Đã tìm thấy khuyến mãi", data));
        }
        [HttpPost]
        public async Task<IActionResult> postkmasync(KhuyenMaiRequest request)
        {
            var data = await _service.PostKM(request);

            if (!data.success)
            {
                return BadRequest(APIResponse<KhuyenMaiResponse>.Fail(data.message));
            }

            return Ok(APIResponse<KhuyenMaiResponse>.OK(data.message, data.response));
        }
        [HttpPut]
        public async Task<IActionResult> putkmasync(KhuyenMaiRequest request)
        {
            var data = await _service.UpdateKM(request);

            if (!data.success)
            {
                return BadRequest(APIResponse<KhuyenMaiResponse>.Fail(data.message));
            }

            return Ok(APIResponse<KhuyenMaiResponse>.OK(data.message, data.response));
        }
        [HttpDelete]
        [Route("{makm}")]
        public async Task<IActionResult> deletekmasync(string makm)
        {
            var data = await _service.DeleteKM(makm);
            if (data.success)
            {
                return NotFound(data.message);
            }
            return Ok(data.message);
        }
        [HttpGet]
        [Route("chitiet/by-makm")]
        public async Task<IActionResult> getchitietbymakmquery([FromQuery(Name = "id")] string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(APIResponse<ChiTietKhuyenMaiResponse>.Fail("Mã khuyến mãi không được để trống"));
            }

            var data = await _service.GetChiTietKMByMaKM(id);
            if (data == null)
            {
                return NotFound(APIResponse<ChiTietKhuyenMaiResponse>.Fail("Không tồn tại mã khuyến mãi này"));
            }
            return Ok(APIResponse<IEnumerable<ChiTietKhuyenMaiResponse>>.OK("Danh sách chi tiết theo khuyến mãi", data));
        }
        [HttpGet]
        [Route("chitiet/by-makm/{makm}")]
        public async Task<IActionResult> getchitietbymakm(string makm)
        {
            var data = await _service.GetChiTietKMByMaKM(makm);
            if (data == null)
            {
                return NotFound(APIResponse<ChiTietKhuyenMaiResponse>.Fail("Không tồn tại mã khuyến mãi này"));
            }
            return Ok(APIResponse<IEnumerable<ChiTietKhuyenMaiResponse>>.OK("Danh sách chi tiết theo khuyến mãi", data));
        }
        [HttpGet]
        [Route("chitiet/by-masp")]
        public async Task<IActionResult> getchitietbymaspquery([FromQuery(Name = "id")] string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(APIResponse<ChiTietKhuyenMaiResponse>.Fail("Mã sản phẩm không được để trống"));
            }

            var data = await _service.GetChiTietKMByMaSP(id);
            if (data == null)
            {
                return NotFound(APIResponse<ChiTietKhuyenMaiResponse>.Fail("Không tồn tại mã sản phẩm này"));
            }
            return Ok(APIResponse<IEnumerable<ChiTietKhuyenMaiResponse>>.OK("Danh sách chi tiết theo sản phẩm", data));
        }
        [HttpGet]
        [Route("chitiet/by-masp/{masp}")]
        public async Task<IActionResult> getchitietbymasp(string masp)
        {
            var data = await _service.GetChiTietKMByMaSP(masp);
            if (data == null)
            {
                return NotFound(APIResponse<ChiTietKhuyenMaiResponse>.Fail("Không tồn tại mã sản phẩm này"));
            }
            return Ok(APIResponse<IEnumerable<ChiTietKhuyenMaiResponse>>.OK("Danh sách chi tiết theo sản phẩm", data));
        }
        [HttpPost]
        [Route("chitiet")]
        public async Task<IActionResult> postchitietasync(ChiTietKhuyenMaiRequest request)
        {
            var data = await _service.PostChiTietKM(request);

            if (!data.success)
            {
                return BadRequest(APIResponse<ChiTietKhuyenMaiResponse>.Fail(data.message));
            }

            return Ok(APIResponse<ChiTietKhuyenMaiResponse>.OK(data.message, data.response));
        }
        [HttpPut]
        [Route("chitiet")]
        public async Task<IActionResult> putchitietasync(ChiTietKhuyenMaiRequest request)
        {
            var (success, message, response) = await _service.UpdateChiTietKM(request);

            if (!success)
            {
                return BadRequest(APIResponse<ChiTietKhuyenMaiResponse>.Fail(message));
            }

            return Ok(APIResponse<ChiTietKhuyenMaiResponse>.OK(message, response));
        }
        [HttpDelete]
        [Route("chitiet/{makm}/{masp}")]
        public async Task<IActionResult> deletechitietasync(string makm, string masp)
        {
            var data = await _service.DeleteChiTietKM(makm, masp);

            if (!data.success)
            {
                return NotFound(data.message);
            }

            return Ok(data.message);
        }
    }
}
