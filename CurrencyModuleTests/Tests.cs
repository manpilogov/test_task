using System;
using Contracts;
using CurencyModule;
using CurencyModule.Models;
using DataProvider;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CurrencyModuleTests
{
	[TestFixture]
	public class Tests
	{
		private ServiceProvider _serviceCollection;
		
		[OneTimeSetUp]
		public void Init()
		{
			var services = new ServiceCollection();
			services.AddSingleton<IRateProvider, RateProvider>();
			services.AddSingleton<ICurrencyService, CurrencyService>();
			_serviceCollection = services.BuildServiceProvider();
		}

		[OneTimeTearDown]
		public void Down()
		{
			_serviceCollection.Dispose();
		}
		
		[Test]
		public void TestConvert_Success()
		{
			var rubCurrency = new Currency() {Ticker = "RUB"};
			var usdCurrency = new Currency() {Ticker = "USD"};
			var convertRequest = new ConvertRequest()
			{
				FromCurrency = usdCurrency, ToCurrency = rubCurrency, CurrencyAmount = 2.0m
			};

			var convertResponse = _serviceCollection.GetService<ICurrencyService>()?.Convert(convertRequest);
			Assert.NotNull(convertResponse);
			Assert.Null(convertResponse.Details);
			Assert.False(convertResponse.HasError);
			Assert.That(Math.Abs(144.2m-convertResponse.Value), Is.LessThan(0.01m)); // Взята погрешность менее минимальной единицы валюты (копейки/центы), т.е. 1/100
		}
		
		[Test]
		public void TestConvert_Overflow()
		{
			var errorMessage = "Error while converting, overflow";
			var rubCurrency = new Currency() {Ticker = "RUB"};
			var usdCurrency = new Currency() {Ticker = "USD"};
			var convertRequest = new ConvertRequest()
			{
				FromCurrency = usdCurrency, ToCurrency = rubCurrency, CurrencyAmount = decimal.MaxValue
			};

			var convertResponse = _serviceCollection.GetService<ICurrencyService>()?.Convert(convertRequest);
			Assert.NotNull(convertResponse);
			Assert.NotNull(convertResponse.Details);
			Assert.True(convertResponse.HasError);
			Assert.AreEqual(convertResponse.Details, errorMessage);
		}
		
		[Test]
		public void TestConvert_NoSuchCurrency()
		{
			var errorMessage = "Error while getting the currency rate; Details: No such currency combination;";
			var rubCurrency = new Currency() {Ticker = "EUR"};
			var usdCurrency = new Currency() {Ticker = "USD"};
			var convertRequest = new ConvertRequest()
			{
				FromCurrency = rubCurrency, ToCurrency = usdCurrency, CurrencyAmount = 2.7m
			};

			var convertResponse = _serviceCollection.GetService<ICurrencyService>()?.Convert(convertRequest);
			Assert.NotNull(convertResponse);
			Assert.NotNull(convertResponse.Details);
			Assert.True(convertResponse.HasError);
			Assert.AreEqual(convertResponse.Details, errorMessage);
		}
		
		[Test]
		public void TestAddition_Success()
		{
			var rubCurrency = new Currency() {Ticker = "RUB"};
			var usdCurrency = new Currency() {Ticker = "USD"};
			var additionRequest = new AdditionRequest()
			{
				FirstTermCurrency = rubCurrency,
				FirstTermCurrencyValue = 72.1m,
				SecondTermCurrency = usdCurrency,
				SecondTermCurrencyValue = 1.0m,
				SumCurrency = usdCurrency
			};

			var additionResponse = _serviceCollection.GetService<ICurrencyService>()?.Addition(additionRequest);
			Assert.NotNull(additionResponse);
			Assert.Null(additionResponse.Details);
			Assert.False(additionResponse.HasError);
			Assert.AreEqual(additionResponse.SumCurrency.Ticker, usdCurrency.Ticker);
			Assert.That(Math.Abs(2.0m - additionResponse.SumValue), Is.LessThan(0.01m));
		}
		
		[Test]
		public void TestSubtraction_Success()
		{
			var rubCurrency = new Currency() {Ticker = "RUB"};
			var usdCurrency = new Currency() {Ticker = "USD"};
			var subtractionRequest = new SubtractionRequest()
			{
				DecreasingCurrency = usdCurrency,
				DecreasingCurrencyValue = 2.0m,
				SubtractionCurrency = rubCurrency,
				SubtractionCurrencyValue = 72.1m,
				DifferenceCurrency = usdCurrency
			};

			var subtractionResponse = _serviceCollection.GetService<ICurrencyService>()?.Subtraction(subtractionRequest);
			Assert.NotNull(subtractionResponse);
			Assert.Null(subtractionResponse.Details);
			Assert.False(subtractionResponse.HasError);
			Assert.AreEqual(subtractionResponse.DifferenceCurrency.Ticker, usdCurrency.Ticker);
			Assert.That(Math.Abs(1.0m - subtractionResponse.DifferenceValue), Is.LessThan(0.01m));
		}
	}
}