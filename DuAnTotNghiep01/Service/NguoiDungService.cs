using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using DATN_API.Models;
using System.Data;

namespace DATN_API.Service
{
    public class NguoiDungService : INguoiDungService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration; // To access connection string

        public NguoiDungService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task ThemNhanVienAsync(string ten, string email, string soDienThoai, string diaChi, string vaiTro, string trangThai = "on")
        {
            var sql = "EXEC sp_ThemNhanVien @TenNhanVien, @Email, @SoDienThoai, @DiaChi, @VaiTro, @TrangThai";

            var parameters = new[]
            {
            new SqlParameter("@TenNhanVien", ten),
            new SqlParameter("@Email", email),
            new SqlParameter("@SoDienThoai", soDienThoai),
            new SqlParameter("@DiaChi", diaChi),
            new SqlParameter("@VaiTro", vaiTro),
            new SqlParameter("@TrangThai", trangThai)
        };

            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }

}
