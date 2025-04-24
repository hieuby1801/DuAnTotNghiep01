using DATN_API.DTOs;
using DATN_API.Models;

namespace DATN_API.Service
{
    public interface IThanhToanService
    {
		public List<GioHangDTO> LayThongTinGioHang(List<int> danhSachMaGioHang, int maNguoiDung);
		public int Themdonhang(int manguoidung);
		public ChiTietDonHang ThemChiTietDonHang(DonHangChiTietDTOs donHangChiTietDTOs);
		public VanChuyenDTOs ThemVaoVanChuyen(VanChuyenDTOs vanChuyenDTOs);
		public bool CapNhatTrangThaiDonHang(int maDonHang, string trangThai);

	}
}
