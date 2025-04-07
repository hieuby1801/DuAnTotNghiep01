using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DATN_API.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public DbSet<DanhGia> DanhGia { get; set; }

    public DbSet<DonHang> DonHangs { get; set; }



    public DbSet<NhaCungCap> NhaCungCap { get; set; }

    public DbSet<PhuongThucThanhToan> PhuongThucThanhToan { get; set; }
    public DbSet<VanChuyen> VanChuyen { get; set; }


    public DbSet<ThanhToan> ThanhToan { get; set; }
    public DbSet<NguoiDung> NguoiDung { get; set; }
    public DbSet<TheLoai> TheLoai { get; set; }
    public DbSet<Sach> Sach { get; set; }
    
    public DbSet <SachTheLoai> SachTheLoai { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Thiết lập khóa chính cho bảng SachTheLoai
        modelBuilder.Entity<SachTheLoai>()
            .HasKey(stl => new { stl.MaSach, stl.MaTheLoai });

        // Quan hệ giữa Sach và SachTheLoai
        modelBuilder.Entity<SachTheLoai>()
            .HasOne(stl => stl.Sach)
            .WithMany(s => s.SachTheLoais)  // Một sách có thể có nhiều thể loại
            .HasForeignKey(stl => stl.MaSach);

        // Quan hệ giữa TheLoai và SachTheLoai
        modelBuilder.Entity<SachTheLoai>()
            .HasOne(stl => stl.TheLoai)
            .WithMany(t => t.SachTheLoais)  // Một thể loại có thể có nhiều sách
            .HasForeignKey(stl => stl.MaTheLoai);
    }
}

