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
        [HttpGet("LayTTGioHang")]
        public List<GioHangDTO> LayThongTinGioHang([FromQuery] List<int> maGioHang, [FromQuery] int maNguoiDung)
        {
            var giohangList = _thanhToanService.LayThongTinGioHang(maGioHang, maNguoiDung);

            if (giohangList != null && giohangList.Any())
            {
                foreach (var item in giohangList)
                {
                    item.MaNguoiDung = maNguoiDung;
                }
                return giohangList.ToList();
            }

            return null;
        }

    }
}
