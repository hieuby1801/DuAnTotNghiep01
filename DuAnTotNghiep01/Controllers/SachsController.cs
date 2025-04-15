using Azure.Messaging;
using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SachsController : ControllerBase
	{
		private readonly ISachservice _sachservice;
		public SachsController(ISachservice sachservice)
		{
			_sachservice = sachservice;
		}
		[HttpGet("Laytatcatheloai")]
		public IActionResult Get()
		{
			var theloai = _sachservice.GetAll();
			if (theloai != null)
			{
				return Ok(theloai);
			}
			return BadRequest(new { Mesgage = "Không có thể  loại nào" });
		}
		[HttpGet("Laysachtheoid/{masach}")]
		public IActionResult GetSach(int masach)
		{
			var sach = _sachservice.GetSach(masach);
			if (sach != null)
			{
				return Ok(sach);
			}
			return BadRequest(new { Message = "Không có sách nào" });
		}
		[HttpGet("Laysach")]
		public IActionResult Laysach()
		{
			var sach = _sachservice.Laytontinsach();
			if (sach != null)
			{
				return Ok(sach);
			}
			return BadRequest(new { Message = "Không có sách nào" });
		}
		[HttpGet("Laysachtheotentheloai")]
		public IActionResult Timsach(string theloai)
		{
			var sach = _sachservice.Timsachtheotheloai(theloai);
			if (sach != null)
			{
				return Ok(sach);
			}
			return BadRequest(new { Message = "Không có sách nào" });
		}
		[HttpGet("Laysachtheothongtinnhap")]

		[HttpGet]
		public ActionResult<List<SachDTO>> Timsachtheothongtinnhap(
			[FromQuery] string? tenSach = null,
			[FromQuery] int? khoangGia = null,
			[FromQuery] string? doTuoi = null,
			[FromQuery] string? tacGia = null,
			[FromQuery] List<string>? theLoai = null)
		{
			var sach = _sachservice.Timsachtheothongtinnhap(tenSach, khoangGia, doTuoi, tacGia, theLoai);

			if (sach != null && sach.Any())
			{
				return Ok(sach);
			}

			return BadRequest(new { Message = "Không có sách nào" });
		}
		[HttpGet("ThemGioHang/{masach}")]
		public IActionResult Themhangvaogio(int masach)
		{
			var sacht = _sachservice.Themgiohang(masach);
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
