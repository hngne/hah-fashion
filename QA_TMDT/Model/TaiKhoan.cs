using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QA_TMDT.Model;

public partial class TaiKhoan
{
    public string MaTaiKhoan { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? Email { get; set; }

    public string? HoTen { get; set; }

    public string? SoDienThoai { get; set; }

    public string? VaiTro { get; set; }

    public string? DiaChi { get; set; }

    public DateTime? NgayTao { get; set; }

    [NotMapped]
    public bool TrangThai { get; set; } = true;

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual GioHang? GioHang { get; set; }
}
