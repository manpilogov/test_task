using Contracts;

namespace CurencyModule.Models
{
	public class SubtractionRequest
	{
		public ICurrency DecreasingCurrency { get; set; }
		public decimal DecreasingCurrencyValue { get; set; }
		public ICurrency SubtractionCurrency { get; set; }
		public decimal SubtractionCurrencyValue { get; set; }
		public ICurrency DifferenceCurrency { get; set; }

		public SubtractionRequest()
		{
			if (DifferenceCurrency == null)
				DifferenceCurrency = DecreasingCurrency;
		}
	}
}