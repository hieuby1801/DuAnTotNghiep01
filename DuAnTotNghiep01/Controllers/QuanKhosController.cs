using DATN_API.DTOs;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class QuanKhosController : ControllerBase
	{
		private readonly Iquankhoservice _iquankhoservice;
		public QuanKhosController(Iquankhoservice iquankhoservice)
		{
			_iquankhoservice = iquankhoservice;
		}

		[HttpPost("Duyetvanchuyen1")]
		public IActionResult Duyetvanchuyen([FromBody] DuyetVanChuyenRequest request)
		{

            if(request != null)
			{
                var result = _iquankhoservice.Duyetvanchuyen(request);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Không tìm thấy thông tin vận chuyển.");
                }
            }
            return NotFound("Không tìm thấy thông tin vận chuyển.1");

        }
	}
}
