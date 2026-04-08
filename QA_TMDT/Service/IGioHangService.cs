using QA_TMDT.Dtos.Response;

namespace QA_TMDT.Service
{
    public interface IGioHangService
    {
        // Lấy giỏ hàng hiển thị
        Task<GioHangResponse?> GetGioHangAsync(string matk);

        // Thêm sản phẩm (Có check tồn kho)
        Task<(bool Success, string Message, GioHangResponse? response)> AddToCartAsync(string matk, string mactsp, int soLuongMua);

        // Giảm số lượng (Nút -)
        Task<(bool Success, string Message, GioHangResponse? response)> DecreaseItemAsync(string matk, string mactsp, int soLuongGiam);

        // Cập nhật số lượng (Nhập ô input, có check tồn kho)
        Task<(bool Success, string Message, GioHangResponse? response)> UpdateItemAsync(string matk, string mactsp, int soLuongMoi);

        // Xóa 1 món
        Task<(bool Success, string Message, GioHangResponse? response)> RemoveItemAsync(string matk, string mactsp);

        // Xóa sạch giỏ
        Task<(bool Success, string Message, GioHangResponse? response)> ClearCartAsync(string matk);
    }
}
