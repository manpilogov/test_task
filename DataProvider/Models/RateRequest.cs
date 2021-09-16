using Contracts;

namespace DataProvider.Models
{
	public class RateRequest
	{
		public ICurrency FirstCurrency { get; set; }
		public ICurrency SecondCurrency { get; set; }
	}
}