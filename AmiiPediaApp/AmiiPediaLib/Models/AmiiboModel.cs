using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmiiPedia.Models
{
	public class AmiiboModel
	{
		public Amiibo[] Amiibo { get; set; }

		public int Length()
		{
			return Amiibo.Length;
		}
	}

	public class Amiibo
	{
		/// <summary>
		/// Franchise the Amiibo represents
		/// </summary>
		public string AmiiboSeries { get; set; }
		/// <summary>
		/// The videogame character represented in the Amiibo
		/// </summary>
		public string Character { get; set; }
		/// <summary>
		/// Videogame series where the character represented in the amiibo comes from
		/// </summary>
		public string GameSeries { get; set; }
		/// <summary>
		/// The Amiibo's image url
		/// </summary>
		public string Image { get; set; }
		/// <summary>
		/// Amiibo's name
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Amiibo's release dates in:
		/// Australia(AU),
		/// Europe(EU),
		/// Japan(JP),
		/// North America(NA)
		/// </summary>
		public Release Release { get; set; }
		/// <summary>
		/// Type of Amiibo(Figure, card, band, etc.)
		/// </summary>
		public string Type { get; set; }
	}

	public class Release
	{
		// Australia
		public string Au { get; set; }
		// Europe
		public string Eu { get; set; }
		// Japan
		public string Jp { get; set; }
		// North America
		public string Na { get; set; }

		public DateTime? GetDate(string location)
		{
			string day = "", month = "", year = "";
			DateTime? date = null;
			int i;

			location = location.ToLower();
			switch (location)
			{
				case "au":
					if (Au == null)
						break;
					for(i = 0; i < Au.Length; i++)
					{
						if(Au[i] == '-')
						{
							i++;
							break;
						}
						year += Au[i];
					}
					for(;i< Au.Length; i++)
					{
						if (Au[i] == '-')
						{
							i++;
							break;
						}
						month += Au[i];
					}
					for (; i < Au.Length; i++)
					{
						day += Au[i];
					}
					date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
					break;
				case "eu":
					if (Eu == null)
						break;
					for (i = 0; i < Eu.Length; i++)
					{
						if (Eu[i] == '-')
						{
							i++;
							break;
						}
						year += Eu[i];
					}
					for (; i < Eu.Length; i++)
					{
						if (Eu[i] == '-')
						{
							i++;
							break;
						}
						month += Eu[i];
					}
					for (; i < Eu.Length; i++)
					{
						day += Eu[i];
					}
					date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
					break;
				case "jp":
					if (Jp == null)
						break;
					for (i = 0; i < Jp.Length; i++)
					{
						if (Jp[i] == '-')
						{
							i++;
							break;
						}
						year += Jp[i];
					}
					for (; i < Jp.Length; i++)
					{
						if (Jp[i] == '-')
						{
							i++;
							break;
						}
						month += Jp[i];
					}
					for (; i < Jp.Length; i++)
					{
						day += Jp[i];
					}
					date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
					break;
				case "na":
					if (Na == null)
						break;
					for (i = 0; i < Na.Length; i++)
					{
						if (Na[i] == '-')
						{
							i++;
							break;
						}
						year += Na[i];
					}
					for (; i < Na.Length; i++)
					{
						if (Na[i] == '-')
						{
							i++;
							break;
						}
						month += Na[i];
					}
					for (; i < Na.Length; i++)
					{
						day += Na[i];
					}
					date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
					break;
			}

			return date;
		}
	}
}
