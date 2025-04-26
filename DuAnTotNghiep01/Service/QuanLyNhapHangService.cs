using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_API.Service
{
    public class QuanLyNhapHangService : IQuanLyNhapHangService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;
        public QuanLyNhapHangService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public List<LoHang> getAllLoHang()
        {
            return _context.loHang.ToList();
        }
        public async Task<bool> insertLoHang(LoHangDTO dto)
        {
            var connectionString = _configuration.GetConnectionString("con");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("InsertLoHang", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MaNhaCungCap", dto.MaNhaCungCap);
                    command.Parameters.AddWithValue("@GiaTienLoHang", dto.GiaTienLoHang);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }
        public Task<bool> insertChiTietLoHang(List<ChiTietLoHangDTO> chiTietLoHangs)
        {

        }
    }
}
