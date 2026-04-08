using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class GioHangBuilder
    {
        public static GioHangResponse ToResponse(GioHang gh)
        {
            return new GioHangResponse
            {
                MaGioHang = gh.MaGioHang,
                MaTaiKhoan = gh.MaTaiKhoan,
                sanPhams = gh.ChiTietGhs.Select(ct => ChiTietGioHangBuilder.ToResponse(ct)).ToList(),
            };
        }
    }
}
