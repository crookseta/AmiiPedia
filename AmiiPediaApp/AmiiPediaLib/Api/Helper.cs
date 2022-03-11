using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AmiiPedia.Api
{
	public class Helper
	{
		public static HttpClient ApiClient { get; set; }

		public static void InitializeClient()
		{
			ApiClient = new HttpClient();
			ApiClient.BaseAddress = new Uri("https://www.amiiboapi.com/api/");
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}
