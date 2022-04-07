using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AmiiPedia.Models
{
	public class AmiiboGameSeriesModel
	{
		[JsonProperty("amiibo")]
		public List<GameSeries> Franchises { get; set; }
		public class GameSeries
		{
			[JsonProperty("name")]
			public string Name { get; set; }
		}

		public List<string> GetFranchises()
		{
			List<string> franchises = new List<string>();

			foreach(var i in Franchises)
			{
				if (!franchises.Contains(i.Name))
				{
					franchises.Add(i.Name);
				}
			}

			franchises.Sort();

			return franchises;
		}
	}
}
