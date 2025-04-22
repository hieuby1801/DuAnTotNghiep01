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
        public QuanLyNhapHangController(IQuanLyNhapHangService nhapHangService)
        {
            _nhapHangService = nhapHangService;
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
    }
}
