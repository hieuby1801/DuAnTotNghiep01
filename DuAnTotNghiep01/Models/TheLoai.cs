using System;
using System.Collections.Generic;

namespace DATN_API.Models;

public partial class TheLoai
{
    public int MaTheLoai { get; set; }

    public string? TenTheLoai { get; set; }

    public virtual ICollection<Sach> MaSaches { get; set; } = new List<Sach>();
}
