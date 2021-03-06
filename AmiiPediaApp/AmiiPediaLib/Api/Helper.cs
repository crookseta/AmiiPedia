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
		public static string BaseAddress { get; } = "https://www.amiiboapi.com/api/";

		public static void InitializeClient()
		{
			ApiClient = new HttpClient();
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}

	public class ApiConnectionException : Exception
	{
		public ApiConnectionException() : base() { }
		public ApiConnectionException(string message) : base(message) { }
		public ApiConnectionException(string message, Exception internalException) : base(message, internalException) { }
	}
}
