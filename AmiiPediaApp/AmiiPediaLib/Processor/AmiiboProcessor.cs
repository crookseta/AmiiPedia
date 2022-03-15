using AmiiPedia.Api;
using AmiiPedia.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmiiPedia.Processor
{
	public class AmiiboProcessor
	{
		private static string baseUrl = Helper.BaseAddress;
		public static async Task<AmiiboModel> LoadAmiibos()
		{
			string url = baseUrl;

			using (HttpResponseMessage response = await Helper.ApiClient.GetAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					AmiiboModel model = await response.Content.ReadAsAsync<AmiiboModel>();
					return model;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
		public static async Task<Amiibo> LoadAmiibo(string parameter, string name)
		{
			string url = "";

			if(parameter != null && name != null)
			{
				url = $"{baseUrl}?{parameter}={name}";
			}
			else
			{
				url = baseUrl;
			}

			using (HttpResponseMessage response = await Helper.ApiClient.GetAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					Amiibo model = await response.Content.ReadAsAsync<Amiibo>();
					return model;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
	}
}
