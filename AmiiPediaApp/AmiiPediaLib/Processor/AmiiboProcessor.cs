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
		public const string baseUrl = "amiibo";

		public async Task<List<AmiiboModel>> LoadAmiibos()
		{
			string url = baseUrl;

			using (HttpResponseMessage response = await Helper.ApiClient.GetAsync(url))
			{
				if (response.IsSuccessStatusCode)
				{
					List<AmiiboModel> model = await response.Content.ReadAsAsync<List<AmiiboModel>>();
					return model;
				}
				else
				{
					throw new Exception(response.ReasonPhrase);
				}
			}
		}
		public async Task<AmiiboModel> LoadAmiibo(string parameter, string name)
		{
			string url = "";

			if(parameter != null && name != null)
			{
				url = $"/{baseUrl}?{parameter}={name}";
			}
			else
			{
				url = baseUrl;
			}

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
	}
}
