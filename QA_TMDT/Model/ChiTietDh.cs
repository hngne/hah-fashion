using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class ChiTietDh
{
    public string MaDonHang { get; set; } = null!;

    public string MaChiTietSp { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGiaLucDat { get; set; }

    public virtual ChiTietSp MaChiTietSpNavigation { get; set; } = null!;

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;
}
