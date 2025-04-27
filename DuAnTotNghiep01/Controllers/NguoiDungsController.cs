using DATN_API.DTOs;
using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungsController : ControllerBase
    {
        private readonly INguoiDungService _nguoiDungService;

        public NguoiDungsController(INguoiDungService nguoiDungService)
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

        [HttpGet("TatCaDonHang/{id}")]
        public IActionResult TatCaDonHang(int id)
		{
			var result =  _nguoiDungService.TatCaDonHang(id);
			if (result == null)
			{
				return NotFound(new { message = "Không tìm thấy đơn hàng" });
			}
			return Ok(result);
		}
		[HttpGet("trangthaiXL/{id}/{trangThai}")]
		public IActionResult LayDonHangTheoTrangThaiVC(int id, string trangThai)
		{
			var result = _nguoiDungService.LayDonHangTheoTrangThaiXL(id, trangThai);
			if (result == null)
			{
				return NotFound(new { message = "Không tìm thấy đơn hàng" });
			}
			return Ok(result);
		}
		[HttpGet("trangthaiDH/{id}/{trangThai}")]
        public IActionResult LayDonHangTheoTrangThaiDH(int id, string trangThai)
		{
			var result = _nguoiDungService.LayDonHangTheoTrangThai(id, trangThai);
			if (result == null)
			{
				return NotFound(new { message = "Không tìm thấy đơn hàng" });
			}
			return Ok(result);
		}
		[HttpDelete("huydon/{maDonHang}")]
		public IActionResult HuyDon(int maDonHang) 
		{
			var result = _nguoiDungService.HuyDonHang(maDonHang);
			if (result)
			{
				return Ok(new { message = "Hủy đơn hàng thành công" });
			}
			else
			{
				return NotFound(new { message = "Không tìm thấy đơn hàng" });
			}
		}
        [HttpPut("MuaLai/{maDonHang}")]
        public IActionResult Mualai(int maDonHang)
        {
            var result = _nguoiDungService.DatLaiDonHang(maDonHang);

            if (result)
            {
                return Ok(new { message = "Đặt lại đơn hàng thành công" });
            }
            else
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng hoặc không thể cập nhật trạng thái" });
            }
        }

    }

}
