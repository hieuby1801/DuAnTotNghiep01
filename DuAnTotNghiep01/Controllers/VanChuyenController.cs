using DATN_API.Models;
using DATN_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VanChuyenController : ControllerBase
    {
        private readonly IVanChuyenService _vanChuyenService;
        public VanChuyenController(IVanChuyenService vanChuyenService)
        {
            _vanChuyenService = vanChuyenService;
        }
        [HttpGet("GetVanChuyen")]
        public List<VanChuyen> GetVanChuyenss()
        {
            var vanchuyen = _vanChuyenService.GetVanChuyenss();
            return vanchuyen;
        }
    }
}
