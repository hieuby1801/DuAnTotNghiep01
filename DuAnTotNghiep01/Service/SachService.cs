using DATN_API.Models;

namespace DATN_API.Service
{
	public class SachService : ISachservice
	{
		private readonly MyDbContext _context;
		public SachService(MyDbContext context)
		{
			_context = context;
		}
		public List<TheLoai> GetAll()
		{
			return _context.TheLoai.ToList(); 
		}

	}
}
