namespace DATN_API.Config
{
    public class MomoConfig
    {
    
        public static string PartnerCode { get; set; } = "MOMOBKUN20180529";
        public static string ReturnUrl { get; set; } = "https://localhost:7189/api/payment/momo-return";
        public static string IpnUrl{ get; set; } = "https://localhost:7189/api/payment/momo-ipn";
		public static string AccessKey { get; set; } = "klm05TvNBzhg7h7j";
        public static string SecretKey { get; set; } = "at67qH6mk8w5Y1nAyMoYKMWACiEi2bsa";
        public static string PaymentUrl { get; set; } = "https://test-payment.momo.vn/v2/gateway/api/create";
    }
}
