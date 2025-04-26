using DATN_API.DTOs;
using DATN_API.Models;

namespace DATN_API.Service
{
    public interface IQuanLyNhapHangService
    {
        public List<LoHang> getAllLoHang();
        public Task<bool> insertLoHang(LoHangDTO dto);
        public Task<bool> insertChiTietLoHang(List<ChiTietLoHangDTO> chiTietLoHangs);
    }
}
