namespace QA_TMDT.Dtos.Request
{
    public class CuaHangRequest
    {
        [Required(ErrorMessage = "Mã cửa hàng không được để trống")]
        [StringLength(50, ErrorMessage = "Mã cửa hàng không được vượt quá 50 ký tự")]
        public string MaCuaHang { get; set; } = null!;

        [Required(ErrorMessage = "Tên cửa hàng không được để trống")]
        [StringLength(100, ErrorMessage = "Tên cửa hàng không được vượt quá 100 ký tự")]
        public string TenCuaHang { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string DiaChi { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        [RegularExpression(@"^(0|\+84)\d{8,10}$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; } = string.Empty;

        public string? MoTa { get; set; }
    }
}
