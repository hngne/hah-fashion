namespace QA_TMDT.Dtos.Request
{
    public class VoucherRequest
    {
        [Required(ErrorMessage = "Mã voucher không được để trống")]
        [StringLength(50, ErrorMessage = "Mã voucher không được vượt quá 50 ký tự")]
        public string MaVoucher { get; set; } = null!;

        [Required(ErrorMessage = "Tên voucher không được để trống")]
        [StringLength(100, ErrorMessage = "Tên voucher không được vượt quá 100 ký tự")]
        public string TenVoucher { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Giảm giá phải lớn hơn 0")]
        public decimal GiamGia { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime? NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime? NgayKetThuc { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Điều kiện áp dụng không được nhỏ hơn 0")]
        public decimal? DieuKienApDung { get; set; }

        public string? MoTa { get; set; }
    }
}
