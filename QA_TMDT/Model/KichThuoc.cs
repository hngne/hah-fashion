using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class KichThuoc
{
    public int MaSize { get; set; }

    public string TenSize { get; set; } = null!;

    public virtual ICollection<ChiTietSp> ChiTietSps { get; set; } = new List<ChiTietSp>();
}
