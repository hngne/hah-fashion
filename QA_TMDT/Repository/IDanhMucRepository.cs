using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface IDanhMucRepository
    {
        Task<List<DanhMuc>> GetAllDM();
        Task<DanhMuc?> GetDMByMaDM(int madm);
        Task<List<DanhMuc>> GetDMByTenDM(string tendm);
        Task<bool> PostDM(DanhMuc dm);
        Task<bool> UpdateDM(DanhMuc dm);
        Task<bool> DeleteDM(int madm);
        Task<bool> checkExistDM(int madm);
        Task<bool> CheckHasChild(int madm);
        Task<bool> CheckHasProduct(int madm);
    }
}
