using DATN_API.DTOs;
using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VanChuyenController : ControllerBase
    {
        private readonly IVanChuyenService _vanChuyenService;
        public VanChuyenController(IVanChuyenService vanChuyenService)
        {
            _vanChuyenService = vanChuyenService;
        }
        [HttpGet("GetVanChuyen")]
        public List<VanChuyen> GetVanChuyenss()
        {
            var vanchuyen = _vanChuyenService.GetVanChuyenss();
            return vanchuyen;
        }
        [HttpGet("GetVanChuyenShipper")]
        public List<VanChuyenDTOs> GetVanChuyenShipper()
        {
            var vanchuyenshipper = _vanChuyenService.Layvanchuyenshipper();
            return vanchuyenshipper;
        }
		[HttpPut("CapNhatVanChuyen")]
		public IActionResult CapNhatVanChuyen([FromBody] VanChuyenDTOs vanchuyen)
		{
			if (vanchuyen == null)
			{
				return BadRequest(new { message = "Dữ liệu không hợp lệ" });
			}

			var result = _vanChuyenService.capnhapvanchuyen(vanchuyen);
			if (result != null)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(new { message = "Cập nhật không thành công" });
			}
		}
		[HttpGet("LocTrangThai")]
		public IActionResult LocTrangThai(string trangthai)
		{
			var ketQua = _vanChuyenService.LayVanChuyenTheoTrangThai(trangthai);
			return Ok(ketQua);
		}
		[HttpPut("CapNhatTrangThaiDonHang")]
		public IActionResult CapNhatTrangThaiDonHang(int madomhang)
		{
			if (madomhang == null)
			{
				return BadRequest(new { message = "Dữ liệu không hợp lệ" });
			}

			var result = _vanChuyenService.CapNhatTrangThaiDonHang(madomhang);
			if (result != null)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(new { message = "Cập nhật không thành công" });
			}
		}
	}
}
