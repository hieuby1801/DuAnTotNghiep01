﻿namespace DATN_API.Response
{
    public class MomoPaymentCreateLinkResponse
    {
        public string partnerCode { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public long amount { get; set; }
        public long responseTime { get; set; }
        public string message { get; set; } = string.Empty;
        public string resultCode { get; set; } 
        public string payUrl { get; set; } = string.Empty;
        public string deeplink { get; set; } = string.Empty;
        public string qrCodeUrl { get; set; } = string.Empty;
    }
}
