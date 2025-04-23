using DATN_API.DTOs;
using DATN_API.Models;

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

    }
}
