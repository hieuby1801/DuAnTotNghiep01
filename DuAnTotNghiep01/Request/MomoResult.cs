using DATN_API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace DATN_API.Request
{
    public class MomoResult
    {
        public string PartnerCode { get; set; }
		[FromQuery(Name = "orderId")]   // tên tham số MoMo gửi về
		public int MaDonHang { get; set; }
        public string RequestId { get; set; }
        public long Amount { get; set; }
        public string OrderInfo { get; set; }
        public string OrderType { get; set; }
        public string TransId { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public string PayType { get; set; }
        public long ResponseTime { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }
		public string SDT { get; set; }
		public string DiaChi { get; set; }
		public DateTime NgayNhanHang { get; set; } = DateTime.Now;


	}
}
