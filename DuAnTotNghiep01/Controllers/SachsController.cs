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
		[HttpGet("Laysachtheoid")]
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
			if(sach != null)
			{
				return Ok(sach);
			}
            return BadRequest(new { Message = "Không có sách nào" });
        }


    }
}
