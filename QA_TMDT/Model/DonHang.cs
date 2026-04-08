using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class DonHang
{
    public string MaDonHang { get; set; } = null!;

    public string MaTaiKhoan { get; set; } = null!;

    public int? MaPttt { get; set; }

    public int? MaPtvc { get; set; }

    public string? MaVoucher { get; set; }

    public string? TenNguoiNhan { get; set; }

    public string? SoDienThoaiNhan { get; set; }

    public string DiaChiGiaoHang { get; set; } = null!;

    public decimal TongTienHang { get; set; }

    public decimal? PhiVanChuyen { get; set; }

    public decimal? GiamGia { get; set; }

    public decimal TongThanhToan { get; set; }

    public string? TrangThaiDonHang { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public bool? ThanhToanVnpay { get; set; }

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual PhuongThucTt? MaPtttNavigation { get; set; }

    public virtual PhuongThucVc? MaPtvcNavigation { get; set; }

    public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;

    public virtual Voucher? MaVoucherNavigation { get; set; }
}
