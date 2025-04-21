using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DATN_API.DTOs
{
    public class ThemSachDto
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string HinhAnh { get; set; }
        public string TrangThai { get; set; }
        public string NgonNgu { get; set; }
        public string KichThuoc { get; set; }
        public decimal TrongLuong { get; set; }
        public int? SoTrang { get; set; }
        public string HinhThuc { get; set; }
        public string MoTa { get; set; }
        public string DoTuoi { get; set; }
        public string ListTheLoai { get; set; }
    }


}
