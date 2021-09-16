using DataProvider.Models;

namespace DataProvider
{
	public interface IRateProvider
	{
		RateResponse GetRate(RateRequest request);
	}
}