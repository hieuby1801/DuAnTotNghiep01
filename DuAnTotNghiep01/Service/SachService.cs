using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DATN_API.Service
{
    public class SachService : ISachservice
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration; // To access connection string
        public SachService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public List<TheLoai> GetAll()
        {
            return _context.TheLoai.ToList();
        }
        public SachDTO GetSach(int maSach)
        {
            // Khai báo đối tượng sach để trả về
            SachDTO sach = null;

            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBookDetailsByMaSach", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaSach", maSach);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Lấy duy nhất 1 dòng
                        {
                            sach = new SachDTO
                            {
                                MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
                                TenSach = reader.GetString(reader.GetOrdinal("TenSach")),
                                TacGia = reader.GetString(reader.GetOrdinal("TacGia")),
                                HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh")),
                                NgonNgu = reader.GetString(reader.GetOrdinal("NgonNgu")),
                                KichThuoc = reader.GetString(reader.GetOrdinal("KichThuoc")),
                                TrongLuong = reader.GetDecimal(reader.GetOrdinal("TrongLuong")),
                                SoTrang = reader.GetInt32(reader.GetOrdinal("SoTrang")),
                                HinhThuc = reader.GetString(reader.GetOrdinal("HinhThuc")),
                                MoTa = reader.GetString(reader.GetOrdinal("MoTa")),
                                TheLoai = reader.GetString(reader.GetOrdinal("TheLoai")),
                                TenNhaCungCap = reader.GetString(reader.GetOrdinal("TenNhaCungCap")),
                                NhaXuatBan = reader.GetString(reader.GetOrdinal("NhaXuatBan")),
                                GiaBan = reader.GetInt32(reader.GetOrdinal("GiaBan")),
                            };
                        }
                    }
                }
            }
            return sach; // Trả về đối tượng SachDTO duy nhất
        }
        public List<SachDTO> Laytontinsach()
        {
            List<SachDTO> result = new List<SachDTO>();
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Tạo command để gọi stored procedure
                using (SqlCommand cmd = new SqlCommand("GetThongTinSach_DangBan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Mở kết nối
                    con.Open();

                    // Thực thi câu lệnh và lấy dữ liệu
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Đọc từng dòng dữ liệu từ SqlDataReader
                        while (reader.Read())
                        {
                            SachDTO sach = new SachDTO
                            {
                                MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
                                TenSach = reader.GetString(reader.GetOrdinal("TenSach")),
                                HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh")),
                                GiaBan = reader.GetInt32(reader.GetOrdinal("GiaBan")),
                                TongSoLuongDaBan = reader.GetInt32(reader.GetOrdinal("TongSoLuongDaBan"))
                            };

                            result.Add(sach);
                        }
                    }
                }
            }

            return result; // Trả về danh sách các sách
        }
        public List<SachDTO> Timsachtheotheloai(string TenTheLoai)
        {
            List<SachDTO> list = new List<SachDTO>();
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSachTheoTheLoai", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenTheLoai", TenTheLoai);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SachDTO sach = new SachDTO
                            {
                                MaSach = Convert.ToInt32(reader["MaSach"]),
                                TenSach = reader["TenSach"].ToString(),
                                GiaBan = reader["GiaBan"] != DBNull.Value ? (int?)reader["GiaBan"] : null,
                                HinhAnh = reader["HinhAnh"].ToString()
                            };
                            list.Add(sach);
                        }
                    }
                }
            }

            return list;
        }
        public List<SachDTO> Timsachtheothongtinnhap(string tenSach = null, int? khoangGia = null, string doTuoi = null, string tacGia = null, List<string> theLoai = null)
        {
            List<SachDTO> danhSachSach = new List<SachDTO>();
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("TimKiemSach", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Truyền tham số
                    cmd.Parameters.AddWithValue("@TenSach", (object)tenSach ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@KhoangGia", (object)khoangGia ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DoTuoi", (object)doTuoi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TacGia", (object)tacGia ?? DBNull.Value);

                    // Chuyển List<string> thể loại thành chuỗi
                    var theLoaiString = theLoai != null && theLoai.Any() ? string.Join(",", theLoai) : null;
                    cmd.Parameters.AddWithValue("@TheLoai", (object)theLoaiString ?? DBNull.Value);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? theLoaiChuoi = reader.IsDBNull(reader.GetOrdinal("TheLoai"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("TheLoai"));

                            var sach = new SachDTO
                            {
                                MaSach = reader.IsDBNull(reader.GetOrdinal("MaSach")) ? 0 : reader.GetInt32(reader.GetOrdinal("MaSach")),
                                TenSach = reader.IsDBNull(reader.GetOrdinal("TenSach")) ? null : reader.GetString(reader.GetOrdinal("TenSach")),
                                TacGia = reader.IsDBNull(reader.GetOrdinal("TacGia")) ? null : reader.GetString(reader.GetOrdinal("TacGia")),
                                HinhAnh = reader.IsDBNull(reader.GetOrdinal("HinhAnh")) ? null : reader.GetString(reader.GetOrdinal("HinhAnh")),
                                GiaBan = reader.IsDBNull(reader.GetOrdinal("GiaBan")) ? 0 : reader.GetInt32(reader.GetOrdinal("GiaBan")),
                                DoTuoi = reader.IsDBNull(reader.GetOrdinal("DoTuoi")) ? null : reader.GetString(reader.GetOrdinal("DoTuoi")),
                                TheLoais = string.IsNullOrWhiteSpace(theLoaiChuoi)
                                    ? new List<string>()
                                    : theLoaiChuoi.Split(',').Select(t => t.Trim()).ToList()
                            };

                            danhSachSach.Add(sach);
                        }
                    }
                }
            }

            return danhSachSach;
        }
        public GioHang Themgiohang(int masach)
        {
            GioHang sach = null;

            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetBookDetailsByMaSach", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaSach", masach);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Lấy duy nhất 1 dòng
                        {
                            sach = new GioHang
                            {
                                MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
                                TenSach = reader.GetString(reader.GetOrdinal("TenSach")),
                                GiaBan = reader.GetInt32(reader.GetOrdinal("GiaBan")),
                                HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh"))
                            };
                        }
                    }
                }
            }
            return sach; // Trả về đối tượng SachDTO duy nhất
        }
        public async Task<List<Sach>> GetAllAsync()
        {
            return await _context.Sach
                .Select(s => new Sach
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    TacGia = s.TacGia,
                    HinhAnh = s.HinhAnh,
                    TrangThai = s.TrangThai
                })
                .ToListAsync();
        }

        public async Task<bool> ThemSachAsync(ThemSachDto dto)
        {
            try
            {
                var sql = "EXEC sp_ThemSach @MaSach, @TenSach, @TacGia, @HinhAnh, @NgonNgu, @KichThuoc, @TrongLuong, @SoTrang, @HinhThuc, @MoTa, @DanhSachTheLoai";

                var parameters = new[]
                {
            new SqlParameter("@MaSach", dto.MaSach),
            new SqlParameter("@TenSach", dto.TenSach),
            new SqlParameter("@TacGia", dto.TacGia),
            new SqlParameter("@HinhAnh", dto.HinhAnh),
            new SqlParameter("@NgonNgu", dto.NgonNgu),
            new SqlParameter("@KichThuoc", dto.KichThuoc),
            new SqlParameter("@TrongLuong", dto.TrongLuong),
            new SqlParameter("@SoTrang", dto.SoTrang),
            new SqlParameter("@HinhThuc", dto.HinhThuc),
            new SqlParameter("@MoTa", dto.MoTa),
            new SqlParameter("@DanhSachTheLoai", dto.DanhSachTheLoai),
        };

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
