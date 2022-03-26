using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AmiiPedia.Models
{
	public class AmiiboModel
	{
		/// <summary>
		/// List of amiibos
		/// </summary>
		[JsonProperty("amiibo", Required = Required.Always)]
		public Amiibo[] Amiibos { get; set; }

		public Amiibo this[int index]
		{
			get => Amiibos[index];
		}

		public int Length()
		{
			return Amiibos.Length;
		}

		//Returns the franchises in the list and how many amiibos belong to each
		public List<(string franchise, int count)> GetFranchises()
		{
			if (Amiibos == null)
			{
				return null;
			}

			List<(string franchise, int count)> list = new List<(string franchise, int count)>();
			int _count = 0;
			string last = string.Empty;

			foreach (var i in Amiibos)
			{
				if (_count == 0)
				{
					last = i.GameSeries;
				}

				if (last == i.GameSeries) //If the GameSeries is still the same, then just add to the counter
				{
					_count++;
				}
				else //if the game series changed, restart the counting with the new series
				{
					list.Add((last, _count));
					last = i.GameSeries;
					_count = 1;
				}
			}

			return list;
		}

		public List<Amiibo> GetAmiibosAsList()
		{
			return new List<Amiibo>(Amiibos);
		}

		public bool Contains(string name, Amiibo.Parameter parameter)
		{
			foreach (var i in Amiibos)
			{
				switch (parameter)
				{
					case Amiibo.Parameter.AmiiboSeries:
						if (i.AmiiboSeries.ToUpper().Contains(name.ToUpper()))
						{
							return true;
						}
						break;
					case Amiibo.Parameter.Character:
						if (i.Character.ToUpper().Contains(name.ToUpper()))
						{
							return true;
						}
						break;
					case Amiibo.Parameter.GameSeries:
						if (i.GameSeries.ToUpper().Contains(name.ToUpper()))
						{
							return true;
						}
						break;
					case Amiibo.Parameter.Name:
						if (i.Name.ToUpper().Contains(name.ToUpper()))
						{
							return true;
						}
						break;
					case Amiibo.Parameter.Type:
						if (i.Type.ToUpper().Contains(name.ToUpper()))
						{
							return true;
						}
						break;
				}
			}

			return false;
		}
	}

	public class Amiibo
	{
		/// <summary>
		/// Franchise the Amiibos represents
		/// </summary>
		[JsonProperty("amiiboSeries")]
		public string AmiiboSeries { get; set; }
		/// <summary>
		/// The videogame character represented in the Amiibos
		/// </summary>
		[JsonProperty("character")]
		public string Character { get; set; }
		/// <summary>
		/// Videogame series where the character represented in the amiibo comes from
		/// </summary>
		[JsonProperty("gameSeries")]
		public string GameSeries { get; set; }
		/// <summary>
		/// The Amiibos's image url
		/// </summary>
		[JsonProperty("image")]
		public string Image { get; set; }
		/// <summary>
		/// Amiibos's name
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }
		/// <summary>
		/// Amiibos's release dates in:
		/// Australia(AU),
		/// Europe(EU),
		/// Japan(JP),
		/// North America(NA)
		/// </summary>
		[JsonProperty("release")]
		public Release Release { get; set; }
		/// <summary>
		/// Type of Amiibos(Figure, card, band, etc.)
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }

		public enum Parameter
		{
			AmiiboSeries,
			Character,
			GameSeries,
			Image,
			Name,
			Release,
			Type
		}
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
					for (i = 0; i < Au.Length; i++)
					{
						if (Au[i] == '-')
						{
							i++;
							break;
						}
						year += Au[i];
					}
					for (; i < Au.Length; i++)
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
