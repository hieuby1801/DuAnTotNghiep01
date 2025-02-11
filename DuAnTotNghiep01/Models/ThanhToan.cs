using System;
using System.Collections.Generic;

namespace DATN_API.Models;

public partial class ThanhToan
{
    public int MaThanhToan { get; set; }

    public int? MaDonHang { get; set; }

    public int? MaPhuongThuc { get; set; }

    public DateOnly? NgayThanhToan { get; set; }

    public int? SoTien { get; set; }

    public string? TrangThai { get; set; }

    public virtual DonHang? MaDonHangNavigation { get; set; }

    public virtual PhuongThucThanhToan? MaPhuongThucNavigation { get; set; }
}
