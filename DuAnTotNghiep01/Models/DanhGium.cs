using System;
using System.Collections.Generic;

namespace DATN_API.Models;

public partial class DanhGium
{
    public int MaDanhGia { get; set; }

    public int? MaNguoiDung { get; set; }

    public int? MaSach { get; set; }

    public int? SoSao { get; set; }

    public string? NoiDung { get; set; }

    public DateOnly? NgayDanhGia { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual Sach? MaSachNavigation { get; set; }
}
