using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Model;

namespace QA_TMDT.Service
{
    public interface IDanhMucService
    {
        Task<IEnumerable<DanhMucResponse>> GetAllDM();
        Task<DanhMucResponse?> GetDMByMaDM(int madm);
        Task<IEnumerable<DanhMucResponse>> GetDMByTenDM(string tendm);
        Task<(bool success,string message, DanhMucResponse? response)> PostDM(DanhMucRequest request);
        Task<(bool success, string message, DanhMucResponse? response)> UpdateDM(int madm,DanhMucRequest request);
        Task<(bool success, string message)> DeleteDM(int madm);
    }
}
