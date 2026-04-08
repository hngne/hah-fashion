using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class DanhMuc
{
    public int MaDanhMuc { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? MaDanhMucCha { get; set; }

    public virtual ICollection<DanhMuc> InverseMaDanhMucChaNavigation { get; set; } = new List<DanhMuc>();

    public virtual DanhMuc? MaDanhMucChaNavigation { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
