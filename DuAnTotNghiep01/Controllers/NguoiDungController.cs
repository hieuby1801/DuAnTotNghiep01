using DATN_API.DTOs;
using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly INguoiDungService _nguoiDungService;

        public NguoiDungController(INguoiDungService nguoiDungService)
        {
            _nguoiDungService = nguoiDungService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(NguoiDungDTO dto)
        {
            await _nguoiDungService.ThemNhanVienAsync(
                dto.TenNhanVien,
                dto.Email,
                dto.SoDienThoai,
                dto.DiaChi,
                dto.VaiTro,
                dto.TrangThai);

            return Ok(new { message = "Thêm nhân viên thành công" });
        }
    }

}
