﻿namespace DATN_API.Models
{
    public class Modeltong
    {
        public string? Email { get; set; }
        public string? Saft { get; set; }
        public string MatKhau { get; set; }
        public NguoiDung NguoiDung { get; set; }
        public Sach Sachs { get; set; }     
        public List<TheLoai> TheLoais { get; set; } = new List<TheLoai>();	
        public List<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
        public List<Sach> Saches { get; set; } = new List<Sach>();    
        public List<NhaCungCap> nhaCungCaps { get; set; } = new List<NhaCungCap>();


    }
}
