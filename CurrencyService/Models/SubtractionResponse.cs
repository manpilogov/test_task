using Contracts;

namespace CurencyModule.Models
{
	public class SubtractionResponse : BaseResponse
	{
		public ICurrency DifferenceCurrency { get; set; }
		public decimal DifferenceValue { get; set; }
	}
}