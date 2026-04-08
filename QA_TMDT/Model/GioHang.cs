using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class GioHang
{
    public string MaGioHang { get; set; } = null!;

    public string? MaTaiKhoan { get; set; }

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual TaiKhoan? MaTaiKhoanNavigation { get; set; }
}
