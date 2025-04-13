namespace DATN_API.Models
{
    public class SachTheLoai
    {
<<<<<<< HEAD

		public int MaSach { get; set; }
		public int? MaTheLoai { get; set; }
	}

  
=======
        public int id { get; set; }
        public int MaSach { get; set; }
        public int MaTheLoai { get; set; }

        // Navigation properties
        public Sach? Sach { get; set; }
        public TheLoai? TheLoai { get; set; }
    }
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
}
