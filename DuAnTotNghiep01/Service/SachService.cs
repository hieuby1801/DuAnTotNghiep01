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
		public List<Sach> Laysach()
		{
			return _Context.Sach.Where(s => s.TrangThai == "on").ToList();
		}
		public List<NhaCungCap> Tencungcap(int macungcap)
		{
			return _Context.NhaCungCap
						   .Where(s => s.MaNhaCungCap == macungcap)
						   .ToList();

		}
		public List<TheLoai> LayTatCaTheLoai()
		{
			// Lấy tất cả tên thể loại từ bảng TheLoai
			var theLoais = _Context.TheLoai
				.Distinct() // Đảm bảo mỗi thể loại chỉ xuất hiện một lần
				.ToList(); // Chuyển kết quả thành danh sách các đối tượng TheLoai
			// Trả về danh sách các thể loại
			return theLoais;
		}
		public List<Sach> Laysachtheotheloai(string tenTheLoai)
		{
			var result = new List<Sach>();
			using (var connection = new SqlConnection(_Context.Database.GetConnectionString()))
			{
				connection.Open();
				using (var command = new SqlCommand("GetBooksByCategory", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@TenTheLoai", tenTheLoai); // Truyền thể loại vào thủ tục

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Sach sach = new Sach
							{
								MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
								TenSach = reader["TenSach"]?.ToString(),
								TacGia = reader["TacGia"]?.ToString(),
								GiaTien = reader["GiaTien"] as int?,
								NamXuatBan = reader["NamXuatBan"] as int?,
								SoLuongTon = reader["SoLuongTon"] as int?,
								MaNhaCungCap = reader["MaNhaCungCap"] as int?,
								HinhAnh = reader["HinhAnh"]?.ToString(),
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
		public List<Sach> Laysachtheo1trong2theloai(List<string> dstheloai)
		{
			var result = new List<Sach>();

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
							Sach sach = new Sach
							{
								MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
								TenSach = reader["TenSach"]?.ToString(),
								TacGia = reader["TacGia"]?.ToString(),
								GiaTien = reader["GiaTien"] as int?,
								NamXuatBan = reader["NamXuatBan"] as int?,
								SoLuongTon = reader["SoLuongTon"] as int?,
								MaNhaCungCap = reader["MaNhaCungCap"] as int?,
								HinhAnh = reader["HinhAnh"]?.ToString(),
							};
							result.Add(sach);
						}
					}
				}

				connection.Close();
			}

			return result;
		}

	}
}

