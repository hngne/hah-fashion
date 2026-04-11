namespace QA_TMDT.Dtos.Request
{
    public class ChiTietKhuyenMaiRequest
    {
        [Required(ErrorMessage = "Mã khuyến mãi không được để trống")]
        [StringLength(50, ErrorMessage = "Mã khuyến mãi không được vượt quá 50 ký tự")]
        public string MaKhuyenMai { get; set; } = null!;

        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
        public string MaSp { get; set; } = null!;

        [Range(1, 99, ErrorMessage = "Phần trăm giảm phải từ 1 đến 99")]
        public int PhanTramGiam { get; set; }
    }
}
