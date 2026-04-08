using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Mapper
{
    public class DanhMucBuilder
    {
        public static DanhMucResponse ToResponse(DanhMuc dm)
        {
            return new DanhMucResponse
            {
                MaDanhMuc = dm.MaDanhMuc,
                TenDanhMuc = dm.TenDanhMuc,
                MoTa = dm.MoTa,
                MaDanhMucCha = dm.MaDanhMucCha
            };
        }
    }
}