/*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
      => optionsBuilder.UseSqlServer("Server=WINDOWS-10;Database=WebBanSach;Trusted_Connection=True;TrustServerCertificate=True;");

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
      modelBuilder.Entity<ChiTietDonHang>(entity =>
      {
          entity.HasKey(e => new { e.MaDonHang, e.MaSach }).HasName("PK__ChiTietD__D9B6D3EF4AC869A9");

          entity.ToTable("ChiTietDonHang");

          entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHangs)
              .HasForeignKey(d => d.MaDonHang)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK__ChiTietDo__MaDon__5CD6CB2B");

          entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.ChiTietDonHangs)
              .HasForeignKey(d => d.MaSach)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK__ChiTietDo__MaSac__5DCAEF64");
      });

      modelBuilder.Entity<DanhGium>(entity =>
      {
          entity.HasKey(e => e.MaDanhGia).HasName("PK__DanhGia__AA9515BF191375E9");

          entity.Property(e => e.MaDanhGia).ValueGeneratedNever();
          entity.Property(e => e.NoiDung).HasMaxLength(500);

          entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGia)
              .HasForeignKey(d => d.MaNguoiDung)
              .HasConstraintName("FK__DanhGia__MaNguoi__6A30C649");

          entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.DanhGia)
              .HasForeignKey(d => d.MaSach)
              .HasConstraintName("FK__DanhGia__MaSach__6B24EA82");
      });

      modelBuilder.Entity<DonHang>(entity =>
      {
          entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__129584AD999EA3F6");

          entity.ToTable("DonHang");

          entity.Property(e => e.MaDonHang).ValueGeneratedNever();
          entity.Property(e => e.TrangThai).HasMaxLength(50);

          entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
              .HasForeignKey(d => d.MaNguoiDung)
              .HasConstraintName("FK__DonHang__MaNguoi__59FA5E80");
      });

      modelBuilder.Entity<NguoiDung>(entity =>
      {
          entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D76220CEED45");

          entity.ToTable("NguoiDung");

          entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D10534265CC169").IsUnique();

          entity.Property(e => e.MaNguoiDung).ValueGeneratedNever();
          entity.Property(e => e.DiaChi).HasMaxLength(255);
          entity.Property(e => e.Email).HasMaxLength(255);
          entity.Property(e => e.MatKhau).HasMaxLength(255);
          entity.Property(e => e.SoDienThoai).HasMaxLength(20);
          entity.Property(e => e.TenNguoiDung).HasMaxLength(255);
          entity.Property(e => e.TrangThai)
              .HasMaxLength(3)
              .HasDefaultValue("on");
          entity.Property(e => e.VaiTro).HasMaxLength(50);
      });

      modelBuilder.Entity<NhaCungCap>(entity =>
      {
          entity.HasKey(e => e.MaNhaCungCap).HasName("PK__NhaCungC__53DA9205C83FD7FE");

          entity.ToTable("NhaCungCap");

          entity.Property(e => e.MaNhaCungCap).ValueGeneratedNever();
          entity.Property(e => e.DiaChi).HasMaxLength(255);
          entity.Property(e => e.Email).HasMaxLength(255);
          entity.Property(e => e.SoDienThoai).HasMaxLength(20);
          entity.Property(e => e.TenNhaCungCap).HasMaxLength(255);
          entity.Property(e => e.TrangThai)
              .HasMaxLength(3)
              .HasDefaultValue("on");
      });

      modelBuilder.Entity<PhuongThucThanhToan>(entity =>
      {
          entity.HasKey(e => e.MaPhuongThuc).HasName("PK__PhuongTh__35F7404EC9552233");

          entity.ToTable("PhuongThucThanhToan");

          entity.Property(e => e.MaPhuongThuc).ValueGeneratedNever();
          entity.Property(e => e.TenPhuongThuc).HasMaxLength(255);
      });

      modelBuilder.Entity<Sach>(entity =>
      {
          entity.HasKey(e => e.MaSach).HasName("PK__Sach__B235742DCED81B36");

          entity.ToTable("Sach");

          entity.Property(e => e.MaSach).ValueGeneratedNever();
          entity.Property(e => e.TacGia).HasMaxLength(255);
          entity.Property(e => e.TenSach).HasMaxLength(255);
          entity.Property(e => e.TrangThai)
              .HasMaxLength(3)
              .HasDefaultValue("on");

          entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.Saches)
              .HasForeignKey(d => d.MaNhaCungCap)
              .HasConstraintName("FK__Sach__MaNhaCungC__5070F446");

          entity.HasMany(d => d.MaTheLoais).WithMany(p => p.MaSaches)
              .UsingEntity<Dictionary<string, object>>(
                  "SachTheLoai",
                  r => r.HasOne<TheLoai>().WithMany()
                      .HasForeignKey("MaTheLoai")
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__SachTheLo__MaThe__571DF1D5"),
                  l => l.HasOne<Sach>().WithMany()
                      .HasForeignKey("MaSach")
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__SachTheLo__MaSac__5629CD9C"),
                  j =>
                  {
                      j.HasKey("MaSach", "MaTheLoai").HasName("PK__SachTheL__0F468B19B60B778E");
                      j.ToTable("SachTheLoai");
                  });
      });

      modelBuilder.Entity<ThanhToan>(entity =>
      {
          entity.HasKey(e => e.MaThanhToan).HasName("PK__ThanhToa__D4B2584467F5C0C4");

          entity.ToTable("ThanhToan");

          entity.Property(e => e.MaThanhToan).ValueGeneratedNever();
          entity.Property(e => e.TrangThai).HasMaxLength(50);

          entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ThanhToans)
              .HasForeignKey(d => d.MaDonHang)
              .HasConstraintName("FK__ThanhToan__MaDon__628FA481");

          entity.HasOne(d => d.MaPhuongThucNavigation).WithMany(p => p.ThanhToans)
              .HasForeignKey(d => d.MaPhuongThuc)
              .HasConstraintName("FK__ThanhToan__MaPhu__6383C8BA");
      });

      modelBuilder.Entity<TheLoai>(entity =>
      {
          entity.HasKey(e => e.MaTheLoai).HasName("PK__TheLoai__D73FF34A4ABD45FE");

          entity.ToTable("TheLoai");

          entity.Property(e => e.MaTheLoai).ValueGeneratedNever();
          entity.Property(e => e.TenTheLoai).HasMaxLength(255);
      });

      modelBuilder.Entity<VanChuyen>(entity =>
      {
          entity.HasKey(e => e.MaVanChuyen).HasName("PK__VanChuye__4B22972D2593AF8E");

          entity.ToTable("VanChuyen");

          entity.Property(e => e.MaVanChuyen).ValueGeneratedNever();
          entity.Property(e => e.DiaChiGiao).HasMaxLength(255);
          entity.Property(e => e.TrangThai).HasMaxLength(50);

          entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.VanChuyens)
              .HasForeignKey(d => d.MaDonHang)
              .HasConstraintName("FK__VanChuyen__MaDon__66603565");
      });

      OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
*/