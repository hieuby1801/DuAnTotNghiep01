using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly IDonHangSeverce _donHangSeverce;
        public DonHangController (DonHangService donHangSeverce)
        {
            _donHangSeverce = donHangSeverce;
        }
        [HttpGet]
        public List<DonHang> GetDonHangs()
        {
            var donHangs = _donHangSeverce.GetDonHangs();
            return donHangs;
        }
    }
}