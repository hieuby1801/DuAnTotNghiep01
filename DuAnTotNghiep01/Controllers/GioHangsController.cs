using DATN_API.DTOs;
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
		public async Task<IActionResult> ThemGioHang([FromBody] CapNhatGioHangRequest req)
		{
			var giohangck = _gioHnagservice.KiemTra(req.MaSach, req.MaNguoiDung);
			if (giohangck != null)
			{
				// Cập nhật số lượng trong giỏ hàng
				req.SoLuong  = giohangck.SoLuong + req.SoLuong;
				var update =  _gioHnagservice.CapNhatGioHang(req);
				return Ok("Cập nhật giỏ hàng thành công.");
			}
			else
			{
				// Nếu giỏ hàng không có sản phẩm, thêm mới
				var result = await _gioHnagservice.ThemgiohangDN(req.MaSach, req.MaNguoiDung, req.SoLuong);
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
		[HttpPut("Capnhapgiohang")]
		public IActionResult Capnhapgiohnag([FromBody] CapNhatGioHangRequest req)
		{
			
			var giohangck = _gioHnagservice.KiemTra(req.MaSach, req.MaNguoiDung);
			if (giohangck != null)
			{
				_gioHnagservice.CapNhatGioHang(req);
				return Ok("Cập nhật thành công");
			}
			return BadRequest("Không tìm thấy sản phẩm");
		}
		[HttpDelete("Xoagiohang/{masach}/{idnd}")]
		public IActionResult Xoagiohang(int masach, int idnd)
		{
			var result = _gioHnagservice.XoaGiohangDN(masach, idnd);
			if (result != null)
			{
				return Ok("Xóa thành công");
			}
			else
			{
				return NotFound("Không tìm thấy mục cần xóa");
			}
		}

		[HttpPost("ThemdsGioHang")]
		public IActionResult ThemdsGioHang([FromBody] List<CapNhatGioHangRequest> requests)
		{
			var result = _gioHnagservice.ThemdangsachGiohangck(requests);

			if (result != null && result.Any())
			{
				return Ok(new
				{
					message = "Thêm/cập nhật giỏ hàng thành công",
					data = result
				});
			}
			else
			{
				return BadRequest("Không thể thêm sách vào giỏ hàng (sách không tồn tại hoặc dữ liệu không hợp lệ)");
			}
		}

	}
}
