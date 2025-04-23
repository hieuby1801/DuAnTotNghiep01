using DATN_API.DTOs;
using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ThanhToansController : ControllerBase
	{
		private readonly IThanhToanService _thanhToanService;
		public ThanhToansController(IThanhToanService thanhToanService) 
		{
			_thanhToanService = thanhToanService;
		}
        [HttpGet("LayGioHang")]
        public IActionResult LayTTGioHang([FromQuery] string maGioHang, [FromQuery] int maNguoiDung)
        {
            if (string.IsNullOrEmpty(maGioHang))
                return BadRequest("Thiếu mã giỏ hàng.");

            var listMa = maGioHang.Split(',').Select(int.Parse).ToList();

            var gioHang = _thanhToanService.LayThongTinGioHang(listMa, maNguoiDung);

            if (gioHang == null || gioHang.Count == 0)
                return NotFound("Không tìm thấy giỏ hàng.");

            return Ok(gioHang);
        }
		// Khi khác hàng chọn phương thức là thanh toán chuyển khoản 
		[HttpPost("ThemdonhangQR")]
		public IActionResult ChuyenKhoan([FromBody] DonHangQRRequest request)
		{
			if (request.masach.Count != request.soluong.Count)
				return BadRequest("Số lượng sách và số lượng không khớp.");

			// Tạo đơn hàng mới
			var maDonHangMoi = _thanhToanService.Themdonhang(request.manguoidung);
			if (maDonHangMoi == null)
			{
				return BadRequest("Lỗi tạo đơn hàng mới.");
			}

			// Tạo chi tiết đơn hàng (danh sách)
			var chiTiet = new DonHangChiTietDTOs
			{
				MaDonHang = maDonHangMoi,
				MaSach = request.masach,
				SoLuong = request.soluong
			};

			var ketQuaChiTiet = _thanhToanService.ThemChiTietDonHang(chiTiet);
			if (ketQuaChiTiet == null)
			{
				return BadRequest("Lỗi tạo chi tiết đơn hàng.");
			}

			return Ok(ketQuaChiTiet);
		}
		[HttpPost("Themdonhangtienmat")]

		public IActionResult Tienmat([FromBody] DonHangQRRequest request)
		{
			if (request.masach.Count != request.soluong.Count)
				return BadRequest("Số lượng sách và số lượng không khớp.");

			// Tạo đơn hàng mới
			var maDonHangMoi = _thanhToanService.Themdonhang(request.manguoidung);
			if (maDonHangMoi == null)
			{
				return BadRequest("Lỗi tạo đơn hàng mới.");
			}

			// Tạo chi tiết đơn hàng (danh sách)
			var chiTiet = new DonHangChiTietDTOs
			{
				MaDonHang = maDonHangMoi,
				MaSach = request.masach,
				SoLuong = request.soluong
			};

			var ketQuaChiTiet = _thanhToanService.ThemChiTietDonHang(chiTiet);
			if (ketQuaChiTiet == null)
			{
				return BadRequest("Lỗi tạo chi tiết đơn hàng.");
			}
			var vanchuye = new VanChuyenDTOs
			{
				MaDonHang = maDonHangMoi,
				SDT = request.SDT,
				DiaChi = request.DiaChi,
				NgayNhanHang = request.NgayNhanHang
			};
			var ketQuaVanChuyen = _thanhToanService.ThemVaoVanChuyen(vanchuye);
			if (ketQuaVanChuyen == null)
			{
				return BadRequest("Lỗi tạo vận chuyển đơn hàng.");
			}
			return Ok(ketQuaVanChuyen);
		}

	}
}
