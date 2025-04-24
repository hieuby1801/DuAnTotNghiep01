using DATN_API.Request;

namespace DATN_API.Service
{
	public interface IPaymentService
	{
		Task<string> CreateMomoPaymentUrl(MomoRequest request);
	}
}
