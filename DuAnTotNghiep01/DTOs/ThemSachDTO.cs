using System.Text.Json.Serialization;

namespace DATN_API.DTOs
{
   public class ThemSachDto
    {
        [JsonPropertyName("maSach")]
        public int MaSach { get; set; }

        [JsonPropertyName("tenSach")]
        public string TenSach { get; set; }

        [JsonPropertyName("tacGia")]
        public string TacGia { get; set; }

        [JsonPropertyName("hinhAnh")]
        public string HinhAnh { get; set; }

        [JsonPropertyName("ngonNgu")]
        public string NgonNgu { get; set; }

        [JsonPropertyName("kichThuoc")]
        public string KichThuoc { get; set; }

        [JsonPropertyName("trongLuong")]
        public decimal TrongLuong { get; set; }

        [JsonPropertyName("soTrang")]
        public int SoTrang { get; set; }

        [JsonPropertyName("hinhThuc")]
        public string HinhThuc { get; set; }

        [JsonPropertyName("moTa")]
        public string MoTa { get; set; }

        [JsonPropertyName("danhSachTheLoai")]
        public string DanhSachTheLoai { get; set; }
    }

}
