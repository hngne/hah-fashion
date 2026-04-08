using QA_TMDT.Model;

namespace QA_TMDT.Helper
{
    public class KhuyenMaiHelper
    {
        public static decimal TinhGiaSauKhuyenMai(decimal giaBanDau, ICollection<ChiTietKm>? listKhuyenMai)
        {
            // 1. Nếu không có danh sách KM hoặc danh sách rỗng -> Trả về nguyên giá
            if (listKhuyenMai == null || !listKhuyenMai.Any())
            {
                return giaBanDau;
            }

            var now = DateTime.Now;

            // 2. Tìm cái khuyến mãi ngon nhất đang chạy
            var kmTotNhat = listKhuyenMai
                .Where(ctkm => ctkm.MaKhuyenMaiNavigation != null
                            && ctkm.MaKhuyenMaiNavigation.NgayBatDau <= now
                            && ctkm.MaKhuyenMaiNavigation.NgayKetThuc >= now)
                .OrderByDescending(ct => ct.PhanTramGiam)
                .FirstOrDefault();

            // 3. Tính toán
            if (kmTotNhat == null)
            {
                return giaBanDau;
            }

            // Công thức: Giá gốc - (Giá gốc * % / 100)
            return giaBanDau - (giaBanDau * kmTotNhat.PhanTramGiam / 100);
        }
    }
}
