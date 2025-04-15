using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_MVC.Controllers
{
    public class ThanhToanController : Controller
    {
        public IActionResult ThanhToan()
        {
			var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
			if (userId == null)
			{
				return RedirectToAction("DangNhap", "DangNhap"); // hoặc xử lý theo ý bạn
			}

			// Dùng userId để lấy giỏ hàng của người dùng
			

			
			return View();
        }
    }
}
