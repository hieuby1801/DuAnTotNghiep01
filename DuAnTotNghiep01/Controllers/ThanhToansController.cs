using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ThanhToansController : ControllerBase
	{
		private readonly IThanhToanService _thanhToanService;
		public ThanhToansController(IThanhToanService thanhToanService) 
		{
			_thanhToanService = thanhToanService;
		}
        [HttpGet("LayGioHang")]
        public IActionResult LayTTGioHang([FromQuery] string maGioHang, [FromQuery] int maNguoiDung)
        {
            if (string.IsNullOrEmpty(maGioHang))
                return BadRequest("Thiếu mã giỏ hàng.");

            var listMa = maGioHang.Split(',').Select(int.Parse).ToList();

            var gioHang = _thanhToanService.LayThongTinGioHang(listMa, maNguoiDung);

            if (gioHang == null || gioHang.Count == 0)
                return NotFound("Không tìm thấy giỏ hàng.");

            return Ok(gioHang);
        }
    }
}
