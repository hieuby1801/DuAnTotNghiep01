using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string? HinhAnh { get; set; }
    public ICollection<SachTheLoai> SachTheLoais { get; set; }

    public virtual ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<DanhGia> DanhGia { get; set; } = new List<DanhGia>();

    [ForeignKey("MaNhaCungCap")]
    public virtual NhaCungCap? NhaCungCap { get; set; }
}
