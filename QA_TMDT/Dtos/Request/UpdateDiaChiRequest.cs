namespace QA_TMDT.Dtos.Request
{
    public class UpdateDiaChiRequest
    {
        [Required(ErrorMessage = "Mã tài khoản không được để trống")]
        [StringLength(50, ErrorMessage = "Mã tài khoản không được vượt quá 50 ký tự")]
        public string MaTaiKhoan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ mới không được để trống")]
        public string DiaChiMoi { get; set; } = string.Empty;
    }
}
