using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using DATN_API.Models;
using System.Data;
using DATN_API.DTOs;

namespace DATN_API.Service
{
    public class NguoiDungService : INguoiDungService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration; // To access connection string

        public NguoiDungService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task ThemNhanVienAsync(string ten, string email, string soDienThoai, string diaChi, string vaiTro, string trangThai = "on")
        {
            var sql = "EXEC sp_ThemNhanVien @TenNhanVien, @Email, @SoDienThoai, @DiaChi, @VaiTro, @TrangThai";

            var parameters = new[]
            {
            new SqlParameter("@TenNhanVien", ten),
            new SqlParameter("@Email", email),
            new SqlParameter("@SoDienThoai", soDienThoai),
            new SqlParameter("@DiaChi", diaChi),
            new SqlParameter("@VaiTro", vaiTro),
            new SqlParameter("@TrangThai", trangThai)
        };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public List<QuanLyDonUserDTOs> TatCaDonHang(int id)
        {
            List<QuanLyDonUserDTOs> donHangs = new List<QuanLyDonUserDTOs>();
            var connectionString = _configuration.GetConnectionString("con");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
            SELECT 
                dh.MaDonHang,
                dh.TongTien,
                dh.TrangThai AS TrangThaiDonHang,
                vc.TrangThai AS TrangThaiVanChuyen,
                ctdh.MaSach,
                s.HinhAnh,
                s.TenSach,
                ctdh.SoLuong,
                vc.DiaChiGiao,
                vc.NgayGiaoHang as NgayDatHang
            FROM DonHang dh
            INNER JOIN ChiTietDonHang ctdh ON dh.MaDonHang = ctdh.MaDonHang
            LEFT JOIN VanChuyen vc ON dh.MaDonHang = vc.MaDonHang
            INNER JOIN Sach s ON ctdh.MaSach = s.MaSach
            WHERE dh.MaNguoiDung = @MaNguoiDung AND dh.TrangThai != N'Đã thanh toán' ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int maDonHang = reader.GetInt32(reader.GetOrdinal("MaDonHang"));

                            // Check xem đã có đơn hàng này trong danh sách chưa
                            var donHang = donHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);

                            if (donHang == null)
                            {
                                // Nếu chưa có, tạo mới
                                donHang = new QuanLyDonUserDTOs
                                {
                                    MaDonHang = maDonHang,
                                    TongTien = reader.GetInt32(reader.GetOrdinal("TongTien")),
                                    TrangThaiDonHang = reader.GetString(reader.GetOrdinal("TrangThaiDonHang")),
                                    TrangThaiVanChuyen = reader.IsDBNull(reader.GetOrdinal("TrangThaiVanChuyen")) ? null : reader.GetString(reader.GetOrdinal("TrangThaiVanChuyen")),
                                    DiaChiGiao = reader.IsDBNull(reader.GetOrdinal("DiaChiGiao")) ? null : reader.GetString(reader.GetOrdinal("DiaChiGiao")),
                                    NgayDatHang = reader.IsDBNull(reader.GetOrdinal("NgayDatHang")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("NgayDatHang")),
                                    ChiTietDonHangs = new List<ChiTietDonHangViewModel>()
                                };
                                donHangs.Add(donHang);
                            }

                            // Thêm chi tiết sách vào đơn hàng
                            ChiTietDonHangViewModel chiTiet = new ChiTietDonHangViewModel
                            {
                                MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
                                HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh")),
                                TenSach = reader.GetString(reader.GetOrdinal("TenSach")),
                                SoLuong = reader.GetInt32(reader.GetOrdinal("SoLuong"))
                            };

                            donHang.ChiTietDonHangs.Add(chiTiet);
                        }
                    }
                }
            }

            return donHangs;
        }

        // trang thai van chuyen đang xử lý
        public List<QuanLyDonUserDTOs> LayDonHangTheoTrangThaiXL(int id, string trangThai)
        {
            List<QuanLyDonUserDTOs> donHangs = new List<QuanLyDonUserDTOs>();
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
        SELECT 
            dh.MaDonHang,
            dh.TongTien,
            dh.TrangThai AS TrangThaiDonHang,
            vc.TrangThai AS TrangThaiVanChuyen,
            ctdh.MaSach,
            s.HinhAnh,
            s.TenSach,
            ctdh.SoLuong,
            vc.DiaChiGiao,
            vc.NgayGiaoHang AS NgayDatHang
        FROM DonHang dh
        INNER JOIN ChiTietDonHang ctdh ON dh.MaDonHang = ctdh.MaDonHang
        LEFT JOIN VanChuyen vc ON dh.MaDonHang = vc.MaDonHang
        INNER JOIN Sach s ON ctdh.MaSach = s.MaSach
        WHERE dh.MaNguoiDung = @MaNguoiDung
            AND vc.TrangThai = @TrangThai";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", id);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int maDonHang = reader.GetInt32(reader.GetOrdinal("MaDonHang"));
                            var donHang = donHangs.FirstOrDefault(x => x.MaDonHang == maDonHang);

                            if (donHang == null)
                            {
                                donHang = new QuanLyDonUserDTOs
                                {
                                    MaDonHang = maDonHang,
                                    TongTien = reader.GetInt32(reader.GetOrdinal("TongTien")),
                                    TrangThaiDonHang = reader.GetString(reader.GetOrdinal("TrangThaiDonHang")),
                                    TrangThaiVanChuyen = reader.IsDBNull(reader.GetOrdinal("TrangThaiVanChuyen")) ? null : reader.GetString(reader.GetOrdinal("TrangThaiVanChuyen")),
                                    DiaChiGiao = reader.IsDBNull(reader.GetOrdinal("DiaChiGiao")) ? null : reader.GetString(reader.GetOrdinal("DiaChiGiao")),
                                    NgayDatHang = reader.IsDBNull(reader.GetOrdinal("NgayDatHang")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("NgayDatHang")),
                                    ChiTietDonHangs = new List<ChiTietDonHangViewModel>()
                                };
                                donHangs.Add(donHang);
                            }

                            ChiTietDonHangViewModel chiTiet = new ChiTietDonHangViewModel
                            {
                                MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
                                HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh")),
                                TenSach = reader.GetString(reader.GetOrdinal("TenSach")),
                                SoLuong = reader.GetInt32(reader.GetOrdinal("SoLuong"))
                            };

                            donHang.ChiTietDonHangs.Add(chiTiet);
                        }
                    }
                }
            }

            return donHangs;
        }

        // trang thai don hang đang chờ thanh toán
        public List<QuanLyDonUserDTOs> LayDonHangTheoTrangThai(int id, string trangThai)
        {
            List<QuanLyDonUserDTOs> donHangs = new List<QuanLyDonUserDTOs>();
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
            SELECT 
                dh.MaDonHang,
                dh.TongTien,
                dh.TrangThai AS TrangThaiDonHang,
                vc.TrangThai AS TrangThaiVanChuyen,
                ctdh.MaSach,
                s.HinhAnh,
                s.TenSach,
                ctdh.SoLuong,
                vc.DiaChiGiao,
                vc.NgayGiaoHang AS NgayDatHang
            FROM DonHang dh
            INNER JOIN ChiTietDonHang ctdh ON dh.MaDonHang = ctdh.MaDonHang
            LEFT JOIN VanChuyen vc ON dh.MaDonHang = vc.MaDonHang
            INNER JOIN Sach s ON ctdh.MaSach = s.MaSach
            WHERE dh.MaNguoiDung = @MaNguoiDung
              AND dh.TrangThai = @TrangThai
            ORDER BY dh.MaDonHang DESC"; // lấy đơn mới trước nếu thích

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNguoiDung", id);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int maDonHang = reader.GetInt32(reader.GetOrdinal("MaDonHang"));

                            // Kiểm tra đơn này đã tồn tại chưa
                            var donHang = donHangs.FirstOrDefault(dh => dh.MaDonHang == maDonHang);

                            if (donHang == null)
                            {
                                // Nếu chưa có, tạo mới đơn
                                donHang = new QuanLyDonUserDTOs
                                {
                                    MaDonHang = maDonHang,
                                    TongTien = reader.GetInt32(reader.GetOrdinal("TongTien")),
                                    TrangThaiDonHang = reader.GetString(reader.GetOrdinal("TrangThaiDonHang")),
                                    TrangThaiVanChuyen = reader.IsDBNull(reader.GetOrdinal("TrangThaiVanChuyen")) ? null : reader.GetString(reader.GetOrdinal("TrangThaiVanChuyen")),
                                    DiaChiGiao = reader.IsDBNull(reader.GetOrdinal("DiaChiGiao")) ? null : reader.GetString(reader.GetOrdinal("DiaChiGiao")),
                                    NgayDatHang = reader.IsDBNull(reader.GetOrdinal("NgayDatHang")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("NgayDatHang")),
                                    ChiTietDonHangs = new List<ChiTietDonHangViewModel>()
                                };
                                donHangs.Add(donHang);
                            }

                            // Thêm sách vào chi tiết đơn
                            var chiTiet = new ChiTietDonHangViewModel
                            {
                                MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
                                TenSach = reader.GetString(reader.GetOrdinal("TenSach")),
                                HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh")),
                                SoLuong = reader.GetInt32(reader.GetOrdinal("SoLuong"))
                            };
                            donHang.ChiTietDonHangs.Add(chiTiet);
                        }
                    }
                }
            }
            return donHangs;
        }
        // xóa đơn hàng 
        public bool HuyDonHang(int maDonHang)
        {
            bool isSuccess = false;
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
            UPDATE DonHang
            SET TrangThai = N'Hủy đơn'
            WHERE MaDonHang = @MaDonHang";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDonHang", maDonHang);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        isSuccess = true;
                    }
                }
            }

            return isSuccess;
        }
        // đặt hàng
        public bool DatLaiDonHang(int maDonHang)
        {
            bool isSuccess = false;
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
            UPDATE DonHang
            SET 
            TrangThai = N'Đang chờ thanh toán',
            NgayDatHang = @NgayDat
            WHERE MaDonHang = @MaDonHang";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDonHang", maDonHang);
                    cmd.Parameters.AddWithValue("@NgayDat", DateTime.Now); // ✅ cập nhật ngày giờ hiện tại

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        isSuccess = true;
                    }
                }
            }

            return isSuccess;
        }
      

    }

}
