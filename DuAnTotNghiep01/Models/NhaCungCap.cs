namespace DATN_API.Models
{
    public class NhaCungCap
    {
<<<<<<< HEAD

		public int MaNhaCungCap { get; set; }
		public string? TenNhaCungCap { get; set; }
		public string? SDT { get; set; }
		public string? Email { get; set; }
		public string? TrangThai { get; set; }
	}

=======
        public int MaNhaCungCap { get; set; }

        public string? TenNhaCungCap { get; set; }

        public string? DiaChi { get; set; }

        public string? SoDienThoai { get; set; }

        public string? Email { get; set; }

        public string TrangThai { get; set; } = null!;

        public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
    }
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
}
