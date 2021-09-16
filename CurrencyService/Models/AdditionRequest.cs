using Contracts;

namespace CurencyModule.Models
{
	public class AdditionRequest
	{
		public ICurrency FirstTermCurrency { get; set; }
		public decimal FirstTermCurrencyValue { get; set; }
		public ICurrency SecondTermCurrency { get; set; }
		public decimal SecondTermCurrencyValue { get; set; }
		public ICurrency SumCurrency { get; set; }

		public AdditionRequest()
		{
			if (SumCurrency == null)
				SumCurrency = FirstTermCurrency;
		}
	}
}