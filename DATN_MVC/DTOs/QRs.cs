using System.Text.Json;

namespace DATN_MVC.DTOs
{
    public class PayUrlInfo
    {
        public string PayUrl { get; set; }
        public string QrImageBase64 { get; set; }
    }

    public class QRs
    {
        public PayUrlInfo PayUrl { get; set; }
    }
}
