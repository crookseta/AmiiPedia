using AmiiPedia.Api;
using AmiiPedia.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AmiiPedia.Processor
{
	public class AmiiboProcessor
	{
		private static readonly string _baseUrl = Helper.BaseAddress;

		public static async Task<List<string>> LoadFranchises()
		{
			string url = _baseUrl + "gameseries/";

			try
			{
				AmiiboGameSeriesModel model = new AmiiboGameSeriesModel();
				var gameseries = await Helper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

				using(var body = gameseries.Content)
				{
					if (gameseries.IsSuccessStatusCode)
					{
						model = await body.ReadAsAsync<AmiiboGameSeriesModel>();
						return model.GetFranchises();
					}
					else
					{
						throw new Exception(gameseries.ReasonPhrase);
					}
				}
			}
			catch (Exception ex)
			{
				throw new ApiConnectionException("Error while connecting to API", ex);
			}
		}

		public static async Task<AmiiboModel> LoadAmiibos()
		{
			string urlFigures = _baseUrl + "amiibo/?type=figure&showusage";
			string urlCards = _baseUrl + "amiibo/?type=card&showusage";


			try
			{
				AmiiboModel model;
				var figures = await Helper.ApiClient.GetAsync(urlFigures, HttpCompletionOption.ResponseContentRead);

				using (var body = figures.Content)
				{
					
					if (figures.IsSuccessStatusCode)
					{
						model = await body.ReadAsAsync<AmiiboModel>();
					}
					else
					{
						throw new Exception(figures.ReasonPhrase);
					}
				}

				var cards = await Helper.ApiClient.GetAsync(urlCards, HttpCompletionOption.ResponseContentRead);

				using(var body = cards.Content)
				{
					if (cards.IsSuccessStatusCode)
					{
						model += await body.ReadAsAsync<AmiiboModel>();
					}
					else
					{
						throw new Exception(cards.ReasonPhrase);
					}
				}

				return model;
			}
			catch (Exception ex)
			{
				throw new ApiConnectionException("Error while connecting to API", ex);
			}
		}
		//public static async Task<Amiibo> LoadAmiibo(string parameter, string name)
		//{
		//	string url;
		//	if (parameter != null && name != null)
		//	{
		//		url = $"{_baseUrl}?{parameter}={name}";
		//	}
		//	else
		//	{
		//		url = _baseUrl;
		//	}

		//	using (HttpResponseMessage response = await Helper.ApiClient.GetAsync(url))
		//	{
		//		if (response.IsSuccessStatusCode)
		//		{
		//			Amiibo model = await response.Content.ReadAsAsync<Amiibo>();
		//			return model;
		//		}
		//		else
		//		{
		//			throw new Exception(response.ReasonPhrase);
		//		}
		//	}
		//}

	}
}
