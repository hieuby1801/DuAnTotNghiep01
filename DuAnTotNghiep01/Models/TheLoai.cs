using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models;

public partial class TheLoai
{
    [Key]
    public int MaTheLoai { get; set; }

    public string? TenTheLoai { get; set; }
    public ICollection<SachTheLoai> SachTheLoais { get; set; }
 
}
