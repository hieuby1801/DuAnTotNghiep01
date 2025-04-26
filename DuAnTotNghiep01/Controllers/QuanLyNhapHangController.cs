using DATN_API.DTOs;
using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyNhapHangController : ControllerBase
    {
        private readonly IQuanLyNhapHangService _nhapHangService;
        private readonly INhaCungCapService _nhaCungCapService;
        public QuanLyNhapHangController(IQuanLyNhapHangService nhapHangService, INhaCungCapService nhaCungCapService)
        {
            _nhapHangService = nhapHangService;
            _nhaCungCapService = nhaCungCapService;
        }
        [HttpGet("DSLoHang")]
        public IActionResult DSLoHang()
        {
            var dsLoHang = _nhapHangService.getAllLoHang();
            if (dsLoHang != null)
            {
                return Ok(dsLoHang);
            }
            return BadRequest(new { Mesgage = "Không có lô hàng" });
        }

        [HttpGet("DSNhaCungCap")]
        public IActionResult DSNhaCungCap()
        {
            var dsNCC = _nhaCungCapService.GetAllNhaCungCap();
            if (dsNCC != null)
            {
                return Ok(dsNCC);
            }
            return BadRequest(new { Mesgage = "Không có nhà cung cấp" });
        }

        [HttpPost("InsertLoHang")]
        public async Task<IActionResult> InsertLoHang(LoHangDTO loHang)
        {
            var result = await _nhapHangService.insertLoHang(loHang);
            if (result)
                return Ok(new { message = "Thêm lô hàng thành công" });
            else
                return BadRequest();
        }
    }
}
