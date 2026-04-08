using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class PhuongThucTt
{
    public int MaPttt { get; set; }

    public string TenPttt { get; set; } = null!;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
