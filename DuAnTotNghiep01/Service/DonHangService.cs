using DATN_API.Models;

namespace DATN_API.Service
{
    public class DonHangService : IDonHangSeverce
    {
        private readonly MyDbContext _Context;
        public DonHangService (MyDbContext context)
        {
            _Context = context;
        }
        public List<DonHang> GetDonHangs()
        {
             return _Context.DonHang.ToList();
        }



    }
}
