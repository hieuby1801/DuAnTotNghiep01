﻿namespace DATN_MVC.Models
{
    public class TheLoai
    {
        public int MaTheLoai { get; set; }

        public string? TenTheLoai { get; set; }

        public ICollection<SachTheLoai> SachTheLoais { get; set; }
    }
}
