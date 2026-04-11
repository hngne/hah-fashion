namespace QA_TMDT.Dtos.Request
{
    public class AnhRequest
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        [StringLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự")]
        public string maSp { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "Phải chọn ít nhất một ảnh")]
        public List<IFormFile> AnhSPs { get; set; } = new List<IFormFile>();
    }
}
