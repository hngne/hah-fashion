namespace QA_TMDT.Dtos.Request
{
    public class AddBienTheRequest
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
        public string MaSP { get; set; } = null!;

        [Required(ErrorMessage = "Kích thước không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Kích thước phải lớn hơn 0")]
        public int? MaKichThuoc { get; set; }

        [Required(ErrorMessage = "Màu sắc không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Màu sắc phải lớn hơn 0")]
        public int? MaMauSac { get; set; }

        [Required(ErrorMessage = "Số lượng tồn không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được nhỏ hơn 0")]
        public int? SoLuongTon { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Giá bán không được nhỏ hơn 0")]
        public decimal? GiaBan { get; set; }
    }
}
