﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models;

public partial class NhaCungCap
{
    [Key]
    public int MaNhaCungCap { get; set; }

    public string? TenNhaCungCap { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<Sach>? Sach { get; set; } = new List<Sach>();
}
