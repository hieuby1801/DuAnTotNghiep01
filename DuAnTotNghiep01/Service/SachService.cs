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
								TongSoLuongDaBan = reader.IsDBNull(reader.GetOrdinal("TongSoLuongDaBan")) ? 0 : reader.GetInt32(reader.GetOrdinal("TongSoLuongDaBan"))
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
            var parameters = new[]
            {
            new SqlParameter("@MaSach", dto.MaSach),
            new SqlParameter("@TenSach", dto.TenSach ?? ""),
            new SqlParameter("@TacGia", dto.TacGia ?? ""),
            new SqlParameter("@HinhAnh", dto.HinhAnh ?? ""),
            new SqlParameter("@TrangThai", dto.TrangThai ?? ""),
            new SqlParameter("@NgonNgu", dto.NgonNgu ?? ""),
            new SqlParameter("@KichThuoc", dto.KichThuoc ?? ""),
            new SqlParameter("@TrongLuong", dto.TrongLuong),
            new SqlParameter("@SoTrang", dto.SoTrang),
            new SqlParameter("@HinhThuc", dto.HinhThuc ?? ""),
            new SqlParameter("@MoTa", dto.MoTa ?? ""),
            new SqlParameter("@DoTuoi", dto.DoTuoi ?? ""),
            new SqlParameter("@ListTheLoai", dto.ListTheLoai ?? "")
        };

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC ThemSach @MaSach, @TenSach, @TacGia, @HinhAnh, @TrangThai, @NgonNgu, @KichThuoc, @TrongLuong, @SoTrang, @HinhThuc, @MoTa, @DoTuoi, @ListTheLoai",
                    parameters);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<SachDto> GetOnlySach()
        {
            return _context.Sach
                .Select(s => new SachDto
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    TacGia = s.TacGia,
                    HinhAnh = s.HinhAnh,
                    TrangThai = s.TrangThai
                })
                .ToList();
        }
        public ThemSachDto GetSachChiTiet(int maSach)
        {
            // Truy vấn sách đồng bộ
            Sach sach = _context.Sach.FirstOrDefault(s => s.MaSach == maSach);
            SachChiTiet chiTiet = _context.SachChiTiet.FirstOrDefault(s => s.MaSach == maSach);

            // Kiểm tra nếu không tìm thấy sách hoặc chi tiết sách
            if (sach == null || chiTiet == null)
            {
                return null;  // Hoặc bạn có thể ném ra một ngoại lệ nếu cần
            }

            // Truy vấn thể loại sách đồng bộ
            var theLoai = _context.SachTheLoai
                .Where(stl => stl.MaSach == maSach)
                .Select(stl => stl.MaTheLoai)
                .ToList();  // Đồng bộ thay vì ToListAsync

            string theLoaiString = string.Join(",", theLoai);

            // Tạo DTO và ánh xạ dữ liệu
            var sachChiTiet = new ThemSachDto
            {
                MaSach = maSach,
                TenSach = sach.TenSach ?? "",  // Kiểm tra NULL cho TenSach
                TacGia = sach.TacGia ?? "",  // Kiểm tra NULL cho TacGia
                HinhAnh = sach.HinhAnh ?? "",  // Kiểm tra NULL cho HinhAnh
                TrangThai = sach.TrangThai ?? "",  // Kiểm tra NULL cho TrangThai
                NgonNgu = chiTiet.NgonNgu ?? "",  // Kiểm tra NULL cho NgonNgu
                KichThuoc = chiTiet.KichThuoc ?? "",  // Kiểm tra NULL cho KichThuoc
                SoTrang = chiTiet.SoTrang ?? 0,  // Kiểm tra NULL cho SoTrang
                HinhThuc = chiTiet.HinhThuc ?? "",  // Kiểm tra NULL cho HinhThuc
                MoTa = chiTiet.MoTa ?? "",  // Kiểm tra NULL cho MoTa
                DoTuoi = chiTiet.DoTuoi ?? "Không rõ",  // Kiểm tra NULL cho DoTuoi
                ListTheLoai = theLoaiString
            };

            return sachChiTiet;
        }

        public async Task<bool> CapNhatSachAsync(ThemSachDto sach)
        {
            var parameters = new[]
            {
				new SqlParameter("@MaSach", sach.MaSach),
				new SqlParameter("@TenSach", sach.TenSach ?? (object)DBNull.Value),
				new SqlParameter("@TacGia", sach.TacGia ?? (object)DBNull.Value),
				new SqlParameter("@HinhAnh", sach.HinhAnh ?? (object)DBNull.Value),
				new SqlParameter("@TrangThai", sach.TrangThai ?? (object)DBNull.Value),
				new SqlParameter("@NgonNgu", sach.NgonNgu ?? (object)DBNull.Value),
				new SqlParameter("@KichThuoc", sach.KichThuoc ?? (object)DBNull.Value),
				new SqlParameter("@TrongLuong", sach.TrongLuong),
				new SqlParameter("@SoTrang", sach.SoTrang),
				new SqlParameter("@HinhThuc", sach.HinhThuc ?? (object)DBNull.Value),
				new SqlParameter("@DoTuoi", sach.DoTuoi ?? (object)DBNull.Value),
				new SqlParameter("@MoTa", sach.MoTa ?? (object)DBNull.Value),
				new SqlParameter("@ListTheLoai", sach.ListTheLoai ?? (object)DBNull.Value)
			};

            try
            {
                var rows = await _context.Database.ExecuteSqlRawAsync("EXEC UpdateSach " +
                "@MaSach, @TenSach, @TacGia, @HinhAnh, @TrangThai, @NgonNgu, @KichThuoc, " +
                "@TrongLuong, @SoTrang, @HinhThuc, @DoTuoi, @MoTa, @ListTheLoai", parameters);

				return true;
            }
            catch
            {
                return false;
            }
        }




    }
}
