using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN_API.Migrations
{
    /// <inheritdoc />
    public partial class suabang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Saft = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCaps",
                columns: table => new
                {
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCaps", x => x.MaNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToans",
                columns: table => new
                {
                    MaPhuongThuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhuongThuc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToans", x => x.MaPhuongThuc);
                });

            migrationBuilder.CreateTable(
                name: "TheLoais",
                columns: table => new
                {
                    MaTheLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTheLoai = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoais", x => x.MaTheLoai);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NgayDatHang = table.Column<DateOnly>(type: "date", nullable: true),
                    TongTien = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNguoiDungNavigationMaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHangs_NguoiDungs_MaNguoiDungNavigationMaNguoiDung",
                        column: x => x.MaNguoiDungNavigationMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "Saches",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSach = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TacGia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiaTien = table.Column<int>(type: "int", nullable: true),
                    NamXuatBan = table.Column<int>(type: "int", nullable: true),
                    SoLuongTon = table.Column<int>(type: "int", nullable: true),
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongNhap = table.Column<int>(type: "int", nullable: true),
                    MaNhaCungCapNavigationMaNhaCungCap = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saches", x => x.MaSach);
                    table.ForeignKey(
                        name: "FK_Saches_NhaCungCaps_MaNhaCungCapNavigationMaNhaCungCap",
                        column: x => x.MaNhaCungCapNavigationMaNhaCungCap,
                        principalTable: "NhaCungCaps",
                        principalColumn: "MaNhaCungCap");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    MaThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: true),
                    MaPhuongThuc = table.Column<int>(type: "int", nullable: true),
                    NgayThanhToan = table.Column<DateOnly>(type: "date", nullable: true),
                    SoTien = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaDonHangNavigationMaDonHang = table.Column<int>(type: "int", nullable: true),
                    MaPhuongThucNavigationMaPhuongThuc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.MaThanhToan);
                    table.ForeignKey(
                        name: "FK_ThanhToans_DonHangs_MaDonHangNavigationMaDonHang",
                        column: x => x.MaDonHangNavigationMaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang");
                    table.ForeignKey(
                        name: "FK_ThanhToans_PhuongThucThanhToans_MaPhuongThucNavigationMaPhuongThuc",
                        column: x => x.MaPhuongThucNavigationMaPhuongThuc,
                        principalTable: "PhuongThucThanhToans",
                        principalColumn: "MaPhuongThuc");
                });

            migrationBuilder.CreateTable(
                name: "VanChuyens",
                columns: table => new
                {
                    MaVanChuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: true),
                    NgayGiaoHang = table.Column<DateOnly>(type: "date", nullable: true),
                    NgayNhanHang = table.Column<DateOnly>(type: "date", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChiGiao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaDonHangNavigationMaDonHang = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VanChuyens", x => x.MaVanChuyen);
                    table.ForeignKey(
                        name: "FK_VanChuyens_DonHangs_MaDonHangNavigationMaDonHang",
                        column: x => x.MaDonHangNavigationMaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    GiaTien = table.Column<int>(type: "int", nullable: true),
                    MaDonHangNavigationMaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaSachNavigationMaSach = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_MaDonHangNavigationMaDonHang",
                        column: x => x.MaDonHangNavigationMaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_Saches_MaSachNavigationMaSach",
                        column: x => x.MaSachNavigationMaSach,
                        principalTable: "Saches",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGia",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    MaSach = table.Column<int>(type: "int", nullable: true),
                    SoSao = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayDanhGia = table.Column<DateOnly>(type: "date", nullable: true),
                    MaNguoiDungNavigationMaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    MaSachNavigationMaSach = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGia", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGia_NguoiDungs_MaNguoiDungNavigationMaNguoiDung",
                        column: x => x.MaNguoiDungNavigationMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_DanhGia_Saches_MaSachNavigationMaSach",
                        column: x => x.MaSachNavigationMaSach,
                        principalTable: "Saches",
                        principalColumn: "MaSach");
                });

            migrationBuilder.CreateTable(
                name: "SachTheLoai",
                columns: table => new
                {
                    MaSachesMaSach = table.Column<int>(type: "int", nullable: false),
                    MaTheLoaisMaTheLoai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SachTheLoai", x => new { x.MaSachesMaSach, x.MaTheLoaisMaTheLoai });
                    table.ForeignKey(
                        name: "FK_SachTheLoai_Saches_MaSachesMaSach",
                        column: x => x.MaSachesMaSach,
                        principalTable: "Saches",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SachTheLoai_TheLoais_MaTheLoaisMaTheLoai",
                        column: x => x.MaTheLoaisMaTheLoai,
                        principalTable: "TheLoais",
                        principalColumn: "MaTheLoai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaDonHangNavigationMaDonHang",
                table: "ChiTietDonHangs",
                column: "MaDonHangNavigationMaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaSachNavigationMaSach",
                table: "ChiTietDonHangs",
                column: "MaSachNavigationMaSach");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaNguoiDungNavigationMaNguoiDung",
                table: "DanhGia",
                column: "MaNguoiDungNavigationMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaSachNavigationMaSach",
                table: "DanhGia",
                column: "MaSachNavigationMaSach");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaNguoiDungNavigationMaNguoiDung",
                table: "DonHangs",
                column: "MaNguoiDungNavigationMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Saches_MaNhaCungCapNavigationMaNhaCungCap",
                table: "Saches",
                column: "MaNhaCungCapNavigationMaNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_SachTheLoai_MaTheLoaisMaTheLoai",
                table: "SachTheLoai",
                column: "MaTheLoaisMaTheLoai");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_MaDonHangNavigationMaDonHang",
                table: "ThanhToans",
                column: "MaDonHangNavigationMaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_MaPhuongThucNavigationMaPhuongThuc",
                table: "ThanhToans",
                column: "MaPhuongThucNavigationMaPhuongThuc");

            migrationBuilder.CreateIndex(
                name: "IX_VanChuyens_MaDonHangNavigationMaDonHang",
                table: "VanChuyens",
                column: "MaDonHangNavigationMaDonHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "DanhGia");

            migrationBuilder.DropTable(
                name: "SachTheLoai");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "VanChuyens");

            migrationBuilder.DropTable(
                name: "Saches");

            migrationBuilder.DropTable(
                name: "TheLoais");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToans");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "NhaCungCaps");

            migrationBuilder.DropTable(
                name: "NguoiDungs");
        }
    }
}
