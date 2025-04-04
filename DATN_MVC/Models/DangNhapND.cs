namespace DATN_MVC.Models
{
    public class DangNhapND
    {
        public string? Email { get; set; }
        public string? Saft { get; set; }
        public string MatKhau { get; set; }
        public NguoiDung NguoiDungss { get; set; }
        public List<Sach> Saches { get; set; } = new List<Sach>();
        public List<NhaCungCap> nhaCungCaps { get; set; } = new List<NhaCungCap>();
    }
}
