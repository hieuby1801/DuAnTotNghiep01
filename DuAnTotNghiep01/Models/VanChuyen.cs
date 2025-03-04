using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models;

public partial class VanChuyen
{
    [Key]
    public int MaVanChuyen { get; set; }

    public int? MaDonHang { get; set; }

    public DateOnly? NgayGiaoHang { get; set; }

    public DateOnly? NgayNhanHang { get; set; }

    public string? TrangThai { get; set; }

    public string? DiaChiGiao { get; set; }

    public virtual DonHang? MaDonHangNavigation { get; set; }
}
