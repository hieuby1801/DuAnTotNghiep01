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
        [HttpGet("Laynhacungcap")]
        public IActionResult Laynhacungcap()
        {
            var nhacungcap = _IsachService.Laynhacung();
            if (nhacungcap != null)
            {
                return Ok(nhacungcap);
            }
            else
            {
                return NotFound(new { Message = "Không tìm thấy nhà cung cấp nào." });
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
        [HttpPost("ThemSach")]
        public IActionResult ThemSach([FromBody] SachDTO TTSach)
        {
            if (ModelState.IsValid)
            {
                var sachcheck = _IsachService.Laysach().FirstOrDefault(s => s.TenSach == TTSach.TenSach || s.MaSach == TTSach.MaSach);
                if (sachcheck != null)
                {
                    return BadRequest(new { Message = "Sách đã tồn tại." });
                }
                var sach = _IsachService.ThemSach(TTSach);
                if (sach != null)
                {
                    return Ok(new { Message = "Thêm sách thành công." });
                }
                else
                {
                    return BadRequest(new { Message = "Thêm sách không thành công." });
                }
            }
            return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
        }

        // cap nhap sach
        [HttpGet("Timsach/{masach}")]
        public IActionResult Timsach(int masach)
        {
            var sach = _IsachService.Timsach(masach);
            if (sach != null)
            {
                return Ok(sach);
            }
            else
            {
                return NotFound(new { Message = "Không tìm thấy sách nào." });
            }
        }
        [HttpPut("CapNhatSach")]
        public IActionResult CapNhatSach([FromBody] SachDTO sach)
        {
            if (ModelState.IsValid)
            {
                var timsach = _IsachService.Timsach(sach.MaSach);
                if (timsach != null)
                {
                    var updatedSach = _IsachService.CapNhatSach(sach);
                    if (updatedSach != null)
                    {
                        return Ok(new { Message = "Cập nhật sách thành công." });
                    }
                    else
                    {
                        return BadRequest(new { Message = "Cập nhật sách không thành công." });
                    }
                }
                else
                {
                    return NotFound(new { Message = "Không tìm thấy sách nào." });
                }

            }
            return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
        }
    }
}
