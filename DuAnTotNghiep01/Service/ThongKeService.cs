using System.Data;
using System.Data.SqlClient;
using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace DATN_API.Service
{
    public class ThongKeService : IThongKeService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration; // To access connection string
        public ThongKeService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<ThongKeDoanhThuNgayDTO> ThongKeDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            List<ThongKeDoanhThuNgayDTO> danhSach = new List<ThongKeDoanhThuNgayDTO>();

            var connectionString = _configuration.GetConnectionString("con");
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ThongKe_DoanhThu_TheoNgay", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThongKeDoanhThuNgayDTO item = new ThongKeDoanhThuNgayDTO
                            {
                                Ngay = reader.GetDateTime(reader.GetOrdinal("Ngay")),
                                TongDoanhThu = reader.GetInt32(reader.GetOrdinal("TongDoanhThu"))
                            };

                            danhSach.Add(item);
                        }
                    }
                }
            }

            return danhSach;
        }
        public List<ThongKeDoanhThuNgayDTO> ThongKeDoanhThuTheoThang(DateTime tuNgay, DateTime denNgay)
        {
            List<ThongKeDoanhThuNgayDTO> danhSach = new List<ThongKeDoanhThuNgayDTO>();

            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ThongKe_DoanhThu_TheoThang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThongKeDoanhThuNgayDTO item = new ThongKeDoanhThuNgayDTO
                            {
                                Ngay = new DateTime(reader.GetInt32(reader.GetOrdinal("Nam")), reader.GetInt32(reader.GetOrdinal("Thang")), 1),
                                TongDoanhThu = reader.GetInt32(reader.GetOrdinal("TongDoanhThu"))
                            };

                            danhSach.Add(item);
                        }
                    }
                }
            }

            return danhSach;
        }

        public List<ThongKeDoanhThuNgayDTO> ThongKeDoanhThuTheoNam(DateTime tuNgay, DateTime denNgay)
        {
            List<ThongKeDoanhThuNgayDTO> danhSach = new List<ThongKeDoanhThuNgayDTO>();

            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ThongKe_DoanhThu_TheoNam", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThongKeDoanhThuNgayDTO item = new ThongKeDoanhThuNgayDTO
                            {
                                Ngay = new DateTime(reader.GetInt32(reader.GetOrdinal("Nam")), 1, 1),
                                TongDoanhThu = reader.GetInt32(reader.GetOrdinal("TongDoanhThu"))
                            };

                            danhSach.Add(item);
                        }
                    }
                }
            }

            return danhSach;
        }
        public List<ThongKeDoanhThuSachDTO> ThongKeDoanhThuTheoSach(DateTime tuNgay, DateTime denNgay)
        {
            var danhSach = new List<ThongKeDoanhThuSachDTO>();
            var connStr = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("ThongKe_DoanhThu_TheoSach", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSach.Add(new ThongKeDoanhThuSachDTO
                        {
                            MaSach = reader["MaSach"].ToString(),
                            TenSach = reader["TenSach"].ToString(),
                            TongSoLuongBan = Convert.ToInt32(reader["TongSoLuongBan"]),
                            TongDoanhThu = Convert.ToInt32(reader["TongDoanhThu"]),
                            SoDonHang = Convert.ToInt32(reader["SoDonHang"])
                        });
                    }
                }
            }

            return danhSach;
        }
        public List<ThongKeDoanhThuTheLoaiDTO> ThongKeDoanhThuTheoTheLoai(DateTime tuNgay, DateTime denNgay)
        {
            var danhSach = new List<ThongKeDoanhThuTheLoaiDTO>();
            var connStr = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("ThongKe_DoanhThu_TheoTheLoai", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        danhSach.Add(new ThongKeDoanhThuTheLoaiDTO
                        {
                            MaTheLoai = reader["MaTheLoai"].ToString(),
                            TenTheLoai = reader["TenTheLoai"].ToString(),
                            TongSoLuongBan = Convert.ToInt32(reader["TongSoLuongBan"]),
                            TongDoanhThu = Convert.ToInt32(reader["TongDoanhThu"]),
                            SoDonHang = Convert.ToInt32(reader["SoDonHang"])
                        });
                    }
                }
            }

            return danhSach;
        }

    }
}
