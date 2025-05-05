using DATN_API.DTOs;
using DATN_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThongKeController : ControllerBase
    {
        private readonly IThongKeService _thongKeService;

        public ThongKeController(IThongKeService thongKeService)
        {
            _thongKeService = thongKeService;
        }

        [HttpGet("DoanhThuTheoNgay")]
        public ActionResult<List<ThongKeDoanhThuNgayDTO>> GetDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                var result = _thongKeService.ThongKeDoanhThuTheoNgay(tuNgay, denNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }
    }
}
