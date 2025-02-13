﻿using System;
using System.Collections.Generic;

namespace DATN_API.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string? TenNguoiDung { get; set; }

    public string? Email { get; set; }

    public string? MatKhau { get; set; }
    public string? Saft { get; set; }
    public DateTime NgaySinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? VaiTro { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<DanhGium> DanhGia { get; set; } = new List<DanhGium>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
