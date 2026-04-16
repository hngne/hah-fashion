using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class TaiKhoanBuilder
    {
        public static TaiKhoanResponse ToResponse(TaiKhoan tk)
        {
            return new TaiKhoanResponse
            {
                MaTaiKhoan = tk.MaTaiKhoan,
                TenDangNhap = tk.TenDangNhap,
                Email = tk.Email,
                HoTen = tk.HoTen,
                SoDienThoai = tk.SoDienThoai,
                VaiTro = tk.VaiTro,
                DiaChi = tk.DiaChi,
                TrangThai = true,
            };
        }
    }
}
