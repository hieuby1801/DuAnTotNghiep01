using DATN_API.Request;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DATN_API.Service
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _config;
		private readonly HttpClient _httpClient;

		public PaymentService(IConfiguration config, IHttpClientFactory httpClientFactory)
		{
			_config = config;
			_httpClient = httpClientFactory.CreateClient();
		}

		public async Task<string> CreateMomoPaymentUrl(MomoRequest req)
		{
			var momoSection = _config.GetSection("Momo");

			string endpoint = momoSection["PaymentUrl"];
			string partnerCode = momoSection["PartnerCode"];
			string accessKey = momoSection["AccessKey"];
			string secretKey = momoSection["SecretKey"];
			string returnUrl = momoSection["ReturnUrl"];
			string ipnUrl = momoSection["IpnUrl"];
			string requestType = "captureWallet";
			string orderId = $"{req.OrderId}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
			string orderInfo = req.OrderInfo.ToString();
			string amount = ((long)req.Amount).ToString();
			string requestId = Guid.NewGuid().ToString();
			string userStk = "0329126894";
			string extraData = Convert.ToBase64String(
					   Encoding.UTF8.GetBytes($"stk={userStk}"));

			string rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}" +
							 $"&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}" +
							 $"&partnerCode={partnerCode}&redirectUrl={returnUrl}" +
							 $"&requestId={requestId}&requestType={requestType}";

			string signature = CreateSignature(secretKey, rawHash);

			var payload = new
			{
				partnerCode,
				accessKey,
				requestId,
				amount,
				orderId,
				orderInfo,
				redirectUrl = returnUrl,
				ipnUrl,
				extraData,
				requestType,
				signature,
				lang = "vi"
			};

			var json = System.Text.Json.JsonSerializer.Serialize(payload);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(endpoint, content);
			var responseContent = await response.Content.ReadAsStringAsync();

			using var doc = JsonDocument.Parse(responseContent);
			if (doc.RootElement.TryGetProperty("payUrl", out var payUrl))
			{
				return payUrl.GetString();
			}

			var message = doc.RootElement.TryGetProperty("message", out var msg)
							? msg.GetString()
							: "Unknown error from Momo";

			throw new Exception($"Momo error: {message}");
		}



		private string CreateSignature(string key, string rawData)
		{
			using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
			byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
			return BitConverter.ToString(hash).Replace("-", "").ToLower();
		}
	}
}
