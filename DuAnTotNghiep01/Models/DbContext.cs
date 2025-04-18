﻿using Microsoft.EntityFrameworkCore;

namespace DATN_API.Models
{
	public class MyDbContext : DbContext
	{
		public MyDbContext (DbContextOptions<MyDbContext> options) : base(options)
		{

		}
		public DbSet<Sach> Sach { get; set; }
		public DbSet <ChiTietDonHang> chiTietDonHangs { get; set; }
		public DbSet<ChiTietLoHang> chiTietLoHangs { get; set; }
		public DbSet <DanhGia> danhGias { get; set; }
		public DbSet<LichSuGia> lichSuGias { get;set; }
		public DbSet<LoHang> loHang { get; set; }
		public DbSet<NguoiDung> NguoiDung { get; set; }
		public DbSet<NhaCungCap> nhaCungCaps { get; set; }
		public DbSet<PhieuTraHang> phieuTraHangs { get; set; }
		public DbSet<PhuongThucThanhToan> phuongThucThanhToans { get; set; }
		public DbSet<SachChiTiet> sachChiTiets { get; set; }
		public DbSet<SachTheLoai> sachTheLoais { get; set; }
		public DbSet<ThanhToan> thanhToans { get; set; }
		public  DbSet<TheLoai> TheLoai { get; set;}
		public DbSet<TonKho> tonKhos { get; set; }
		public DbSet<VanChuyen>	vanChuyens { get; set; }
		public DbSet<GioHang> Giohang { get; set; }
	}
}
