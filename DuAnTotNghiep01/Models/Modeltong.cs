namespace DATN_API.Models
{
    public class Modeltong
    {
<<<<<<< HEAD

=======
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
        public string? Email { get; set; }
        public string? Saft { get; set; }
        public string MatKhau { get; set; }
        public NguoiDung NguoiDungss { get; set; }
        public Sach Sachs { get; set; }
<<<<<<< HEAD
     
        public List<TheLoai> TheLoais { get; set; } = new List<TheLoai>();
	
=======
        public SachDTO sachDTOss { get; set; }
        public List<TheLoai> TheLoais { get; set; } = new List<TheLoai>();
		public List<SachDTO> sachDTOs { get; set; } = new List<SachDTO>();
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
        public List<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
        public List<Sach> Saches { get; set; } = new List<Sach>();
    
        public List<NhaCungCap> nhaCungCaps { get; set; } = new List<NhaCungCap>();
<<<<<<< HEAD

=======
>>>>>>> f61a8d38d73929f12c078539cd2e10562b25c593
    }
}
