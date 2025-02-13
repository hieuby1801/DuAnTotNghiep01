namespace DATN_MVC.Models
{
    public class PhuongThucThanhToan
    {
        public int MaPhuongThuc { get; set; }

        public string? TenPhuongThuc { get; set; }

        public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    }
}
