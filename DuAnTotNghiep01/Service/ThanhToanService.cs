using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
		public int Themdonhang(int manguoidung,int PhiVanChuyen)
		{
			var connectionString = _configuration.GetConnectionString("con");

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new SqlCommand("TaoDonHangMoi", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@MaNguoiDung", manguoidung);
					command.Parameters.AddWithValue("@TongTien", PhiVanChuyen);

					// Trả về mã đơn hàng mới từ stored procedure
					return Convert.ToInt32(command.ExecuteScalar());
				}
			}
		}
        //Thêm chi Tiết đơn hàng
        public List<ChiTietDonHang> ThemChiTietDonHang(DonHangChiTietDTOs donHangChiTietDTOs)
        {
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ThemChiTietDonHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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

            List<ChiTietDonHang> listChiTiet = new List<ChiTietDonHang>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var query = @"
            SELECT MaSach, GiaTien, SoLuong
            FROM ChiTietDonHang
            WHERE MaDonHang = @MaDonHang";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDonHang", donHangChiTietDTOs.MaDonHang);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int maSach = reader.GetInt32(0);
                            int giaTien = reader.GetInt32(1);
                            int soLuong = reader.GetInt32(2);

                            listChiTiet.Add(new ChiTietDonHang
                            {
                                MaDonHang = donHangChiTietDTOs.MaDonHang,
                                MaSach = maSach,
                                SoLuong = soLuong,
                                GiaTien = giaTien
                            });
                        }
                    }
                }
            }

            return listChiTiet;
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
		public bool CapNhatTrangThaiDonHang(int maDonHang, string trangThai)
		{
			// Gọi stored procedure để cập nhật trạng thái đơn hàng
			var parameters = new[]
			{
		new SqlParameter("@MaDonHang", maDonHang),
		new SqlParameter("@TrangThai", trangThai)
	};

			var result = _context.Database.ExecuteSqlRaw("EXEC dbo.CapNhatTrangThaiDonHang @MaDonHang, @TrangThai", parameters);
			return result > 0;  // Trả về true nếu cập nhật thành công, false nếu thất bại
		}

		public DonHang LaydonhangtheoidDH(int id)
		{
			var donhang = _context.DonHang
				.Where(dh => dh.MaDonHang == id)
				.FirstOrDefault(); // Trả về toàn bộ đối tượng DonHang
			return donhang; // Trả về đối tượng DonHang
		}
	}
}
