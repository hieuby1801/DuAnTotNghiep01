using Microsoft.AspNetCore.Mvc;

namespace DATN_MVC.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Admin()
		{
			return View();
		}
	}
}
