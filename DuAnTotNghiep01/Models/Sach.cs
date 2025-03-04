using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models;

public partial class Sach
{
    [Key]
    public int MaSach { get; set; }

    public string? TenSach { get; set; }

    public string? TacGia { get; set; }

    public int? GiaTien { get; set; }

    public int? NamXuatBan { get; set; }

    public int? SoLuongTon { get; set; }

    public int? MaNhaCungCap { get; set; }

    public string TrangThai { get; set; } = null!;

    public int? SoLuongNhap { get; set; }

    public virtual ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }

    public virtual ICollection<TheLoai>? MaTheLoais { get; set; } = new List<TheLoai>();
}
