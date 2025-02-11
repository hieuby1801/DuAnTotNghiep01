using System;
using System.Collections.Generic;

namespace DATN_API.Models;

public partial class ChiTietDonHang
{
    public int MaDonHang { get; set; }

    public int MaSach { get; set; }

    public int? SoLuong { get; set; }

    public int? GiaTien { get; set; }

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;

    public virtual Sach MaSachNavigation { get; set; } = null!;
}
