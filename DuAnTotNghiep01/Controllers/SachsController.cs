using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SachsController : ControllerBase
	{
		private readonly ISachservice _sachservice;
		public SachsController (ISachservice sachservice)
		{
			_sachservice = sachservice;
		}
		[HttpGet]
		public IActionResult Get()
		{
			var theloai= _sachservice.GetAll();
			if(theloai != null)
			{
				return Ok(theloai); 
			}
			return BadRequest(new {Mesgage  = "Không có thể  loại nào"});
		}

	}
}
