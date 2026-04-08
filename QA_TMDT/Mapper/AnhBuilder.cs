using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class AnhBuilder
    {
        public static AnhResponse ToResponse(AnhSp a)
        {
            return new AnhResponse
            {
                maAnh = a.MaAnh,
                maSP = a.MaSp,
                duongDanAnh = a.DuongDan
            };
        }
    }
}
