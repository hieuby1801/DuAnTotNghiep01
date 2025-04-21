using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DATN_API.Service
{
    public class ThanhToanService :  IThanhToanService
    {
		private readonly MyDbContext _context;
		private readonly IConfiguration _configuration; // To access connection string
		public ThanhToanService(MyDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}
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
					catch (Exception ex)
					{
						Console.WriteLine("Lỗi khi lấy thông tin giỏ hàng: " + ex.Message);
					}
				}

				return gioHangList;
        }



	}
}
