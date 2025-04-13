namespace DATN_API.Models
{
    public class NhaCungCap
    {
        public int MaNhaCungCap { get; set; }

        public string? TenNhaCungCap { get; set; }

        public string? DiaChi { get; set; }

        public string? SoDienThoai { get; set; }

        public string? Email { get; set; }

        public string TrangThai { get; set; } = null!;

        public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
    }
}
