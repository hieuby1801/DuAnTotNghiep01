namespace DATN_MVC.Models
{
    public class TheLoai
    {
        public int MaTheLoai { get; set; }

        public string? TenTheLoai { get; set; }

        public virtual ICollection<Sach> MaSaches { get; set; } = new List<Sach>();
    }
}
