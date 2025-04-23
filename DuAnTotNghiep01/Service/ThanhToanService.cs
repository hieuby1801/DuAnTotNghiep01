using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DATN_API.Service
{
	public class ThanhToanService : IThanhToanService
	{
		private readonly MyDbContext _context;
		private readonly IConfiguration _configuration; // To access connection string
		public ThanhToanService(MyDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}
		// Thanh  toán giỏ hàng qua QR
		public List<GioHangDTO> LayThongTinGioHang(List<int> danhSachMaGioHang, int maNguoiDung)
		{
			var gioHangList = new List<GioHangDTO>();
			var connectionString = _configuration.GetConnectionString("con");

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				try
				{
					conn.Open();

					using (SqlCommand cmd = new SqlCommand("LayThongTinGioHang", conn))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						// Chuyển danh sách mã giỏ hàng thành chuỗi
						var danhSachMaGioHangString = string.Join(",", danhSachMaGioHang);

						// Thêm tham số vào câu lệnh
						cmd.Parameters.Add(new SqlParameter("@MaNguoiDung", maNguoiDung));
						cmd.Parameters.Add(new SqlParameter("@DanhSachMaGioHang", danhSachMaGioHangString));

						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								var gioHang = new GioHangDTO
								{
									MaSach = reader.GetInt32(reader.GetOrdinal("MaSach")),
									TenSach = reader.GetString(reader.GetOrdinal("TenSach")),
									SoLuong = reader.GetInt32(reader.GetOrdinal("SoLuong")),
									GiaBan = reader.GetInt32(reader.GetOrdinal("GiaBan")),
									HinhAnh = reader.GetString(reader.GetOrdinal("HinhAnh")),
								};

								gioHangList.Add(gioHang);
							}
						}
					}
				}
				catch { }
			}
			return gioHangList;
		}
		//Thêm đơn hàng 
		public int Themdonhang(int manguoidung)
		{
			var connectionString = _configuration.GetConnectionString("con");

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand("TaoDonHangMoi", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@MaNguoiDung", manguoidung);

					// Trả về mã đơn hàng mới từ stored procedure
					return Convert.ToInt32(command.ExecuteScalar());
				}
			}
		}
		//Thêm chi Tiết đơn hàng
		public ChiTietDonHang ThemChiTietDonHang(DonHangChiTietDTOs donHangChiTietDTOs)
		{
			var connectionString = _configuration.GetConnectionString("con");

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand("ThemChiTietDonHang", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					// Tạo DataTable để truyền vào TVP (table-valued parameter)
					DataTable chiTietTable = new DataTable();
					chiTietTable.Columns.Add("MaSach", typeof(int));
					chiTietTable.Columns.Add("SoLuong", typeof(int));

					for (int i = 0; i < donHangChiTietDTOs.MaSach.Count; i++)
					{
						chiTietTable.Rows.Add(donHangChiTietDTOs.MaSach[i], donHangChiTietDTOs.SoLuong[i]);
					}

					cmd.Parameters.AddWithValue("@MaDonHang", donHangChiTietDTOs.MaDonHang);

					SqlParameter tvpParam = cmd.Parameters.AddWithValue("@ChiTietDonHangs", chiTietTable);
					tvpParam.SqlDbType = SqlDbType.Structured;
					tvpParam.TypeName = "dbo.ChiTietDonHangType";

					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}

			// Trả về 1 bản ghi đầu tiên hoặc null (nếu bạn cần trả danh sách thì return List<ChiTietDonHang>)
			return new ChiTietDonHang
			{
				MaDonHang = donHangChiTietDTOs.MaDonHang,
				MaSach = donHangChiTietDTOs.MaSach[0],
				SoLuong = donHangChiTietDTOs.SoLuong[0]
			};
		}


		// Thanh toán giỏ hàng bằng tiền mặt
		public VanChuyenDTOs ThemVaoVanChuyen(VanChuyenDTOs vanChuyenDTOs)
		{
			var connectionString = _configuration.GetConnectionString("con");

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand("ThemVaoVanChuyen", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@MaDonHang", vanChuyenDTOs.MaDonHang);
					command.Parameters.AddWithValue("@DiaChi", vanChuyenDTOs.DiaChi);
					command.Parameters.AddWithValue("@SDT", vanChuyenDTOs.SDT);

					// Trả về mã đơn hàng mới từ stored procedure
					return new VanChuyenDTOs
					{
						MaDonHang = Convert.ToInt32(command.ExecuteScalar()),
						DiaChi = vanChuyenDTOs.DiaChi,
						SDT = vanChuyenDTOs.SDT,
						NgayNhanHang = vanChuyenDTOs.NgayNhanHang,
					};
				}
			}
		}

	}
}
