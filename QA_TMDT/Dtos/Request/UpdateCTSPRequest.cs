namespace QA_TMDT.Dtos.Request
{
    public class UpdateCTSPRequest
    {
        [Required(ErrorMessage = "Mã chi tiết sản phẩm không được để trống")]
        [StringLength(50, ErrorMessage = "Mã chi tiết sản phẩm không được vượt quá 50 ký tự")]
        public string MaChiTietSp { get; set; } = null!;

        [Required(ErrorMessage = "Số lượng tồn không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được nhỏ hơn 0")]
        public int? SoLuongTon { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Giá bán không được nhỏ hơn 0")]
        public decimal? GiaBan { get; set; }
    }
}
