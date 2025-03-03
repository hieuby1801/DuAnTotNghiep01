﻿// <auto-generated />
using System;
using DATN_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DATN_API.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DATN_API.Models.ChiTietDonHang", b =>
                {
                    b.Property<int?>("GiaTien")
                        .HasColumnType("int");

                    b.Property<int>("MaDonHang")
                        .HasColumnType("int");

                    b.Property<int>("MaDonHangNavigationMaDonHang")
                        .HasColumnType("int");

                    b.Property<int>("MaSach")
                        .HasColumnType("int");

                    b.Property<int>("MaSachNavigationMaSach")
                        .HasColumnType("int");

                    b.Property<int?>("SoLuong")
                        .HasColumnType("int");

                    b.HasIndex("MaDonHangNavigationMaDonHang");

                    b.HasIndex("MaSachNavigationMaSach");

                    b.ToTable("ChiTietDonHangs");
                });

            modelBuilder.Entity("DATN_API.Models.DanhGia", b =>
                {
                    b.Property<int>("MaDanhGia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDanhGia"));

                    b.Property<int?>("MaNguoiDung")
                        .HasColumnType("int");

                    b.Property<int?>("MaNguoiDungNavigationMaNguoiDung")
                        .HasColumnType("int");

                    b.Property<int?>("MaSach")
                        .HasColumnType("int");

                    b.Property<int?>("MaSachNavigationMaSach")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("NgayDanhGia")
                        .HasColumnType("date");

                    b.Property<string>("NoiDung")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SoSao")
                        .HasColumnType("int");

                    b.HasKey("MaDanhGia");

                    b.HasIndex("MaNguoiDungNavigationMaNguoiDung");

                    b.HasIndex("MaSachNavigationMaSach");

                    b.ToTable("DanhGia");
                });

            modelBuilder.Entity("DATN_API.Models.DonHang", b =>
                {
                    b.Property<int>("MaDonHang")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDonHang"));

                    b.Property<int?>("MaNguoiDung")
                        .HasColumnType("int");

                    b.Property<int?>("MaNguoiDungNavigationMaNguoiDung")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("NgayDatHang")
                        .HasColumnType("date");

                    b.Property<int?>("TongTien")
                        .HasColumnType("int");

                    b.Property<string>("TrangThai")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaDonHang");

                    b.HasIndex("MaNguoiDungNavigationMaNguoiDung");

                    b.ToTable("DonHangs");
                });

            modelBuilder.Entity("DATN_API.Models.NguoiDung", b =>
                {
                    b.Property<int>("MaNguoiDung")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNguoiDung"));

                    b.Property<string>("DiaChi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatKhau")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgaySinh")
                        .HasColumnType("datetime2");

                    b.Property<string>("Saft")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenNguoiDung")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrangThai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VaiTro")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNguoiDung");

                    b.ToTable("NguoiDungs");
                });

            modelBuilder.Entity("DATN_API.Models.NhaCungCap", b =>
                {
                    b.Property<int>("MaNhaCungCap")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNhaCungCap"));

                    b.Property<string>("DiaChi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenNhaCungCap")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNhaCungCap");

                    b.ToTable("NhaCungCaps");
                });

            modelBuilder.Entity("DATN_API.Models.PhuongThucThanhToan", b =>
                {
                    b.Property<int>("MaPhuongThuc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaPhuongThuc"));

                    b.Property<string>("TenPhuongThuc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaPhuongThuc");

                    b.ToTable("PhuongThucThanhToans");
                });

            modelBuilder.Entity("DATN_API.Models.Sach", b =>
                {
                    b.Property<int>("MaSach")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaSach"));

                    b.Property<int?>("GiaTien")
                        .HasColumnType("int");

                    b.Property<int?>("MaNhaCungCap")
                        .HasColumnType("int");

                    b.Property<int?>("MaNhaCungCapNavigationMaNhaCungCap")
                        .HasColumnType("int");

                    b.Property<int?>("NamXuatBan")
                        .HasColumnType("int");

                    b.Property<int?>("SoLuongNhap")
                        .HasColumnType("int");

                    b.Property<int?>("SoLuongTon")
                        .HasColumnType("int");

                    b.Property<string>("TacGia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenSach")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaSach");

                    b.HasIndex("MaNhaCungCapNavigationMaNhaCungCap");

                    b.ToTable("Saches");
                });

            modelBuilder.Entity("DATN_API.Models.ThanhToan", b =>
                {
                    b.Property<int>("MaThanhToan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaThanhToan"));

                    b.Property<int?>("MaDonHang")
                        .HasColumnType("int");

                    b.Property<int?>("MaDonHangNavigationMaDonHang")
                        .HasColumnType("int");

                    b.Property<int?>("MaPhuongThuc")
                        .HasColumnType("int");

                    b.Property<int?>("MaPhuongThucNavigationMaPhuongThuc")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("NgayThanhToan")
                        .HasColumnType("date");

                    b.Property<int?>("SoTien")
                        .HasColumnType("int");

                    b.Property<string>("TrangThai")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaThanhToan");

                    b.HasIndex("MaDonHangNavigationMaDonHang");

                    b.HasIndex("MaPhuongThucNavigationMaPhuongThuc");

                    b.ToTable("ThanhToans");
                });

            modelBuilder.Entity("DATN_API.Models.TheLoai", b =>
                {
                    b.Property<int>("MaTheLoai")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaTheLoai"));

                    b.Property<string>("TenTheLoai")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaTheLoai");

                    b.ToTable("TheLoais");
                });

            modelBuilder.Entity("DATN_API.Models.VanChuyen", b =>
                {
                    b.Property<int>("MaVanChuyen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaVanChuyen"));

                    b.Property<string>("DiaChiGiao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaDonHang")
                        .HasColumnType("int");

                    b.Property<int?>("MaDonHangNavigationMaDonHang")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("NgayGiaoHang")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("NgayNhanHang")
                        .HasColumnType("date");

                    b.Property<string>("TrangThai")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaVanChuyen");

                    b.HasIndex("MaDonHangNavigationMaDonHang");

                    b.ToTable("VanChuyens");
                });

            modelBuilder.Entity("SachTheLoai", b =>
                {
                    b.Property<int>("MaSachesMaSach")
                        .HasColumnType("int");

                    b.Property<int>("MaTheLoaisMaTheLoai")
                        .HasColumnType("int");

                    b.HasKey("MaSachesMaSach", "MaTheLoaisMaTheLoai");

                    b.HasIndex("MaTheLoaisMaTheLoai");

                    b.ToTable("SachTheLoai");
                });

            modelBuilder.Entity("DATN_API.Models.ChiTietDonHang", b =>
                {
                    b.HasOne("DATN_API.Models.DonHang", "MaDonHangNavigation")
                        .WithMany()
                        .HasForeignKey("MaDonHangNavigationMaDonHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DATN_API.Models.Sach", "MaSachNavigation")
                        .WithMany()
                        .HasForeignKey("MaSachNavigationMaSach")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MaDonHangNavigation");

                    b.Navigation("MaSachNavigation");
                });

            modelBuilder.Entity("DATN_API.Models.DanhGia", b =>
                {
                    b.HasOne("DATN_API.Models.NguoiDung", "MaNguoiDungNavigation")
                        .WithMany("DanhGia")
                        .HasForeignKey("MaNguoiDungNavigationMaNguoiDung");

                    b.HasOne("DATN_API.Models.Sach", "MaSachNavigation")
                        .WithMany("DanhGia")
                        .HasForeignKey("MaSachNavigationMaSach");

                    b.Navigation("MaNguoiDungNavigation");

                    b.Navigation("MaSachNavigation");
                });

            modelBuilder.Entity("DATN_API.Models.DonHang", b =>
                {
                    b.HasOne("DATN_API.Models.NguoiDung", "MaNguoiDungNavigation")
                        .WithMany("DonHangs")
                        .HasForeignKey("MaNguoiDungNavigationMaNguoiDung");

                    b.Navigation("MaNguoiDungNavigation");
                });

            modelBuilder.Entity("DATN_API.Models.Sach", b =>
                {
                    b.HasOne("DATN_API.Models.NhaCungCap", "MaNhaCungCapNavigation")
                        .WithMany("Saches")
                        .HasForeignKey("MaNhaCungCapNavigationMaNhaCungCap");

                    b.Navigation("MaNhaCungCapNavigation");
                });

            modelBuilder.Entity("DATN_API.Models.ThanhToan", b =>
                {
                    b.HasOne("DATN_API.Models.DonHang", "MaDonHangNavigation")
                        .WithMany("ThanhToans")
                        .HasForeignKey("MaDonHangNavigationMaDonHang");

                    b.HasOne("DATN_API.Models.PhuongThucThanhToan", "MaPhuongThucNavigation")
                        .WithMany("ThanhToans")
                        .HasForeignKey("MaPhuongThucNavigationMaPhuongThuc");

                    b.Navigation("MaDonHangNavigation");

                    b.Navigation("MaPhuongThucNavigation");
                });

            modelBuilder.Entity("DATN_API.Models.VanChuyen", b =>
                {
                    b.HasOne("DATN_API.Models.DonHang", "MaDonHangNavigation")
                        .WithMany("VanChuyens")
                        .HasForeignKey("MaDonHangNavigationMaDonHang");

                    b.Navigation("MaDonHangNavigation");
                });

            modelBuilder.Entity("SachTheLoai", b =>
                {
                    b.HasOne("DATN_API.Models.Sach", null)
                        .WithMany()
                        .HasForeignKey("MaSachesMaSach")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DATN_API.Models.TheLoai", null)
                        .WithMany()
                        .HasForeignKey("MaTheLoaisMaTheLoai")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DATN_API.Models.DonHang", b =>
                {
                    b.Navigation("ThanhToans");

                    b.Navigation("VanChuyens");
                });

            modelBuilder.Entity("DATN_API.Models.NguoiDung", b =>
                {
                    b.Navigation("DanhGia");

                    b.Navigation("DonHangs");
                });

            modelBuilder.Entity("DATN_API.Models.NhaCungCap", b =>
                {
                    b.Navigation("Saches");
                });

            modelBuilder.Entity("DATN_API.Models.PhuongThucThanhToan", b =>
                {
                    b.Navigation("ThanhToans");
                });

            modelBuilder.Entity("DATN_API.Models.Sach", b =>
                {
                    b.Navigation("DanhGia");
                });
#pragma warning restore 612, 618
        }
    }
}
