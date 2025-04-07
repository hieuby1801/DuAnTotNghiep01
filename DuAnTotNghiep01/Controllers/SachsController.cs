using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SachsController : ControllerBase
    {
        private readonly ISachService _IsachService;
        public SachsController(ISachService sachService)
        {
            _IsachService = sachService;
        }
        [HttpGet("Laysach")]
        public IActionResult Laysach()
        {
            var sach = _IsachService.Laysach();
            if (sach != null)
            {
                return Ok(sach);
            }
            else
            {
                return NotFound(new { Message = "Không tìm thấy sách nào." });
            }
        }
        [HttpGet("Laytencungcap/{id}")]
        public IActionResult Layten(int id)
        {
            var tencungcap = _IsachService.Tencungcap(id);
            if (tencungcap != null)
            {
                return Ok(tencungcap);
            }
            else
            {
                return NotFound(new { Message = "Không tìm thấy nhà cung cấp nào." });
            }
        }
        [HttpGet("LayTatCaTheLoai")]
        public IActionResult LayTatCaTheLoai()
        {
            var tentheloai = _IsachService.LayTatCaTheLoai();
            if (tentheloai != null)
            {
                return Ok(tentheloai);
            }
            else
            {
                return NotFound(new { Message = "Không tìm thấy thể loại nào." });
            }
        }
		[HttpGet("Laysachtheotheloai")]
		public IActionResult Laysachtheloai([FromQuery] string tentheloai)
		{
			var sach = _IsachService.Laysachtheotheloai(tentheloai);
			if (sach != null && sach.Any())
				return Ok(sach);
			else
				return NotFound(new { Message = "Không tìm thấy sách nào theo thể loại này." });
		}
		[HttpGet("Laysachtu2theloaitrolen")]
		public IActionResult Laysachtu2theloaitrolen([FromQuery] List<string> dstheloai)
		{
			var sach = _IsachService.Laysachtu2theloaitrolen(dstheloai);
			if (sach != null && sach.Any())
				return Ok(sach);
			else
				return NotFound(new { Message = "Không tìm thấy sách nào " });
		}
		[HttpGet("Laysachtheo1trong2theloai")]
        public IActionResult Laysachtheo1trong2theloai([FromQuery] List<string> dstheloai)
		{
			var sach = _IsachService.Laysachtheo1trong2theloai(dstheloai);
			if (sach != null && sach.Any())
				return Ok(sach);
			else
				return NotFound(new { Message = "Không tìm thấy sách nào " });
		}
	}
}
