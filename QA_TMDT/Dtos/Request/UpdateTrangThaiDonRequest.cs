namespace QA_TMDT.Dtos.Request
{
    public class UpdateTrangThaiDonRequest
    {
        [Required(ErrorMessage = "Trạng thái mới không được để trống")]
        [StringLength(50, ErrorMessage = "Trạng thái mới không được vượt quá 50 ký tự")]
        public string TrangThaiMoi { get; set; } = string.Empty;
    }
}
