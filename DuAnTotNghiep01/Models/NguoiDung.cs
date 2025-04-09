using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models;

public partial class NguoiDung
{
    [Key]
    public int MaNguoiDung { get; set; }

    public string? TenNguoiDung { get; set; }

    public string? Email { get; set; }

    public string? MatKhau { get; set; }
    public string? Saft { get; set; }
    public DateTime? NgaySinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? VaiTro { get; set; }

    public string? TrangThai { get; set; } = null!;

}
