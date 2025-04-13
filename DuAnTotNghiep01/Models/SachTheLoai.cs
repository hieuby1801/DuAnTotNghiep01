namespace DATN_API.Models
{
    public class SachTheLoai
    {
        public int id { get; set; }
        public int MaSach { get; set; }
        public int MaTheLoai { get; set; }

        // Navigation properties
        public Sach? Sach { get; set; }
        public TheLoai? TheLoai { get; set; }
    }
}
