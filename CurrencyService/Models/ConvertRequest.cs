using Contracts;

namespace CurencyModule.Models
{
	public class ConvertRequest
	{
		public ICurrency FromCurrency { get; set; }
		public ICurrency ToCurrency { get; set; }
		public decimal CurrencyAmount { get; set; }
	}
}