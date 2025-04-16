using DATN_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Service
{
    public interface INguoiDungService
    {
        public Task ThemNhanVienAsync(string ten, string email, string soDienThoai, string diaChi, string vaiTro, string trangThai = "on");
    }
}
