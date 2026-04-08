using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class PhuongThucVc
{
    public int MaPtvc { get; set; }

    public string TenPtvc { get; set; } = null!;

    public decimal PhiVanChuyen { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
