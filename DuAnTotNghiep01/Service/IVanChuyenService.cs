using DATN_API.DTOs;
using DATN_API.Models;

namespace DATN_API.Service
{
    public interface IVanChuyenService
    {
        public List<VanChuyen> GetVanChuyenss();
        public List<VanChuyenDTOs> Layvanchuyenshipper();
        public VanChuyenDTOs capnhapvanchuyen(VanChuyenDTOs vanchuyen);
        public List<VanChuyenDTOs> LayVanChuyenTheoTrangThai(string trangthai);
        public string CapNhatTrangThaiDonHang(int maDonHang);

	}
}
