using CurencyModule.Models;

namespace CurencyModule
{
	public interface ICurrencyService
	{
		ConvertResponse Convert(ConvertRequest request);
		AdditionResponse Addition(AdditionRequest request);
		SubtractionResponse Subtraction(SubtractionRequest request);
	}
}