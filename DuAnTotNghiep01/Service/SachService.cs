using DATN_API.Models;
using Microsoft.Data.SqlClient;
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
                                TheLoai = reader.GetString(reader.GetOrdinal("TheLoai"))
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


    }
}
