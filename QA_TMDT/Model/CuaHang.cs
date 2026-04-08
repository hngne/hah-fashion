using System;
using System.Collections.Generic;

namespace QA_TMDT.Model;

public partial class CuaHang
{
    public string MaCuaHang { get; set; } = null!;

    public string TenCuaHang { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? MoTa { get; set; }
}
