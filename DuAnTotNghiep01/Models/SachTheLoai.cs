﻿namespace DATN_API.Models
{
    public class SachTheLoai
    {
        public int MaSach { get; set; }
        public int MaTheLoai { get; set; }

        // Các Navigation properties
        public virtual Sach Sach { get; set; }
        public virtual TheLoai TheLoai { get; set; }
    }
}
