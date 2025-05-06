using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DATN_API.Service
{
	public class VanChuyenService : IVanChuyenService
	{
		private readonly MyDbContext _Context;
		private readonly IConfiguration _configuration; // To access connection string
		public VanChuyenService(MyDbContext context, IConfiguration configuration)
		{
			_Context = context;
			_configuration = configuration;
		}
		public List<VanChuyen> GetVanChuyenss()
		{
			return _Context.VanChuyen.ToList();
		}
		public List<VanChuyenDTOs> Layvanchuyenshipper()
		{
			var connectionString = _configuration.GetConnectionString("con");
			var vanChuyens = new List<VanChuyenDTOs>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var query = @"
        SELECT 
            vc.MaVanChuyen,
            vc.MaDonHang,
            vc.NgayGiaoHang,
            vc.TrangThai,
            vc.DiaChiGiao,
            vc.SDT,
            dh.TongTien,
            s.TenSach,
            s.HinhAnh,
            ctdh.SoLuong,
            ctdh.GiaTien
        FROM VanChuyen vc
        JOIN DonHang dh ON vc.MaDonHang = dh.MaDonHang
        JOIN ChiTietDonHang ctdh ON dh.MaDonHang = ctdh.MaDonHang
        JOIN Sach s ON ctdh.MaSach = s.MaSach
        WHERE vc.TrangThai = N'Đang chờ nhận hàng'";

				using (SqlCommand command = new SqlCommand(query, connection))
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						int maVC = reader.GetInt32(0);  // Cột MaVanChuyen
						var vanChuyen = vanChuyens.FirstOrDefault(x => x.MaVanChuyen == maVC);

						if (vanChuyen == null)
						{
							vanChuyen = new VanChuyenDTOs
							{
								MaVanChuyen = maVC,
								MaDonHang = reader.GetInt32(1), // Cột MaDonHang
								NgayGiaohang = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2), // Cột NgayGiaoHang
								TrangThai = reader.IsDBNull(3) ? null : reader.GetString(3), // Cột TrangThai
								DiaChi = reader.IsDBNull(4) ? "" : reader.GetString(4), // Cột DiaChiGiao
								SDT = reader.IsDBNull(5) ? "" : reader.GetString(5), // Cột SDT
								TongTien = reader.IsDBNull(6) ? 0 : reader.GetInt32(6), // Cột TongTien
								SanPhams = new List<SanPhamDto>()
							};

							vanChuyens.Add(vanChuyen);
						}

						// Thêm sản phẩm vào danh sách của VanChuyen hiện tại
						vanChuyen.SanPhams.Add(new SanPhamDto
						{
							TenSach = reader.IsDBNull(7) ? "" : reader.GetString(7), // Cột TenSach
							HinhAnh = reader.IsDBNull(8) ? "" : reader.GetString(8), // Cột HinhAnh
							SoLuong = reader.IsDBNull(9) ? 0 : reader.GetInt32(9), // Cột SoLuong
							GiaTien = reader.IsDBNull(10) ? 0 : reader.GetInt32(10) // Cột GiaTien
						});
					}
				}
			}

			return vanChuyens;
		}
		public VanChuyenDTOs capnhapvanchuyen(VanChuyenDTOs vanchuyen)
		{
			var connectionString = _configuration.GetConnectionString("con");

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var query = @"
						UPDATE VanChuyen
						SET TrangThai = @TrangThai,
						 NgayNhanHang = GETDATE()
						WHERE MaVanChuyen = @MaVanChuyen";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@TrangThai", vanchuyen.TrangThai ?? (object)DBNull.Value);
					command.Parameters.AddWithValue("@MaVanChuyen", vanchuyen.MaVanChuyen);
					command.ExecuteNonQuery();
				}
			}

			return vanchuyen; // Trả về lại bản ghi sau khi cập nhật
		}
		public List<VanChuyenDTOs> LayVanChuyenTheoTrangThai(string trangthai)
		{
			var connectionString = _configuration.GetConnectionString("con");
			var vanChuyens = new List<VanChuyenDTOs>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				var query = @"
            SELECT 
                vc.MaVanChuyen,
                vc.MaDonHang,
                vc.NgayGiaoHang,
                vc.TrangThai,
                vc.DiaChiGiao,
                vc.SDT,
                dh.TongTien,
                s.TenSach,
                s.HinhAnh,
                ctdh.SoLuong,
                ctdh.GiaTien
            FROM VanChuyen vc
            JOIN DonHang dh ON vc.MaDonHang = dh.MaDonHang
            JOIN ChiTietDonHang ctdh ON dh.MaDonHang = ctdh.MaDonHang
            JOIN Sach s ON ctdh.MaSach = s.MaSach
            WHERE vc.TrangThai = @TrangThai";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@TrangThai", trangthai);

					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							int maVC = reader.GetInt32(0);
							var vanChuyen = vanChuyens.FirstOrDefault(x => x.MaVanChuyen == maVC);

							if (vanChuyen == null)
							{
								vanChuyen = new VanChuyenDTOs
								{
									MaVanChuyen = maVC,
									MaDonHang = reader.GetInt32(1),
									NgayGiaohang = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
									TrangThai = reader.IsDBNull(3) ? null : reader.GetString(3),
									DiaChi = reader.IsDBNull(4) ? "" : reader.GetString(4),
									SDT = reader.IsDBNull(5) ? "" : reader.GetString(5),
									TongTien = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
									SanPhams = new List<SanPhamDto>()
								};

								vanChuyens.Add(vanChuyen);
							}

							vanChuyen.SanPhams.Add(new SanPhamDto
							{
								TenSach = reader.IsDBNull(7) ? "" : reader.GetString(7),
								HinhAnh = reader.IsDBNull(8) ? "" : reader.GetString(8),
								SoLuong = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
								GiaTien = reader.IsDBNull(10) ? 0 : reader.GetInt32(10)
							});
						}
					}
				}
			}

			return vanChuyens;
		}
		public string CapNhatTrangThaiDonHang(int maDonHang)
		{
			var connectionString = _configuration.GetConnectionString("con");
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand("CapNhatTrangThaiDonHangVanChuyen", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					// Thêm tham số vào thủ tục
					command.Parameters.Add("@MaDonHang", SqlDbType.Int).Value = maDonHang;

					try
					{
						// Thực thi thủ tục
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								int ketQua = reader.GetInt32(reader.GetOrdinal("KetQua"));
								string thongBao = reader.GetString(reader.GetOrdinal("ThongBao"));

								// Nếu thủ tục thành công, trả về thông báo thành công
								if (ketQua == 1)
								{
									return "Cập nhật trạng thái thành công: " + thongBao;
								}
								else
								{
									return "Lỗi: " + thongBao;
								}
							}
						}

						// Trả về thông báo mặc định nếu không có dữ liệu trả về
						return "Không có kết quả từ thủ tục lưu trữ."; // Hoặc bất kỳ thông báo mặc định nào bạn muốn
					}
					catch (Exception ex)
					{
						return "Lỗi: " + ex.Message;
					}
				}
			}
		}

	}

}
