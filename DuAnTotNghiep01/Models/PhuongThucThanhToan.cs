namespace DATN_API.Models
{
    public class PhuongThucThanhToan
    {
<<<<<<< HEAD
		public int MaPhuongThuc { get; set; }
		public string? TenPhuongThuc { get; set; }

	}

=======
        public int MaPhuongThuc { get; set; }

        public string? TenPhuongThuc { get; set; }

        public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    }
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
}
