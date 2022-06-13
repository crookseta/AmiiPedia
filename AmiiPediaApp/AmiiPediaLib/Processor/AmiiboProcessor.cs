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
			string url = _baseUrl + "amiibo/";

			try
			{
				AmiiboModel model;
				var obj = await Helper.ApiClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);

				using(var body = obj.Content)
				{
					if (obj.IsSuccessStatusCode)
					{
						model = await body.ReadAsAsync<AmiiboModel>();
					}
					else
					{
						throw new Exception(obj.ReasonPhrase);
					}
				}

				return model;
			}
			catch (Exception ex)
			{
				throw new ApiConnectionException("Error while connecting to API", ex);
			}
		}

	}
}
