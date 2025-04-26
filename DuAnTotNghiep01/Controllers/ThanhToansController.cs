using DATN_API.DTOs;
using DATN_API.Models;
using DATN_API.Request;
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
		private readonly IPaymentService _paymentService;
		private readonly IDangNhapService _dangNhapService;
		public ThanhToansController(IThanhToanService thanhToanService, IPaymentService paymentService, IDangNhapService dangNhapService) 
		{
			_thanhToanService = thanhToanService;
			_paymentService = paymentService;
			_dangNhapService = dangNhapService;
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
		[HttpPost("Themdonhangtienmat")]
		public IActionResult Tienmat([FromBody] DonHangQRRequest request )
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
		[HttpPost("create")]
		public async Task<IActionResult> CreatePaymentLink([FromBody] MomoRequest request)
		{
			var payUrl = await _paymentService.CreateMomoPaymentUrl(request);
			return Ok(new { PayUrl = payUrl });
		}

		// Tạo đơn hàng và chi tiết đơn hàng, sau đó tạo liên kết thanh toán
		[HttpPost("ThemdonhangQR")]
		public async Task<IActionResult> ChuyenKhoan([FromBody] DonHangQRRequest request)
		{
			if (request.masach.Count != request.soluong.Count)
				return BadRequest("Số lượng sách và số lượng không khớp.");

			// Tạo đơn hàng mới (giả lập quá trình tạo đơn hàng)
			var maDonHangMoi = _thanhToanService.Themdonhang(request.manguoidung);
			if (maDonHangMoi == null)
			{
				return BadRequest("Lỗi tạo đơn hàng mới.");
			}

			// Tạo chi tiết đơn hàng (danh sách sách và số lượng)
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
			var nguoidung = _dangNhapService.LayTheoId(request.manguoidung);
			if (nguoidung == null)
			{
				return BadRequest("Người dùng không tồn tại.");
			}
			// Tạo liên kết thanh toán MoMo (dùng dữ liệu mẫu để trả về URL thanh toán)
			var momoRequest = new MomoRequest
			{
				OrderId = maDonHangMoi,  // Sử dụng mã đơn hàng mới
				OrderInfo = nguoidung.TenNguoiDung+ "chuyển tiền" + nguoidung.SoDienThoai + request.DiaChi,  // Thông tin đơn hàng
				Amount = ketQuaChiTiet.GiaTien  // Giá trị thanh toán (ví dụ 10.000 VND cho demo)
			};

			// Gọi phương thức tạo URL thanh toán từ dịch vụ MoMo
			var payUrl = await _paymentService.CreateMomoPaymentUrl(momoRequest);

			// Trả về URL thanh toán cho người dùng
			return Ok(new { PayUrl = payUrl });
		}

		// Nhận kết quả từ MoMo và cập nhật trạng thái đơn hàng 
		[HttpGet("momo-return")]
		public IActionResult MomoReturn([FromQuery] MomoResult result)
		{
			if (result.ResultCode == 0)  // Kiểm tra nếu thanh toán thành công
			{
				// Cập nhật trạng thái đơn hàng là "Đã thanh toán" (giả lập)
				var updateStatusResult = _thanhToanService.CapNhatTrangThaiDonHang(result.MaDonHang, "Đã thanh toán");
				if (!updateStatusResult)
				{
					return BadRequest("Cập nhật trạng thái đơn hàng thất bại.");
				}

				// Tạo thông tin vận chuyển cho đơn hàng (giả lập)
				var vanChuyen = new VanChuyenDTOs
				{
					MaDonHang = result.MaDonHang,
					SDT = result.SDT,  // Lấy số điện thoại từ kết quả Momo
					DiaChi = result.DiaChi,  // Lấy địa chỉ từ kết quả Momo
					NgayNhanHang = result.NgayNhanHang  // Lấy ngày nhận hàng từ kết quả Momo
				};

				var ketQuaVanChuyen = _thanhToanService.ThemVaoVanChuyen(vanChuyen);
				if (ketQuaVanChuyen == null)
				{
					return BadRequest("Lỗi tạo vận chuyển đơn hàng.");
				}

				return Ok("Thanh toán thành công và đơn hàng đã được xử lý.");
			}
			else
			{
				return BadRequest("Thanh toán thất bại.");
			}
		}


	}
}
