namespace QA_TMDT.Dtos.Request
{
    public class DatHangRequest
    {
        [StringLength(50, ErrorMessage = "Mã tài khoản không được vượt quá 50 ký tự")]
        public string MaTaiKhoan { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Phương thức thanh toán phải lớn hơn 0")]
        public int MaPTTT { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Phương thức vận chuyển phải lớn hơn 0")]
        public int MaPTVC { get; set; } 

        [StringLength(50, ErrorMessage = "Mã voucher không được vượt quá 50 ký tự")]
        public string? MaVoucher { get; set; }

        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        [StringLength(100, ErrorMessage = "Tên người nhận không được vượt quá 100 ký tự")]
        public string TenNguoiNhan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        [RegularExpression(@"^(0|\+84)\d{8,10}$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string soDienThoai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        public string DiaChiGiaoHang { get; set; } = null!;
    }
}
