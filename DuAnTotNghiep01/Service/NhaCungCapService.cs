using DATN_API.DTOs;
using DATN_API.Models;

namespace DATN_API.Service
{
    public class NhaCungCapService : INhaCungCapService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration; // To access connection string
        public NhaCungCapService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<NhaCungCapDto> GetAllNhaCungCap()
        {
            return _context.nhaCungCaps
                .Select(ncc => new NhaCungCapDto
                {
                    MaNhaCungCap = ncc.MaNhaCungCap,
                    TenNhaCungCap = ncc.TenNhaCungCap,
                    SDT = ncc.SDT,
                    Email = ncc.Email,
                    TrangThai = ncc.TrangThai
                })
                .ToList(); 
        }
    }
}
