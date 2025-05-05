using System.Data;
using System.Data.SqlClient;
using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace DATN_API.Service
{
    public class ThongKeService : IThongKeService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration; // To access connection string
        public ThongKeService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<ThongKeDoanhThuNgayDTO> ThongKeDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            List<ThongKeDoanhThuNgayDTO> danhSach = new List<ThongKeDoanhThuNgayDTO>();

            var connectionString = _configuration.GetConnectionString("con");
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ThongKe_DoanhThu_TheoNgay", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThongKeDoanhThuNgayDTO item = new ThongKeDoanhThuNgayDTO
                            {
                                Ngay = reader.GetDateTime(reader.GetOrdinal("Ngay")),
                                TongDoanhThu = reader.GetInt32(reader.GetOrdinal("TongDoanhThu"))
                            };

                            danhSach.Add(item);
                        }
                    }
                }
            }

            return danhSach;
        }
    }
}
