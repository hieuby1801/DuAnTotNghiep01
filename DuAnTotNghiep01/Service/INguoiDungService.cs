using DATN_API.DTOs;
using DATN_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Service
{
    public interface INguoiDungService
    {
        public Task ThemNhanVienAsync(string ten, string email, string soDienThoai, string diaChi, string vaiTro, string trangThai = "on");

        public List<QuanLyDonUserDTOs> TatCaDonHang(int id);
        public List<QuanLyDonUserDTOs> LayDonHangTheoTrangThaiXL(int id, string trangThai);
        public List<QuanLyDonUserDTOs> LayDonHangTheoTrangThai(int id, string trangThai);
        public bool HuyDonHang(int maDonHang);
        public bool DatLaiDonHang(int maDonHang);

    }
}
