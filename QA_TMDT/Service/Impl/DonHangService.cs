using QA_TMDT.Dtos.Request;
using QA_TMDT.Dtos.Response;
using QA_TMDT.Helper;
using QA_TMDT.Mapper;
using QA_TMDT.Model;
using QA_TMDT.Repository;
using QA_TMDT.Utils;

namespace QA_TMDT.Service.Impl
{
    public class DonHangService : IDonHangService
    {
        private readonly IDonHangRepository _repo;
        private readonly IGioHangRepository _gioRepo;
        private readonly IVoucherRepository _voucherRepo;
        private readonly QaTmdtContext _context;
        private readonly IChiTietSPRepository _chiTietSPRepo;

        public DonHangService(IDonHangRepository repo, IGioHangRepository gioRepo, IVoucherRepository voucherRepo, QaTmdtContext context,IChiTietSPRepository chiTietSPRepo)
        {
            _repo = repo;
            _gioRepo = gioRepo;
            _voucherRepo = voucherRepo;
            _context = context;
            _chiTietSPRepo = chiTietSPRepo;
        }

        public async Task<IEnumerable<DonHangResponse>> GetAll(string? maDonHang = null, string? trangThai = null)
        {
            var result = await _repo.GetAll(maDonHang, trangThai);
            return result.Select(DonHangBuilder.ToResponse);
        }
        public async Task<(bool, string, DonHangResponse?)> CreateDonHang(DatHangRequest request)
        {
            if (!await _repo.CheckPTTT(request.MaPTTT))
            {
                return (false, "Phương thức thanh toán không hợp lệ", null);
            }
            if (!await _repo.CheckPTVC(request.MaPTVC))
            {
                return (false, "Phương thức vận chuyển không hợp lệ", null);
            }
            if (!await _repo.CheckAcc(request.MaTaiKhoan))
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var gioHang = await _gioRepo.GetGioHangByMaTaiKhoan(request.MaTaiKhoan);
            if (gioHang == null || !gioHang.ChiTietGhs.Any())
            {
                return (false, "Giỏ hàng trống", null);
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var donHang = new DonHang
                {
                    MaDonHang = Guid.NewGuid().ToString(),
                    MaTaiKhoan = request.MaTaiKhoan,
                    MaPttt = request.MaPTTT,
                    MaPtvc = request.MaPTVC,
                    TrangThaiDonHang = TrangThaiDonHang.ChoXacNhan,
                    NgayDatHang = DateTime.Now,
                    TenNguoiNhan = request.TenNguoiNhan,
                    SoDienThoaiNhan = request.soDienThoai,
                    DiaChiGiaoHang = request.DiaChiGiaoHang,
                    ChiTietDhs = new List<ChiTietDh>()
                };
                decimal tongTienHang = 0;
                foreach(var item in gioHang.ChiTietGhs)
                {
                    var bienthe = await _chiTietSPRepo.GetChiTietSP(item.MaChiTietSp);
                    if(bienthe == null)
                    {
                        throw new Exception("Sản phẩm không tồn tại");
                    }
                    var truKhoSuccess = await _chiTietSPRepo.TruTonKho(item.MaChiTietSp,item.SoLuong);
                    if (!truKhoSuccess)
                    {
                        throw new Exception($"Sản phẩm {bienthe.MaSpNavigation?.TenSp} hết hàng");
                    }

                    decimal donGia = bienthe.GiaBan ?? bienthe.MaSpNavigation?.GiaGoc ?? 0;
                    decimal donGiaDat = KhuyenMaiHelper.TinhGiaSauKhuyenMai(donGia, bienthe.MaSpNavigation?.ChiTietKms);
                    decimal thanhTienItem = donGiaDat * item.SoLuong;
                    tongTienHang += thanhTienItem;

                    donHang.ChiTietDhs.Add(new ChiTietDh
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaChiTietSp = item.MaChiTietSp,
                        SoLuong = item.SoLuong,
                        DonGiaLucDat = donGiaDat,
                    });
                }
                donHang.TongTienHang = tongTienHang;
                var ptvcInfo = await _context.PhuongThucVcs.FindAsync(request.MaPTVC);

                decimal phiShipGoc = ptvcInfo?.PhiVanChuyen ?? 30000;
                decimal phiShip = phiShipGoc;
                if (tongTienHang > 500000)
                {
                    phiShip = 0;
                }

                decimal giamGia = 0;
                if (!String.IsNullOrEmpty(request.MaVoucher))
                {
                    var voucher = await _voucherRepo.GetVoucherById(request.MaVoucher);
                    var now = DateTime.Now;

                    if (voucher == null || voucher.NgayBatDau > now || voucher.NgayKetThuc < now)
                    {
                        return (false, "Voucher không tồn tại hoặc đã hết hạn", null);
                    }

                    decimal dieuKienMin = voucher.DieuKienApDung ?? 0;
                    if (tongTienHang < dieuKienMin)
                    {
                        return (false, $"Đơn hàng chưa đủ điều kiện. Cần tối thiểu {dieuKienMin:N0}đ", null);
                    }

                    var usedCodes = await _voucherRepo.GetUsedVoucherCodes(request.MaTaiKhoan);
                    if (usedCodes.Contains(request.MaVoucher))
                    {
                        return (false, "Bạn đã sử dụng mã giảm giá này rồi", null);
                    }
                    donHang.MaVoucher = request.MaVoucher;
                    giamGia = voucher.GiamGia;
                }

                decimal tongThanhToan = tongTienHang + phiShip - giamGia;
                donHang.PhiVanChuyen = phiShip;
                donHang.GiamGia = giamGia;
                donHang.TongThanhToan = tongThanhToan;
                await _repo.CreateDonHang(donHang);
                await _gioRepo.RemoveAllGioHang(request.MaTaiKhoan);
                await transaction.CommitAsync();
                var resultDH = await _repo.GetDonHangByMaDH(donHang.MaDonHang);
                return (true, "Đặt hàng thành công", DonHangBuilder.ToResponse(resultDH!));
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, "Lỗi: " + ex.Message, null);
            }
        }
        public async Task<DonHangResponse?> GetDonHangByMaDH(string maDH)
        {
            var result = await _repo.GetDonHangByMaDH(maDH);
            return result == null ? null : DonHangBuilder.ToResponse(result);
        }
        public async Task<IEnumerable<DonHangResponse>?> GetDonHangByMaTK(string maTK)
        {
            var checkAcc = await _repo.CheckAcc(maTK);
            if (!checkAcc)
            {
                return null;
            }
            var result = await _repo.GetDonHangByMaTK(maTK);
            return result.Select(DonHangBuilder.ToResponse).ToList();
        }
        public async Task<(bool, string, DonHangResponse?)> UpdateTrangThaiDonHang(string maDonHang, UpdateTrangThaiDonRequest request)
        {
            var donHang = await _repo.GetDonHangByMaDH(maDonHang);
            if (donHang == null) return (false, "Đơn hàng không tồn tại", null);

            string trangThaiCu = donHang.TrangThaiDonHang!;
            if (trangThaiCu == request.TrangThaiMoi)
            {
                return (true, "Trạng thái đã được cập nhật trước đó rồi", DonHangBuilder.ToResponse(donHang));
            }

            bool luongTrangThai = false;
            switch (trangThaiCu)
            {
                case TrangThaiDonHang.ChoXacNhan:
                    if (request.TrangThaiMoi == TrangThaiDonHang.DangVanChuyen || request.TrangThaiMoi == TrangThaiDonHang.DaHuy)
                    {
                        luongTrangThai = true;
                    }
                    break;

                case TrangThaiDonHang.DangVanChuyen:
                    if (request.TrangThaiMoi == TrangThaiDonHang.GiaoHangThanhCong || request.TrangThaiMoi == TrangThaiDonHang.DaHuy)
                    {
                        luongTrangThai = true;
                    }
                    break;
                case TrangThaiDonHang.GiaoHangThanhCong:
                case TrangThaiDonHang.DaHuy:
                    return (false, "Đơn hàng đã hoàn tất hoặc đã hủy, không thể thay đổi trạng thái nữa", DonHangBuilder.ToResponse(donHang));

                default:
                    return (false, "Trạng thái đơn hàng hiện tại không hợp lệ(data bug)", null);

            }

            if (!luongTrangThai)
            {
                return (false, $"Không thể chuyển trạng thái từ '{trangThaiCu}' sang '{request.TrangThaiMoi}'. Quy trình không hợp lệ!", DonHangBuilder.ToResponse(donHang));
            }

            donHang.TrangThaiDonHang = request.TrangThaiMoi;

            await _repo.UpdateDonHang(donHang);

            return (true, $"Cập nhật trạng thái thành {request.TrangThaiMoi}", DonHangBuilder.ToResponse(donHang));
        }
        public async Task<(bool, string, DonHangResponse?)> HuyDonHang(string maDonHang, HuyDonRequest request)
        {
            var donHang = await _repo.GetDonHangByMaDH(maDonHang);
            if (donHang == null) return (false, "Đơn hàng không tồn tại", null);

            if (donHang.MaTaiKhoan != request.MaTaiKhoan) return (false, "Bạn không có quyền hủy đơn này", null);

            if (donHang.TrangThaiDonHang != TrangThaiDonHang.ChoXacNhan)
            {
                return (false, "Đơn hàng đang vận chuyển hoặc đã xong, không thể hủy", DonHangBuilder.ToResponse(donHang));
            }
            if(donHang.ChiTietDhs != null)
            {
                foreach(var item in donHang.ChiTietDhs)
                {
                    await _chiTietSPRepo.CongTonKho(item.MaChiTietSp, item.SoLuong);
                }
            }
            donHang.MaVoucher = null;
            donHang.TrangThaiDonHang = TrangThaiDonHang.DaHuy;

            await _repo.UpdateDonHang(donHang);
            return (true, "Đã hủy đơn hàng thành công và hoàn lại hàng trở về kho", DonHangBuilder.ToResponse(donHang));
        }
        public async Task<(bool, string, DonHangResponse?)> UpdateDiaChiGiaoHang(string maDonHang, UpdateDiaChiRequest request)
        {
            var donHang = await _repo.GetDonHangByMaDH(maDonHang);
            if (donHang == null) return (false, "Đơn hàng không tồn tại", null);

            if (donHang.MaTaiKhoan != request.MaTaiKhoan) return (false, "Không đúng tài khoản", null);

            if (donHang.TrangThaiDonHang != TrangThaiDonHang.ChoXacNhan)
            {
                return (false, "Đơn hàng đã đi giao, không thể thay đổi địa chỉ", DonHangBuilder.ToResponse(donHang));
            }

            donHang.DiaChiGiaoHang = request.DiaChiMoi;
            await _repo.UpdateDonHang(donHang);

            return (true, "Cập nhật địa chỉ thành công", DonHangBuilder.ToResponse(donHang));
        }
        public async Task<(bool, string)> DeleteDonHang(string maDH)
        {
            var dh = await _repo.GetDonHangByMaDH(maDH);
            if (dh == null)
            {
                return (false, "Không có đơn hàng này");
            }
            if (dh.TrangThaiDonHang == TrangThaiDonHang.GiaoHangThanhCong || dh.TrangThaiDonHang == TrangThaiDonHang.ChoXacNhan || dh.TrangThaiDonHang == TrangThaiDonHang.DangVanChuyen)
            {
                return (false, "Không được xóa đơn hàng này do nghiệp vụ không cho phép");
            }
            var result = await _repo.DeleteDonHang(maDH);
            if (!result)
            {
                return (false, "Có lỗi khi xóa");
            }
            return (true, "Xóa thành công đơn hàng");
        }
    }
}
