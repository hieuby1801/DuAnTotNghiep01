using DATN_API.Models;
using Microsoft.EntityFrameworkCore;

namespace DATN_API.Service
{
    public class VanChuyenService : IVanChuyenService
    {
        private readonly MyDbContext _Context;
        public VanChuyenService(MyDbContext context)
        {
            _Context = context;
        }
        public List<VanChuyen> GetVanChuyenss()
        {
            return _Context.VanChuyen.ToList();
        }
    }
     
}
