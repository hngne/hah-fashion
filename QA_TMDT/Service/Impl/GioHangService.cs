using QA_TMDT.Dtos.Response;
using QA_TMDT.Repository;
using QA_TMDT.Mapper;

namespace QA_TMDT.Service.Impl
{
    public class GioHangService : IGioHangService
    {
        private readonly IGioHangRepository _repo;
        public GioHangService(IGioHangRepository repo)
        {
            _repo = repo;
        }


        // Lấy giỏ hàng hiển thị
        public async Task<GioHangResponse?> GetGioHangAsync(string matk)
        {
            var checkTK = await _repo.CheckTkExist(matk);
            if (!checkTK)
            {
                return null;
            }
            var checkGH = await _repo.CheckExistGHByMaTK(matk);
            if (!checkGH)
            {
                return null;
            }
            var result = await _repo.GetGioHangByMaTaiKhoan(matk);
            return GioHangBuilder.ToResponse(result!);
        }

        // Thêm sản phẩm (Có check tồn kho)
        public async Task<(bool, string, GioHangResponse?)> AddToCartAsync(string matk, string mactsp, int soLuongMua)
        {
            var checkTk = await _repo.CheckTkExist(matk);
            if (!checkTk)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var checkGH = await _repo.CheckExistGHByMaTK(matk);
            if (!checkGH)
            {
                await _repo.CreateGioHang(matk);
            }
            var checkCTSP = await _repo.CheckCTSPExist(mactsp);
            if (!checkCTSP)
            {
                return (false, "Không có biến thể này", null);
            }
            int soluongTon = await _repo.GetSoLuongTon(mactsp);
            if(soluongTon <= 0)
            {
                return (false, "Lỗi không lấy được số lượng tồn || sản phẩm đã hết hàng", null);
            }
            int soLuongDaCo = 0;
            var giohangHienTai = await _repo.GetGHBasic(matk);
            var itemCo = giohangHienTai?.ChiTietGhs.FirstOrDefault(sp => sp.MaChiTietSp == mactsp);
            if(itemCo != null)
            {
                soLuongDaCo = itemCo.SoLuong;
            }
            if(soluongTon < soLuongDaCo + soLuongMua)
            {
                return (false, $"Kho chỉ còn {soluongTon}, bạn đã có {soLuongDaCo} trong giỏ.", null);
            }
            var result = await _repo.AddOrUpdateGioHang(matk, mactsp, soLuongMua);
            if (!result)
            {
                return (false, "Lỗi liên quan đến server", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoan(matk);
            return (true, "Thêm sản phẩm vào giỏ hàng thành công", GioHangBuilder.ToResponse(gioHang!));
        }

        // Giảm số lượng (Nút -)
        public async Task<(bool, string, GioHangResponse?)> DecreaseItemAsync(string matk, string mactsp, int soLuongGiam)
        {
            var checkTk = await _repo.CheckTkExist(matk);
            if (!checkTk)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var checkGH = await _repo.CheckExistGHByMaTK(matk);
            if (!checkGH)
            {
                return (false, "Tài khoản chưa có giỏ hàng", null);
            }
            var checkCTSP = await _repo.CheckCTSPExist(mactsp);
            if (!checkCTSP)
            {
                return (false, "Không có biến thể này", null);
            }
            var result = await _repo.DecreaseItemGioHang(matk, mactsp, soLuongGiam);
            if (!result)
            {
                return (false, "Lỗi liên quan đến server", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoan(matk);
            return (true, "Giảm sản phẩm vào giỏ hàng thành công", GioHangBuilder.ToResponse(gioHang!));
        }

        // Cập nhật số lượng (Nhập ô input, có check tồn kho)
        public async Task<(bool, string, GioHangResponse?)> UpdateItemAsync(string matk, string mactsp, int soLuongMoi)
        {
            var checkTk = await _repo.CheckTkExist(matk);
            if (!checkTk)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var checkGH = await _repo.CheckExistGHByMaTK(matk);
            if (!checkGH)
            {
                return (false, "Tài khoản chưa có giỏ hàng", null);
            }
            var checkCTSP = await _repo.CheckCTSPExist(mactsp);
            if (!checkCTSP)
            {
                return (false, "Không có biến thể này", null);
            }
            int soluongTon = await _repo.GetSoLuongTon(mactsp);
            if (soluongTon <= 0)
            {
                return (false, "Lỗi không lấy được số lượng tồn || sản phẩm hết hàng", null);
            }
            else if (soluongTon < soLuongMoi)
            {
                return (false, $"Kho chỉ còn {soluongTon} sản phẩm", null);
            }
            var result = await _repo.UpdateItemGioHang(matk, mactsp, soLuongMoi);
            if (!result)
            {
                return (false, "Lỗi liên quan đến server", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoan(matk);
            return (true, "Cập nhật số lượng sản phẩm vào giỏ hàng thành công", GioHangBuilder.ToResponse(gioHang!));
        }

        // Xóa 1 món
        public async Task<(bool, string, GioHangResponse?)> RemoveItemAsync(string matk, string mactsp)
        {
            var checkTk = await _repo.CheckTkExist(matk);
            if (!checkTk)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var checkGH = await _repo.CheckExistGHByMaTK(matk);
            if (!checkGH)
            {
                return (false, "Tài khoản chưa có giỏ hàng", null);
            }
            var checkCTSP = await _repo.CheckCTSPExist(mactsp);
            if (!checkCTSP)
            {
                return (false, "Không có biến thể này", null);
            }
            var result = await _repo.RemoveItemAsync(matk, mactsp);
            if (!result)
            {
                return (false, "Có lỗi liên quan server", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoan(matk);
            return (true, "Xóa sản phẩm khỏi giỏ hàng thành công", GioHangBuilder.ToResponse(gioHang!));
        }

        // Xóa sạch giỏ
        public async Task<(bool, string, GioHangResponse?)> ClearCartAsync(string matk)
        {
            var checkTk = await _repo.CheckTkExist(matk);
            if (!checkTk)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var checkGH = await _repo.CheckExistGHByMaTK(matk);
            if (!checkGH)
            {
                return (false, "Tài khoản chưa có giỏ hàng", null);
            }
            var result = await _repo.RemoveAllGioHang(matk);
            if (!result)
            {
                return (false, "Có lỗi liên quan server", null);
            }
            var gioHang = await _repo.GetGioHangByMaTaiKhoan(matk);
            return (true, "Xóa tất cả sản phẩm khỏi giỏ hàng thành công", GioHangBuilder.ToResponse(gioHang!));
        }
    }
}
