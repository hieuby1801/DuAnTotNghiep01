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
    }
}
