using Microsoft.AspNetCore.Mvc;

namespace DATN_MVC.Controllers
{
	public class QuanLyNhapHangController : Controller
	{
		public IActionResult NhapLoHangMoi()
		{
			return View();
		}
		public IActionResult DanhSachNhapHang()
		{
			return View();
		}
	}
}
