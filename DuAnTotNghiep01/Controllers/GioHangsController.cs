using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GioHangsController : ControllerBase
	{
		private readonly IGioHnagservice _gioHnagservice;
        public GioHangsController(IGioHnagservice gioHnagservice)
		{
            _gioHnagservice = gioHnagservice;
		}
		[HttpPost("ThemGioHang")]
		public async Task<IActionResult> ThemGioHang(int masach, int id, int soluong)
		{
			var giohangck = _gioHnagservice.KiemTra(masach,id);
			if (giohangck != null)
			{
				// Cập nhật số lượng trong giỏ hàng
				
				var update = await _gioHnagservice.CapNhatGioHang(masach, id, soluong);
				return Ok("Cập nhật giỏ hàng thành công.");
			}
			else
			{
				// Nếu giỏ hàng không có sản phẩm, thêm mới
				var result = await _gioHnagservice.ThemgiohangDN(masach, id, soluong);
				if (!result)
				{
					return BadRequest("Thêm giỏ hàng thất bại.");
				}
				return Ok("Thêm giỏ hàng thành công.");
			}
		}
		[HttpGet("LayGioHang/{id}")]
		public IActionResult LayGioHang(int id)
		{
			var giohang = _gioHnagservice.Laygiohnagtheoid(id);
			if (giohang != null)
			{
				return Ok(giohang);
			}
			return BadRequest(new { Message = "Không có giỏ hàng nào" });
		}
        [HttpGet("ThemGioHangck/{masach}")]
        public IActionResult Themhangvaogio(int masach)
        {
            var sacht = _gioHnagservice.Themgiohang(masach);
            if (masach != null)
            {
                return Ok(sacht);
            }
            else
            {
                return BadRequest(new { Message = "Khong co sach" });
            }
        }
    }
}
