
using DATN_API.Models;
namespace DATN_API.Service
{
	public interface ISachservice 
	{
		public List<TheLoai> GetAll();
        public SachDTO GetSach(int maSach);
        public List<SachDTO> Laytontinsach();

    }
}
