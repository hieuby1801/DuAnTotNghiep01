﻿using DATN_API.DTOs;
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

		[HttpPost("InsertChiTietLoHangList")]
        public async Task<IActionResult> InsertChiTietLoHangList([FromBody] List<ChiTietLoHangDTO> listDto)
        {
            // Kiểm tra xem danh sách chi tiết lô hàng có trống hay không
            if (listDto == null || listDto.Count == 0)
            {
                return BadRequest(new { message = "Danh sách chi tiết lô hàng trống" });
            }

            bool allSuccess = true;
            List<string> errorMessages = new List<string>(); // Danh sách lưu các lỗi nếu có

            foreach (var dto in listDto)
            {
                // Thực hiện insert chi tiết lô hàng
                var success1 = await _nhapHangService.insertChiTietLoHang(dto);

                // Tạo DTO cho lịch sử giá
                var lichSuGiaDto = new LichSuGiaDTO()
                {
                    MaSach = dto.MaSach,
                    MaLo = dto.MaLo,
                    GiaNhap = dto.GiaNhap
                };

                // Thực hiện insert lịch sử giá
                var success2 = await _nhapHangService.insertLichSuGia(lichSuGiaDto);

                // Kiểm tra nếu một trong các thao tác insert thất bại
                if (!success1)
                {
                    allSuccess = false;
                    errorMessages.Add($"Insert chi tiết lô hàng thất bại cho mã sách {dto.MaSach}, mã lô {dto.MaLo}");
                }

                if (!success2)
                {
                    allSuccess = false;
                    errorMessages.Add($"Insert lịch sử giá thất bại cho mã sách {dto.MaSach}, mã lô {dto.MaLo}");
                }

                // Nếu có lỗi xảy ra, dừng vòng lặp và trả về lỗi
                if (!allSuccess)
                {
                    break;
                }
            }

            // Nếu tất cả các thao tác thành công
            if (allSuccess)
            {
                return Ok(new { message = "Thêm danh sách chi tiết lô hàng thành công" });
            }
            else
            {
                // Trả về danh sách lỗi nếu có
                return BadRequest(new { message = "Có lỗi xảy ra trong quá trình thêm chi tiết lô hàng", errors = errorMessages });
            }
        }
        [HttpGet("{maLo}")]
        public IActionResult GetLoHang(int maLo)
        {
            var loHang = _nhapHangService.getLoHang(maLo);
            if (loHang == null)
            {
                return NotFound(new { message = "Lô hàng không tồn tại" });
            }
            return Ok(loHang);
        }
        [HttpGet("chi-tiet/{maLo}")]
        public IActionResult GetChiTietLoHang(int maLo)
        {
            var chiTietLoHangs = _nhapHangService.getChiTietLoHang(maLo);
            if (chiTietLoHangs == null || !chiTietLoHangs.Any())
            {
                return NotFound(new { message = "Không tìm thấy chi tiết lô hàng cho mã lô này." });
            }

            return Ok(chiTietLoHangs);
        }
    }
}
