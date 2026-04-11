namespace QA_TMDT.Dtos.Request
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống")]
        [StringLength(255, ErrorMessage = "Mật khẩu cũ không được vượt quá 255 ký tự")]
        public string MatKhauCu { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [StringLength(255, ErrorMessage = "Mật khẩu mới không được vượt quá 255 ký tự")]
        public string MatKhauMoi { get; set; } = null!;
    }
}
