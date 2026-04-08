using QA_TMDT.Model;

namespace QA_TMDT.Repository
{
    public interface IGioHangRepository
    {
        Task<GioHang?> GetGioHangByMaTaiKhoan(string matk);
        Task<bool> AddOrUpdateGioHang(string matk, string mactsp, int soluong);
        Task<bool> DecreaseItemGioHang(string matk, string mactsp, int soluong);
        Task<bool> UpdateItemGioHang(string matk, string mactsp, int soluong);
        Task<bool> RemoveItemAsync(string matk, string mactsp);
        Task<bool> RemoveAllGioHang(string matk);
        Task CreateGioHang(string matk);
        Task<bool> CheckExistGHByMaTK(string matk);
        Task<int> CountTotalItems(string matk);
        Task<bool> CheckTkExist(string matk);
        Task<int> GetSoLuongTon(string matk);
        Task<bool> CheckCTSPExist(string mactsp);
        Task<GioHang?> GetGHBasic(string matk);
        Task SavechangeAsync();
    }
}
