using System;
using System.Collections.Generic;
using DataProvider.Models;

namespace DataProvider
{
	/// <summary>
	/// Имитация провайдера курсов
	/// </summary>
	public class RateProvider : IRateProvider
	{
		private Dictionary<string, Dictionary<string, decimal>> Rates { get; } // Стакан может быть ассиметричен, потому два уровня вложенности

		public RateProvider()
		{
			Rates = new Dictionary<string, Dictionary<string, decimal>>()
			{
				{"USD", new Dictionary<string, decimal>()
					{
						{"RUB", 72.1m}
					}
				},
				{"RUB", new Dictionary<string, decimal>()
					{
						{"USD", 1.0m/72.1m}
					}
				},
			};
		}
		
		public RateResponse GetRate(RateRequest request)
		{
			var response = new RateResponse() {Rate = 1.0m};
			if (request.FirstCurrency.Ticker == request.SecondCurrency.Ticker)
				return response;

			try
			{
				response.Rate = Rates[request.FirstCurrency.Ticker][request.SecondCurrency.Ticker];
			}
			catch (KeyNotFoundException e)
			{
				response.HasError = true;
				response.Details = $"No such currency combination;";
			}

			return response;
		}
	}
}