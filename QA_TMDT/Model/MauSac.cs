using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class MauSac
{
    public int MaMau { get; set; }

    public string TenMau { get; set; } = null!;

    public string? MaHex { get; set; }

    public virtual ICollection<ChiTietSp> ChiTietSps { get; set; } = new List<ChiTietSp>();
}
