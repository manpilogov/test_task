using Contracts;

namespace CurencyModule.Models
{
	public class AdditionResponse : BaseResponse
	{
		public ICurrency SumCurrency { get; set; }
		public decimal SumValue { get; set; }
	}
}