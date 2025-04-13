namespace DATN_API.Models
{
    public class TheLoai
    {
<<<<<<< HEAD

		public int MaTheLoai { get; set; }
		public string? TenTheLoai { get; set; }
	}

=======
        public int MaTheLoai { get; set; }

        public string? TenTheLoai { get; set; }

        public ICollection<SachTheLoai> SachTheLoais { get; set; }
    }
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
}
