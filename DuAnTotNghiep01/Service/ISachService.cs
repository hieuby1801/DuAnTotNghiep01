using DATN_API.Models;

namespace DATN_API.Service
{
    public interface ISachService
    {
       public List<Sach> Laysach();
        public List<NhaCungCap> Tencungcap(int macungcap);
    }
}
