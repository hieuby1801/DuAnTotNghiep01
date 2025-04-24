namespace DATN_API.DTOs
{
    public class MomoQrResponse
    {
        public string PayUrl { get; set; }
        public string QrImageBase64 { get; set; }  // chứa dạng "data:image/png;base64,..."
    }
}
