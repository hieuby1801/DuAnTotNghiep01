using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.Data.SqlClient;

namespace DATN_API.Service
{
	public class QuanKhoservice : Iquankhoservice
	{
		private readonly MyDbContext _context;
		private readonly IConfiguration _configuration; // To access connection string
		public QuanKhoservice(MyDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}
		public VanChuyen Duyetvanchuyen(DuyetVanChuyenRequest request)
		{
			var connectionString = _configuration.GetConnectionString("con");
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlTransaction transaction = connection.BeginTransaction();

				try
				{
					// 1. Cập nhật trạng thái và ngày nhận hàng trong bảng VanChuyen
					string updateVanChuyenQuery = @"
				UPDATE VanChuyen
				SET TrangThai = N'Đang chờ nhận hàng',
					NgayGiaoHang = GETDATE()
				WHERE MaVanChuyen = @MaVanChuyen;
			";

					using (SqlCommand cmd1 = new SqlCommand(updateVanChuyenQuery, connection, transaction))
					{
						cmd1.Parameters.AddWithValue("@MaVanChuyen", request.MaVanChuyen);
						cmd1.ExecuteNonQuery();
					}

					// 2. Cập nhật số lượng tồn kho (trừ đi số lượng đã đặt)
					string updateTonKhoQuery = @"
				UPDATE tk
				SET tk.SoLuongTon = tk.SoLuongTon - ctdh.SoLuong
				FROM TonKho tk
				JOIN ChiTietDonHang ctdh ON ctdh.MaSach = tk.MaSach
				JOIN DonHang dh ON dh.MaDonHang = ctdh.MaDonHang
				WHERE dh.MaDonHang = @MaDonHang;
			";

					using (SqlCommand cmd2 = new SqlCommand(updateTonKhoQuery, connection, transaction))
					{
						cmd2.Parameters.AddWithValue("@MaDonHang", request.MaDonHang);
						cmd2.ExecuteNonQuery();
					}

					// 3. Lấy dữ liệu VanChuyen sau khi cập nhật
					string getVanChuyenQuery = "SELECT * FROM VanChuyen WHERE MaVanChuyen = @MaVanChuyen";

					VanChuyen vanChuyen = null;

					using (SqlCommand cmd3 = new SqlCommand(getVanChuyenQuery, connection, transaction))
					{
						cmd3.Parameters.AddWithValue("@MaVanChuyen", request.MaVanChuyen);
						using (SqlDataReader reader = cmd3.ExecuteReader())
						{
							if (reader.Read())
							{
								vanChuyen = new VanChuyen
								{
									MaVanChuyen = reader.GetInt32(reader.GetOrdinal("MaVanChuyen")),
									TrangThai = reader.GetString(reader.GetOrdinal("TrangThai")),
									NgayGiaoHang = reader.GetDateTime(reader.GetOrdinal("NgayGiaoHang")),
									// Thêm các cột khác nếu cần
								};
							}
						}
					}

					transaction.Commit();
					return vanChuyen;
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}
		public List<TonKho> Laytonkho ()
		{
			return _context.TonKho.ToList();
		}



	}
}
