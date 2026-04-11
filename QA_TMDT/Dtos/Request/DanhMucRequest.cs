namespace QA_TMDT.Dtos.Request
{
    public class DanhMucRequest
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
        public string TenDanhMuc { get; set; } = null!;

        public string? MoTa { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Mã danh mục cha phải lớn hơn 0")]
        public int? MaDanhMucCha { get; set; }
    }
}
