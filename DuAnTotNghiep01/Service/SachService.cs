using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DATN_API.Service
{
	public class SachService : ISachService
	{
		private readonly MyDbContext _Context;
		public SachService(MyDbContext context)
		{
			_Context = context;
		}
		public List<SachDTO> Laysach()
		{
			var sachs = _Context.Sach
		.Include(s => s.SachTheLoais)
			.ThenInclude(stl => stl.TheLoai)
		.Select(s => new SachDTO
		{
			MaSach = s.MaSach,
			TenSach = s.TenSach,
			TacGia = s.TacGia,
			GiaTien = s.GiaTien,
			NamXuatBan = s.NamXuatBan,
			SoLuongTon = s.SoLuongTon,
			SoLuongNhap = s.SoLuongNhap,
			MaNhaCungCap = s.MaNhaCungCap,
			HinhAnh = s.HinhAnh,
			TheLoais = s.SachTheLoais.Select(stl => stl.TheLoai.TenTheLoai).ToList()
		})
		.ToList();

			return sachs;
		}
		public List<NhaCungCap> Laynhacung()
		{
			return _Context.NhaCungCap.ToList();
		}
		//
		public List<NhaCungCap> Tencungcap(int macungcap)
		{
			return _Context.NhaCungCap
						   .Where(s => s.MaNhaCungCap == macungcap)
						   .ToList();

		}
        // lấy thể loại
        public List<TheLoai> LayTatCaTheLoai()
		{
			// Lấy tất cả tên thể loại từ bảng TheLoai
			var theLoais = _Context.TheLoai
				.Distinct() // Đảm bảo mỗi thể loại chỉ xuất hiện một lần
				.ToList(); // Chuyển kết quả thành danh sách các đối tượng TheLoai
			// Trả về danh sách các thể loại
			return theLoais;
		}
        // tìm sách theo thể loại trên menu
        public List<SachDTO> Laysachtheotheloai(string tenTheLoai)
        {
            var result = new List<SachDTO>();

            using (var connection = new SqlConnection(_Context.Database.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand("GetBooksByCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenTheLoai", tenTheLoai); // truyền thể loại vào stored procedure

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Tách các thể loại từ chuỗi và sắp xếp
                            var theLoaisRaw = reader["TheLoais"]?.ToString()?.Split(", ")?.ToList() ?? new List<string>();

                            var theLoaisSapXep = theLoaisRaw
                                .OrderByDescending(tl => string.Equals(tl, tenTheLoai, StringComparison.OrdinalIgnoreCase)) // ưu tiên thể loại đang chọn
                                .ThenBy(tl => tl) // các thể loại còn lại sắp xếp ABC
                                .ToList();

                            // Tạo đối tượng sách
                            var sach = new SachDTO
                            {
                                MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
                                TenSach = reader["TenSach"]?.ToString(),
                                TacGia = reader["TacGia"]?.ToString(),
                                GiaTien = reader["GiaTien"] as int?,
                                NamXuatBan = reader["NamXuatBan"] as int?,
                                SoLuongTon = reader["SoLuongTon"] as int?,
                                MaNhaCungCap = reader["MaNhaCungCap"] as int?,
                                HinhAnh = reader["HinhAnh"]?.ToString(),
                                TheLoais = theLoaisSapXep
                            };

                            result.Add(sach);
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }
       
        public List<Sach> Laysachtu2theloaitrolen(List<string> dstheloai)
		{
			var result = new List<Sach>();

			using (var connection = new SqlConnection(_Context.Database.GetConnectionString()))
			{
				connection.Open();

				// Tạo DataTable khớp với user-defined table type
				DataTable theLoaiTable = new DataTable();
				theLoaiTable.Columns.Add("TenTheLoai", typeof(string));
				foreach (var theLoai in dstheloai)
				{
					theLoaiTable.Rows.Add(theLoai);
				}

				using (var command = new SqlCommand("GetBooksByCategories_All", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					var tvpParam = new SqlParameter("@TheLoaiList", SqlDbType.Structured)
					{
						TypeName = "dbo.TheLoaiList",
						Value = theLoaiTable
					};
					command.Parameters.Add(tvpParam);

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							var sach = new Sach
							{
								MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
								TenSach = reader["TenSach"]?.ToString(),
								TacGia = reader["TacGia"]?.ToString(),
								GiaTien = reader["GiaTien"] as int?,
								NamXuatBan = reader["NamXuatBan"] as int?,
								SoLuongTon = reader["SoLuongTon"] as int?,
								MaNhaCungCap = reader["MaNhaCungCap"] as int?,
								HinhAnh = reader["HinhAnh"]?.ToString()
							};
							result.Add(sach);
						}
					}
				}

				connection.Close();
			}

			return result;
		}
        // tìm sách theo thể loại
        public List<SachDTO> Laysachtheo1trong2theloai(List<string> dstheloai)
		{
			var result = new List<SachDTO>();

			using (var connection = new SqlConnection(_Context.Database.GetConnectionString()))
			{
				connection.Open();

				// Tạo DataTable cho TVP
				var table = new DataTable();
				table.Columns.Add("TenTheLoai", typeof(string));

				// Thêm các thể loại vào DataTable
				foreach (var theLoai in dstheloai)
				{
					table.Rows.Add(theLoai);
				}

				using (var command = new SqlCommand("GetBooksByCategories", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					// Thêm tham số TVP vào câu lệnh SQL
					var tvpParam = new SqlParameter("@TheLoaiList", SqlDbType.Structured)
					{
						TypeName = "dbo.TheLoaiList",  // Tên kiểu TVP trong SQL
						Value = table
					};
					command.Parameters.Add(tvpParam);
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							var theLoaisRaw = reader["TheLoais"]?.ToString()?.Split(", ").ToList() ?? new List<string>();
							// Ưu tiên sắp xếp theo danh sách bạn đã nhập
							var theLoaisSapXep = theLoaisRaw
								.OrderByDescending(tl => dstheloai.Contains(tl)) // ưu tiên cái bạn nhập
								.ThenBy(tl => tl) // phụ: ABC
								.ToList();
							SachDTO sach = new SachDTO
							{
								MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
								TenSach = reader["TenSach"]?.ToString(),
								TacGia = reader["TacGia"]?.ToString(),
								GiaTien = reader["GiaTien"] as int?,
								NamXuatBan = reader["NamXuatBan"] as int?,
								SoLuongTon = reader["SoLuongTon"] as int?,
								MaNhaCungCap = reader["MaNhaCungCap"] as int?,
								HinhAnh = reader["HinhAnh"]?.ToString(),
								TheLoais = theLoaisSapXep,
							};
							result.Add(sach);
						}
					}
				}
				connection.Close();
			}
			return result;
		}
        //Thêm sách
        public SachDTO ThemSach(SachDTO TTSach)
        {
            using (var connection = new SqlConnection(_Context.Database.GetConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_ThemSachVaTheLoai", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Truyền tham số
                    command.Parameters.AddWithValue("@MaSach", TTSach.MaSach);
                    command.Parameters.AddWithValue("@TenSach", TTSach.TenSach ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TacGia", TTSach.TacGia ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GiaTien", TTSach.GiaTien ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@HinhAnh", TTSach.HinhAnh ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NamXuatBan", TTSach.NamXuatBan ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SoLuongTon", TTSach.SoLuongTon ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MaNhaCungCap", TTSach.MaNhaCungCap ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TrangThai", "on");
                    command.Parameters.AddWithValue("@SoLuongNhap", TTSach.SoLuongNhap ?? (object)DBNull.Value);

                    // Chuyển List<int> thành chuỗi '1,2,3'
                    string danhSachTheLoai = string.Join(",", TTSach.DanhSachMaTheLoai ?? new List<int>());
                    command.Parameters.AddWithValue("@DanhSachMaTheLoai", danhSachTheLoai);

                    command.ExecuteNonQuery();

                    // Sau khi thêm xong, trả lại DTO vừa thêm
                    return new SachDTO
                    {
                        MaSach = TTSach.MaSach,
                        TenSach = TTSach.TenSach,
                        TacGia = TTSach.TacGia,
                        GiaTien = TTSach.GiaTien,
                        HinhAnh = TTSach.HinhAnh,
                        NamXuatBan = TTSach.NamXuatBan,
                        SoLuongTon = TTSach.SoLuongTon,
                        MaNhaCungCap = TTSach.MaNhaCungCap,
                        SoLuongNhap = TTSach.SoLuongNhap,
                        DanhSachMaTheLoai = TTSach.DanhSachMaTheLoai
                    };
                }
            }
        }

        // cap nhap sach
        public SachDTO Timsach(int Masach)
        {
            using (var connection = new SqlConnection(_Context.Database.GetConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_ThongTinSachTheoMa", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaSach", Masach);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var dto = new SachDTO
                            {
                                MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
                                TenSach = reader["TenSach"]?.ToString(),
                                TacGia = reader["TacGia"]?.ToString(),
                                GiaTien = reader["GiaTien"] != DBNull.Value ? Convert.ToInt32(reader["GiaTien"]) : null,
                                NamXuatBan = reader["NamXuatBan"] != DBNull.Value ? Convert.ToInt32(reader["NamXuatBan"]) : null,
                                SoLuongTon = reader["SoLuongTon"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongTon"]) : null,
                                SoLuongNhap = reader["SoLuongNhap"] != DBNull.Value ? Convert.ToInt32(reader["SoLuongNhap"]) : null,
                                TrangThai = reader["TrangThai"]?.ToString(),
                                HinhAnh = reader["HinhAnh"]?.ToString(),
                                TenNhaCungCap = reader["TenNhaCungCap"]?.ToString(),
                                TheLoais = reader["TheLoai"]?.ToString()?.Split(", ").ToList(), // tách chuỗi thành list
                                DanhSachMaTheLoai = reader["DanhSachMaTheLoai"] != DBNull.Value
                                    ? reader["DanhSachMaTheLoai"].ToString().Split(", ").Select(int.Parse).ToList()
                                    : new List<int>()
                            };

                            return dto;
                        }
                    }
                }
            }
            return null;
        }
        public SachDTO CapNhatSach(SachDTO sach)
        {
            using (var connection = new SqlConnection(_Context.Database.GetConnectionString()))
            {
                connection.Open();

                using (var command = new SqlCommand("sp_CapNhatThongTinSach", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaSach", sach.MaSach);
                    command.Parameters.AddWithValue("@TenSach", sach.TenSach ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TacGia", sach.TacGia ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GiaTien", sach.GiaTien ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@HinhAnh", sach.HinhAnh ?? (object)DBNull.Value);

                    string danhSachTheLoai = sach.DanhSachMaTheLoai != null
                        ? string.Join(",", sach.DanhSachMaTheLoai)
                        : "";

                    command.Parameters.AddWithValue("@DanhSachTheLoai", danhSachTheLoai ?? (object)DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }

            // Sau khi cập nhật xong, lấy lại thông tin sách từ database
            var sachMoi = _Context.Sach.FirstOrDefault(s => s.MaSach == sach.MaSach);
            if (sachMoi == null) return null;

            return new SachDTO
            {
                MaSach = sachMoi.MaSach,
                TenSach = sachMoi.TenSach,
                TacGia = sachMoi.TacGia,
                GiaTien = sachMoi.GiaTien,
                NamXuatBan = sachMoi.NamXuatBan,
                SoLuongTon = sachMoi.SoLuongTon,
                SoLuongNhap = sachMoi.SoLuongNhap,
                HinhAnh = sachMoi.HinhAnh,
                TrangThai = sachMoi.TrangThai,
                MaNhaCungCap = sachMoi.MaNhaCungCap,
                // nếu cần thêm TenNhaCungCap hoặc thể loại thì truy vấn thêm
            };
        }
    }
}


