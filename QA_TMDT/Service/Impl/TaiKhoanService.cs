using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;
using QA_TMDT.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using QA_TMDT.Mapper;

namespace QA_TMDT.Service.Impl
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly ITaiKhoanRepository _repo;
        private readonly PasswordHasher<TaiKhoan> _hasher = new PasswordHasher<TaiKhoan>();
        private readonly IConfiguration _config;
        public TaiKhoanService(ITaiKhoanRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        private string generateToken(TaiKhoan tk)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var signature = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("MaTaiKhoan", tk.MaTaiKhoan),
                new Claim(ClaimTypes.Name, tk.TenDangNhap),
                new Claim(ClaimTypes.Role, tk.VaiTro ?? "user"),
                new Claim("HoTen", tk.HoTen ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signature
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<(bool, string, DangNhapResponse?)> DangKy(DangKyRequest request)
        {
            var exist = await _repo.GetTKByTenDangNhap(request.TenDangNhap);
            if (exist != null)
            {
                return (false, "Tên đăng nhập đã tồn tại", null);
            }
            var tk = new TaiKhoan();
            tk.MaTaiKhoan = Guid.NewGuid().ToString();
            tk.TenDangNhap = request.TenDangNhap;
            tk.Email = request.Email;
            tk.HoTen = request.HoTen;
            tk.SoDienThoai = request.SoDienThoai;
            tk.VaiTro = "user";
            tk.DiaChi = request.DiaChi;
            tk.MatKhau = _hasher.HashPassword(tk, request.MatKhau);

            var success = await _repo.AddAsync(tk);
            if (!success)
            {
                return (false, "Có lỗi xảy ra khi tạo tài khoản", null);
            }
            await _repo.SaveChangeAsync();
            var response = new DangNhapResponse()
            {
                MaTaiKhoan = tk.MaTaiKhoan,
                TenDangNhap = tk.TenDangNhap,
                Email = tk.Email,
                HoTen = tk.HoTen,
                SoDienThoai = tk.SoDienThoai,
                VaiTro = tk.VaiTro,
                DiaChi = tk.DiaChi,
                Token = ""
            };
            return (true, "Đăng ký thành công", response);
        }
        public async Task<(bool, string, DangNhapResponse?)> DangNhap(DangNhapRequest request)
        {
            var exist = await _repo.GetTKByTenDangNhap(request.TenDangNhap);
            if (exist == null)
            {
                return (false, "Tên đăng nhập hoặc mật khẩu không chính xác", null);
            }

            var verify = _hasher.VerifyHashedPassword(exist, exist.MatKhau, request.MatKhau);
            if(verify == PasswordVerificationResult.Failed)
            {
                return (false, "Tên đăng nhập hoặc mật khẩu không chính xác", null);
            }
            var Token = generateToken(exist);
            return (true, "Đăng nhập thành công", new DangNhapResponse()
            {
                MaTaiKhoan = exist.MaTaiKhoan,
                TenDangNhap = exist.TenDangNhap,
                Email = exist.Email,
                HoTen = exist.HoTen,
                SoDienThoai = exist.SoDienThoai,
                VaiTro = exist.VaiTro,
                DiaChi = exist.DiaChi,
                Token = Token
            });
        }
        public async Task<IEnumerable<TaiKhoanResponse>> GetAllTK()
        {
            var result = await _repo.GetAllTK();
            return result.Select(TaiKhoanBuilder.ToResponse);
        }
        public async Task<TaiKhoanResponse?> GetTKByMaTK(string maTK)
        {
            var check = await _repo.CheckTKExist(maTK);
            if (!check)
            {
                return null;
            }
            var result = await _repo.GetTKByMaTK(maTK);
            return TaiKhoanBuilder.ToResponse(result!);
        }
        public async Task<(bool, string, TaiKhoanResponse?)> UpdateProfile(string maTK, UpdateProfile request)
        {
            var check = await _repo.CheckTKExist(maTK);
            if (!check)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var exist = await _repo.GetTKByMaTK(maTK);
            exist.HoTen = request.HoTen;
            exist.SoDienThoai = request.SoDienThoai;
            exist.DiaChi = request.DiaChi;
            exist.Email = request.Email;
            var success = await _repo.UpdateTK(exist);
            if (!success)
            {
                return (false, "Có lỗi khi cập nhật thông tin", null);
            }
            return (true, "Cập nhật thông tin thành công", TaiKhoanBuilder.ToResponse(exist));
        }
        public async Task<(bool, string, TaiKhoanResponse?)> ChangePassword(string maTK, ChangePasswordRequest request)
        {
            var check = await _repo.CheckTKExist(maTK);
            if (!check)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var exist = await _repo.GetTKByMaTK(maTK);

            var verify = _hasher.VerifyHashedPassword(exist, exist.MatKhau, request.MatKhauCu);
            if(verify == PasswordVerificationResult.Failed)
            {
                return (false, "Mật khẩu cũ không chính xác", null);
            }

            exist.MatKhau = _hasher.HashPassword(exist, request.MatKhauMoi);

            var success = await _repo.UpdateTK(exist);
            if (!success)
            {
                return (false, "Có lỗi khi đổi mật khẩu", null);
            }
            return (true, "Đổi mật khẩu thành công", TaiKhoanBuilder.ToResponse(exist));
        }
        public async Task<(bool, string, TaiKhoanResponse?)> UpdateRole(string maTK, string newRole)
        {
            var check = await _repo.CheckTKExist(maTK);
            if (!check)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            if (newRole.ToLower() != "admin" && newRole.ToLower() != "user")
                return (false, "Role không hợp lệ (chỉ chấp nhận 'admin' hoặc 'user')", null);
            var exist = await _repo.GetTKByMaTK(maTK);
            exist.VaiTro = newRole;

            var success = await _repo.UpdateTK(exist);
            if (!success)
            {
                return (false, "Có lỗi khi cập nhật tài khoản", null);
            }
            return (true, "Cập nhật thông tin thành công", TaiKhoanBuilder.ToResponse(exist));
        }
        public async Task<(bool, string)> DeleteOrLockTK(string maTK)
        {
            var result = await _repo.DeleteOrLock(maTK);
            if (!result)
            {
                return (false, "Tài khoản có thể không tồn tại hoặc đã có đơn hàng");
            }
            return (true, "Xóa tài khoản thành công");
        }
    }
}
