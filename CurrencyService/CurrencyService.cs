using System;
using CurencyModule.Models;
using DataProvider;
using DataProvider.Models;

namespace CurencyModule
{
	public class CurrencyService : ICurrencyService
	{
		private readonly IRateProvider _rateProvider;
		
		public CurrencyService(IRateProvider rateProvider)
		{
			_rateProvider = rateProvider;
		}

		public ConvertResponse Convert(ConvertRequest request) // Да, тут скорее всего был бы async.....
		{
			var convertResponse = new ConvertResponse();
			
			var rateRequest = new RateRequest()
			{
				FirstCurrency = request.FromCurrency, SecondCurrency = request.ToCurrency
			};
			var rateResponse = _rateProvider.GetRate(rateRequest); // А тут await

			if (!rateResponse.HasError)
			{
				try
				{
					convertResponse.Value = rateResponse.Rate * request.CurrencyAmount;
				}
				catch (OverflowException e)
				{
					convertResponse.HasError = true;
					convertResponse.Details = $"Error while converting, overflow";
				}
			}
			else
			{
				convertResponse.HasError = true;
				convertResponse.Details = $"Error while getting the currency rate; Details: {rateResponse.Details}";
			}

			return convertResponse;
		}

		public AdditionResponse Addition(AdditionRequest request)
		{
			var firstTerm = request.FirstTermCurrencyValue;
			var secondTerm = request.SecondTermCurrencyValue;
			var response = new AdditionResponse()
			{
				SumCurrency = request.SumCurrency
			};
			
			if (request.SumCurrency.Ticker != request.FirstTermCurrency.Ticker)
			{
				var convertRequest = new ConvertRequest()
				{
					FromCurrency = request.FirstTermCurrency,
					ToCurrency = request.SumCurrency,
					CurrencyAmount = request.FirstTermCurrencyValue
				};
				var convertResponse = Convert(convertRequest);

				if (convertResponse.HasError)
				{
					response.HasError = true;
					response.Details = $"Error while converting; Details: {convertResponse.Details}";
					return response;
				}
				
				firstTerm = convertResponse.Value;
			}
			
			if (request.SumCurrency.Ticker != request.SecondTermCurrency.Ticker)
			{
				var convertRequest = new ConvertRequest()
				{
					FromCurrency = request.SecondTermCurrency,
					ToCurrency = request.SumCurrency,
					CurrencyAmount = request.SecondTermCurrencyValue
				};
				var convertResponse = Convert(convertRequest);
				
				if (convertResponse.HasError)
				{
					response.HasError = true;
					response.Details = $"Error while converting; Details: {convertResponse.Details}";
					return response;
				}
				
				secondTerm = convertResponse.Value;
			}

			response.SumValue = firstTerm + secondTerm;
			return response;
		}

		public SubtractionResponse Subtraction(SubtractionRequest request)
		{
			// Оптимально использовать Automapper, например
			var additionRequest = new AdditionRequest()
			{
				FirstTermCurrency = request.DecreasingCurrency,
				SecondTermCurrency = request.SubtractionCurrency,
				FirstTermCurrencyValue = request.DecreasingCurrencyValue,
				SecondTermCurrencyValue = -request.SubtractionCurrencyValue,
				SumCurrency = request.DifferenceCurrency
			};

			var additionResponse = Addition(additionRequest);
			return new SubtractionResponse()
			{
				DifferenceCurrency = additionResponse.SumCurrency
				, DifferenceValue = additionResponse.SumValue, HasError = additionResponse.HasError
				, Details = additionResponse.Details
			};
		}
	}
}