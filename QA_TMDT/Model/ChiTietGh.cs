using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class ChiTietGh
{
    public string MaGioHang { get; set; } = null!;

    public string MaChiTietSp { get; set; } = null!;

    public int SoLuong { get; set; }

    public virtual ChiTietSp MaChiTietSpNavigation { get; set; } = null!;

    public virtual GioHang MaGioHangNavigation { get; set; } = null!;
}
