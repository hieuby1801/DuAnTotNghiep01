using Azure.Messaging;
using DATN_API.DTOs;
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

<<<<<<< HEAD
        [HttpGet("getOnlySach")]
        public IActionResult GetAll()
        {
            var result = _sachservice.GetOnlySach();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { Message = "Khong co sach" });
            }
=======
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sachservice.GetAllAsync();
            return Ok(result);
>>>>>>> 51d004671a7640d6a441416d66b1d59435708bc8
        }

        [HttpPost("them")]
        public async Task<IActionResult> ThemSach([FromBody] ThemSachDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // kiểm tra model hợp lệ
            }

            var ketQua = await _sachservice.ThemSachAsync(dto);
            if (ketQua)
            {
                return Ok(new { message = "Thêm sách thành công" });
            }

            return BadRequest(new { message = "Thêm sách thất bại" });
        }
    }
}
