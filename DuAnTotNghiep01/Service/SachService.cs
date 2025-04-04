using DATN_API.Models;

namespace DATN_API.Service
{
    public class SachService : ISachService
    {
        private readonly MyDbContext _Context;
        public SachService(MyDbContext context)
        {
            _Context = context;
        }
        public List<Sach> Laysach()
        {
            return _Context.Sach.Where(s => s.TrangThai == "on").ToList();
        }
        public List<NhaCungCap> Tencungcap(int macungcap)
        {
            return _Context.NhaCungCap
                           .Where(s => s.MaNhaCungCap == macungcap)
                           .ToList();
        }
    }
}
