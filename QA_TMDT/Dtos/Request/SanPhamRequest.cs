using Microsoft.AspNetCore.Mvc;
using QA_TMDT.Helper;

namespace QA_TMDT.Dtos.Request
{
    public class SanPhamRequest
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
        public string MaSp { get; set; } = null!;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự")]
        public string TenSp { get; set; } = null!;

        [Required(ErrorMessage = "Mã danh mục không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã danh mục phải lớn hơn 0")]
        public int? MaDanhMuc { get; set; }

        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Giá gốc không được để trống")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Giá gốc không được nhỏ hơn 0")]
        public decimal? GiaGoc { get; set; }

        public string? ChatLieu { get; set; }

        public bool? TrangThai { get; set; }
        public List<IFormFile>? AnhSps { get; set; }

        [ModelBinder(BinderType = typeof(JsonModelBinder))]
        [MinLength(1, ErrorMessage = "Phải có ít nhất một biến thể sản phẩm")]
        public List<ChiTietSanPhamRequest> DsBienThe { get; set; } = new List<ChiTietSanPhamRequest>();
    }
}
