using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_MVC.Controllers
{
    public class ThanhToanController : Controller
    {
        public IActionResult ThanhToan()
        {
			var token = HttpContext.Session.GetString("NguoiDungId");
			if (string.IsNullOrEmpty(token))
			{
				return RedirectToAction("DangNhap", "DangNhap"); // hoặc xử lý theo ý bạn
			}
			
			return View();
        }
    }
}
