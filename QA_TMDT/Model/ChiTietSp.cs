using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class ChiTietSp
{
    public string MaChiTietSp { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public int MaKichThuoc { get; set; }

    public int MaMauSac { get; set; }

    public int? SoLuongTon { get; set; }

    public decimal? GiaBan { get; set; }

    public string? AnhMinhHoa { get; set; }

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual KichThuoc MaKichThuocNavigation { get; set; } = null!;

    public virtual MauSac MaMauSacNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
