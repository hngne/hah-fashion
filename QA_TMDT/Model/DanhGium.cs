using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class DanhGium
{
    public string MaDanhGia { get; set; } = null!;

    public string? MaSp { get; set; }

    public string? MaTaiKhoan { get; set; }

    public int? SoSao { get; set; }

    public string? BinhLuan { get; set; }

    public DateTime? NgayDanhGia { get; set; }

    public string? TrangThaiDg { get; set; }

    public virtual SanPham? MaSpNavigation { get; set; }

    public virtual TaiKhoan? MaTaiKhoanNavigation { get; set; }
}
