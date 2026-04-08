using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Service;
using QA_TMDT.Utils;

namespace QA_TMDT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ThongKeController : ControllerBase
    {
        private readonly IThongKeService _service;

        public ThongKeController(IThongKeService service)
        {
            _service = service;
        }

        [HttpGet("doanh-thu")]
        public async Task<IActionResult> GetDoanhThu(DateTime? from, DateTime? to, string type = "day")
        {
            var f = from ?? DateTime.Now.AddMonths(-1);
            var t = to ?? DateTime.Now;
            type = string.IsNullOrWhiteSpace(type) ? "day" : type;
            var data = await _service.GetDoanhThu(f, t, type);
            return Ok(APIResponse<object>.OK("Thành công", data));
        }

        [HttpGet("top-ban-chay")]
        public async Task<IActionResult> GetTopBanChay(int top = 5)
        {
            var data = await _service.GetTopBanChay(top);
            return Ok(APIResponse<object>.OK("Thành công", data));
        }

        [HttpGet("top-e")]
        public async Task<IActionResult> GetTopE(int top = 5)
        {
            var data = await _service.GetTopBanE(top);
            return Ok(APIResponse<object>.OK("Thành công", data));
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcel(DateTime? from, DateTime? to, string type)
        {
            var f = from ?? DateTime.Now.AddMonths(-1);
            var t = to ?? DateTime.Now;
            var file = await _service.ExportExcelDoanhThu(f, t, type);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"BaoCao_{type}_{DateTime.Now.Ticks}.xlsx");
        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportPdf(DateTime? from, DateTime? to, string type)
        {
            var f = from ?? DateTime.Now.AddMonths(-1);
            var t = to ?? DateTime.Now;
            var file = await _service.ExportPdfDoanhThu(f, t, type);
            return File(file, "application/pdf", $"BaoCao_{type}_{DateTime.Now.Ticks}.pdf");
        }
    }
}
