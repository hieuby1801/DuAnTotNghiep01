﻿using System.ComponentModel.DataAnnotations;

namespace DATN_API.Models
{
	public class ChiTietLoHang
	{
		[Key]
        public int MaChiTietLoHang { get; set; }
		public int? MaLo { get; set; }
		public int? MaSach { get; set; }
		public int? SoLuongNhap { get; set; }
		public int? GiaNhap { get; set; }
		public string? NhaXuatBan { get; set; }


	}
}
