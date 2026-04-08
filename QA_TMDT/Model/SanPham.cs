using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class SanPham
{
    public string MaSp { get; set; } = null!;

    public string TenSp { get; set; } = null!;

    public string? TenTimKiem { get; set; }

    public int? MaDanhMuc { get; set; }

    public string? MoTa { get; set; }

    public decimal GiaGoc { get; set; }

    public string? ChatLieu { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ICollection<AnhSp> AnhSps { get; set; } = new List<AnhSp>();

    public virtual ICollection<ChiTietKm> ChiTietKms { get; set; } = new List<ChiTietKm>();

    public virtual ICollection<ChiTietSp> ChiTietSps { get; set; } = new List<ChiTietSp>();

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual DanhMuc? MaDanhMucNavigation { get; set; }
}
