using Contracts;

namespace CurencyModule.Models
{
	public class ConvertResponse : BaseResponse
	{
		public decimal Value { get; set; }
	}
}