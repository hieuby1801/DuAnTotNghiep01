using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GioHangsController : ControllerBase
	{
		private readonly ISachservice _sachservice;
		public GioHangsController(ISachservice sachservice)
		{
			_sachservice = sachservice;
		}
		[HttpPost("ThemGioHang")]
		public async Task<IActionResult> ThemGioHang(int masach, int id, int soluong)
		{
			var giohangck =  _sachservice.KiemTra(masach);
			if(giohangck != null)
			{
				int soluongmoi = giohangck.SoLuong + soluong;
				var update = await _sachservice.CapNhatGioHang(masach, id, soluongmoi);

				return Ok("Cập nhật giỏ hàng thành công.");
			}
			else
			{
				var result = await _sachservice.ThemgiohangDN(masach, id, soluong);
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
			var giohang = _sachservice.Laygiohnagtheoid(id);
			if (giohang != null)
			{
				return Ok(giohang);
			}
			return BadRequest(new { Message = "Không có giỏ hàng nào" });
		}
	}
}
