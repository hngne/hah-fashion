using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class AnhSp
{
    public int MaAnh { get; set; }

    public string MaSp { get; set; } = null!;

    public string DuongDan { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
