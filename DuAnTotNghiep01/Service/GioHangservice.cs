using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.AspNetCore.Components.Forms;
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
		public async Task<bool> ThemgiohangDN(List<(int masach, int soluong)> products, int id)
		{
			try
			{
				// Tạo truy vấn SQL với nhiều giá trị
				var sql = "INSERT INTO GioHang (MaNguoiDung, MaSach, SoLuong, ThoiGian) VALUES ";

				// Tạo danh sách các tham số cho mỗi sản phẩm
				var parameters = new List<SqlParameter>();
				var values = new List<string>();

				foreach (var product in products)
				{
					// Thêm các giá trị vào danh sách values và thêm tham số vào parameters
					values.Add("(@id, @masach" + product.masach + ", @soluong" + product.masach + ", GETDATE())");

					parameters.Add(new SqlParameter($"@id", id));
					parameters.Add(new SqlParameter($"@masach{product.masach}", product.masach));
					parameters.Add(new SqlParameter($"@soluong{product.masach}", product.soluong));
				}

				// Nối tất cả các giá trị vào câu SQL
				sql += string.Join(", ", values);

				// Thực hiện truy vấn batch insert
				int result = await _context.Database.ExecuteSqlRawAsync(sql, parameters);

				// Nếu thêm thành công, ExecuteSqlRawAsync trả về số dòng ảnh hưởng > 0
				return result > 0;
			}
			catch 
			{
				
				return false;
			}
		}
		// cập nhật giỏ hàng
		public GioHang CapNhatGioHang(CapNhatGioHangRequest request)
		{
			try
			{
				// Kiểm tra xem bản ghi có tồn tại không
				var gioHang = _context.Giohang
					.FirstOrDefault(g => g.MaNguoiDung == request.MaNguoiDung && g.MaSach == request.MaSach);

				if (gioHang == null)
				{
					// Nếu không có bản ghi → trả về null hoặc có thể tạo mới tùy nhu cầu
					return null;
				}

				// Kiểm tra sự thay đổi thực sự của SoLuong
				if (gioHang.SoLuong == request.SoLuong)
				{
					// Nếu không thay đổi, trả về giỏ hàng hiện tại
					return gioHang;
				}

				// Nếu số lượng = 0 thì xóa khỏi giỏ
				if (request.SoLuong == 0)
				{
					_context.Giohang.Remove(gioHang);
				}
				else
				{
					// Ngược lại, cập nhật số lượng và thời gian
					gioHang.SoLuong = request.SoLuong;
					
					_context.Giohang.Update(gioHang);
				}

				// Lưu thay đổi đồng bộ
				_context.SaveChanges();

				// Trả về giỏ hàng đã được cập nhật
				return gioHang;
			}
			catch (Exception ex)
			{
				// Nếu có lỗi, trả về null
				return null;
			}
		}

		//kiểm tra giỏ hàng 
		public GioHang KiemTra(int masach, int maNguoiDung)
		{
			return _context.Giohang.FirstOrDefault(s => s.MaSach == masach && s.MaNguoiDung == maNguoiDung);
		}

		// Lấy Giỏ hàng theo MaNguoiDung
		public List<GioHang> Laygiohnagtheoid(int manguoidung)
		{
			var gioHangs = _context.Giohang
		.Where(s => s.MaNguoiDung == manguoidung)
		.ToList();
			return gioHangs;
		}
		// thêm danh sách giỏ hàng
		public List<GioHang> ThemdangsachGiohangck(List<CapNhatGioHangRequest> requests)
		{
			var dsGioHangKetQua = new List<GioHang>();

			foreach (var request in requests)
			{
				var gioHangs = _context.Giohang
					.Where(x => x.MaSach == request.MaSach && x.MaNguoiDung == request.MaNguoiDung)
					.ToList();

				if (gioHangs.Any())
				{
					foreach (var item in gioHangs)
					{
						item.SoLuong += request.SoLuong;
						
						
					}
					dsGioHangKetQua.AddRange(gioHangs);
				}
				else
				{
					var sachTonTai = _context.Sach.Any(x => x.MaSach == request.MaSach);
					if (sachTonTai)
					{
						var gioHangMoi = new GioHang
						{
							MaSach = request.MaSach,
							MaNguoiDung = request.MaNguoiDung,
							SoLuong = request.SoLuong,
							
						};

						_context.Giohang.Add(gioHangMoi);
						dsGioHangKetQua.Add(gioHangMoi);
					}
				}
			}

			_context.SaveChanges();
			return dsGioHangKetQua;
		}


		// xoa gio hang dang nhap
		public List<GioHang> XoaGiohangDN(List<int> danhSachMaSach, int idnd)
		{
			var gioHangs = _context.Giohang
				.Where(x => danhSachMaSach.Contains(x.MaSach) && x.MaNguoiDung == idnd)
				.ToList();

			if (gioHangs.Any())
			{
				_context.Giohang.RemoveRange(gioHangs);
				_context.SaveChanges();
				return gioHangs;
			}

			return new List<GioHang>(); // Trả về danh sách rỗng nếu không tìm thấy
		}

	}
}
