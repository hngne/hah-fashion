using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Helper;

namespace QA_TMDT.Dtos.Request
{
    public class SanPhamRequest
    {
        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;
        public int? MaDanhMuc { get; set; }

        public string? MoTa { get; set; }

        public decimal GiaGoc { get; set; }

        public string? ChatLieu { get; set; }

        public bool? TrangThai { get; set; }
        public List<IFormFile>? AnhSps { get; set; }

        [ModelBinder(BinderType = typeof(JsonModelBinder))]
        public List<ChiTietSanPhamRequest> DsBienThe { get; set; } = new List<ChiTietSanPhamRequest>();
    }
}
