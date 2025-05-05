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
        [HttpGet("DoanhThuTheoThang")]
        public ActionResult<List<ThongKeDoanhThuNgayDTO>> GetDoanhThuTheoThang(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                var result = _thongKeService.ThongKeDoanhThuTheoThang(tuNgay, denNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        [HttpGet("DoanhThuTheoNam")]
        public ActionResult<List<ThongKeDoanhThuNgayDTO>> GetDoanhThuTheoNam(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                var result = _thongKeService.ThongKeDoanhThuTheoNam(tuNgay, denNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }
        [HttpGet("DoanhThuTheoSach")]
        public ActionResult<List<ThongKeDoanhThuSachDTO>> GetDoanhThuTheoSach(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                var result = _thongKeService.ThongKeDoanhThuTheoSach(tuNgay, denNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        [HttpGet("DoanhThuTheoTheLoai")]
        public ActionResult<List<ThongKeDoanhThuTheLoaiDTO>> GetDoanhThuTheoTheLoai(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                var result = _thongKeService.ThongKeDoanhThuTheoTheLoai(tuNgay, denNgay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

    }
}
