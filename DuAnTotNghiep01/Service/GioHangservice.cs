using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DATN_API.Service
{
    public class GioHangservice : IGioHnagservice
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration; // To access connection string
        public GioHangservice(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        // thêm giỏ hàng khi chưa đăng nhập
        public GioHangDTO Themgiohang(int masach)
        {
            GioHangDTO sach = null;

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
                            sach = new GioHangDTO
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
        // thêm vào giỏ hàng khi đã đăng nhập
        public async Task<bool> ThemgiohangDN(int masach, int id, int soluong)
        {
            try
            {
                var sql = "INSERT INTO GioHang (MaNguoiDung, MaSach, SoLuong, ThoiGian) VALUES (@id, @masach, @soluong, GETDATE())";
                var parameters = new[]
                {
            new SqlParameter("@id", id),
            new SqlParameter("@masach", masach),
            new SqlParameter("@soluong", soluong),
        };

                int result = await _context.Database.ExecuteSqlRawAsync(sql, parameters);

                // Nếu thêm thành công, ExecuteSqlRawAsync trả về số dòng ảnh hưởng > 0
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Ghi log nếu cần
                return false;
            }
        }
        // cập nhật giỏ hàng
        public async Task<bool> CapNhatGioHang(int masach, int id, int soluong)
        {
            try
            {
                var sql = "UPDATE GioHang SET SoLuong = @soluong, ThoiGian = GETDATE() WHERE MaNguoiDung = @id AND MaSach = @masach";

                var parameters = new[]
                {
            new SqlParameter("@id", id),
            new SqlParameter("@masach", masach),
            new SqlParameter("@soluong", soluong)
        };

                // Thực thi câu lệnh SQL để cập nhật
                int result = await _context.Database.ExecuteSqlRawAsync(sql, parameters);

                // Nếu cập nhật thành công, ExecuteSqlRawAsync sẽ trả về số dòng ảnh hưởng > 0
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Ghi log nếu cần
                return false;
            }
        }

		//kiểm tra giỏ hàng 
		public GioHang KiemTra(int masach, int maNguoiDung)
		{
			return _context.GioHang.FirstOrDefault(s => s.MaSach == masach && s.MaNguoiDung == maNguoiDung);
		}

		// Lấy Giỏ hàng theo MaNguoiDung
		public List<GioHang> Laygiohnagtheoid(int manguoidung)
        {
            var gioHangs = _context.GioHang
        .Where(s => s.MaNguoiDung == manguoidung)
        .ToList();
            return gioHangs;
        }
    }
}
