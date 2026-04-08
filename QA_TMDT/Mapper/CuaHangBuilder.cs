using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class CuaHangBuilder
    {
        public static CuaHangResponse ToResponse(CuaHang ch)
        {
            return new CuaHangResponse
            {
                MaCuaHang = ch.MaCuaHang,
                TenCuaHang = ch.TenCuaHang,
                DiaChi = ch.DiaChi,
                SoDienThoai = ch.SoDienThoai,
                Email = ch.Email,
                MoTa = ch.MoTa,
            };
        }
    }
}
