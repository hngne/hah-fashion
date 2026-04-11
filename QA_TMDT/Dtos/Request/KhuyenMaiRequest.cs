namespace QA_TMDT.Dtos.Request
{
    public class KhuyenMaiRequest
    {
        [Required(ErrorMessage = "Mã khuyến mãi không được để trống")]
        [StringLength(50, ErrorMessage = "Mã khuyến mãi không được vượt quá 50 ký tự")]
        public string MaKhuyenMai { get; set; } = null!;

        [Required(ErrorMessage = "Tên khuyến mãi không được để trống")]
        [StringLength(100, ErrorMessage = "Tên khuyến mãi không được vượt quá 100 ký tự")]
        public string TenKhuyenMai { get; set; } = null!;

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime? NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime? NgayKetThuc { get; set; }

        public string? MoTa { get; set; }
    }
}
