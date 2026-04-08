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
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _service;

        public VoucherController(IVoucherService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> getallvoucher()
        {
            var data = await _service.GetAllVouchers();
            return Ok(APIResponse<IEnumerable<VoucherResponse>>.OK("Lấy danh sách voucher thành công", data));
        }
        [HttpGet]
        [Route("by-mavoucher/{mavoucher}")]
        public async Task<IActionResult> getvoucherbymavoucher(string mavoucher)
        {
            var data = await _service.GetVoucherByCode(mavoucher);
            if(data == null)
            {
                return NotFound(APIResponse<VoucherResponse>.Fail("Không có mã voucher này"));
            }
            return Ok(APIResponse<VoucherResponse>.OK("Tìm thấy voucher", data));
        }
        [HttpPost]
        public async Task<IActionResult> createvoucher(VoucherRequest request)
        {
            var result = await _service.CreateVoucher(request);
            if (!result.Success)
            {
                return BadRequest(APIResponse<VoucherResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<VoucherResponse>.OK(result.Message, result.response));
        }
        [HttpPut]
        public async Task<IActionResult> updatevoucher(VoucherRequest request)
        {
            var result = await _service.UpdateVoucher(request);
            if (!result.Success)
            {
                return BadRequest(APIResponse<VoucherResponse>.Fail(result.Message));
            }
            return Ok(APIResponse<VoucherResponse>.OK(result.Message, result.response));
        }
        [HttpDelete]
        public async Task<IActionResult> deletevoucher(string maVoucher)
        {
            var result = await _service.DeleteVoucher(maVoucher);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
        [HttpGet]
        [Route("avaiable-user-canuse/{maTK}")]
        public async Task<IActionResult> getavaiablevouchers(string maTK)
        {
            var data = await _service.GetVouchersForUser(maTK);

            return Ok(APIResponse<IEnumerable<VoucherResponse>>.OK("Lấy thành công danh sách voucher tài khoản được phép sử dụng", data));
        }
    }
}
