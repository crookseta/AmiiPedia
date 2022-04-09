using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmiiPedia.Models
{
	public class AmiiboModel
	{
		/// <summary>
		/// List of amiibos
		/// </summary>
		//[JsonProperty("amiibo", Required = Required.Always)]
		//public Amiibo[] Amiibos { get; set; }
		[JsonProperty("amiibo", Required = Required.Always)]
		public List<Amiibo> Amiibos { get; set; }

		public Amiibo this[int index]
		{
			get => Amiibos[index];
		}
		public AmiiboModel()
		{
			Amiibos = new List<Amiibo>();
		}

		public int Length()
		{
			return Amiibos.Count;
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

		public static AmiiboModel operator +(AmiiboModel a, AmiiboModel b)
		{
			AmiiboModel c = new AmiiboModel();
			c.Amiibos.AddRange(a.Amiibos);
			c.Amiibos.AddRange(b.Amiibos);
			return c;
		}
	}

	public class Amiibo
	{
		private Games[] _nintendo3ds;
		private Games[] _nintendoSwitch;
		private Games[] _nintendoWiiU;

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
		/// 
		/// </summary>
		[JsonProperty("games3DS")]
		public Games[] Nintendo3ds { get => _nintendo3ds; 
			set 
			{
				_nintendo3ds = value;
				foreach(var i in _nintendo3ds)
				{
					i.Platform = "Nintendo 3DS";
				}
			} 
		}
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("gamesSwitch")]
		public Games[] NintendoSwitch { get => _nintendoSwitch;
			set
			{
				_nintendoSwitch = value;
				foreach (var i in _nintendoSwitch)
				{
					i.Platform = "Nintendo Switch";
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("gamesWiiU")]
		public Games[] NintendoWiiU { get => _nintendoWiiU;
			set
			{
				_nintendoWiiU = value;
				foreach (var i in _nintendoWiiU)
				{
					i.Platform = "Nintendo Wii U";
				}
			}
		}
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

		public Games[] GetGames()
		{
			List<Games> games = new List<Games>();
			games.AddRange(Nintendo3ds);
			games.AddRange(NintendoSwitch);
			games.AddRange(NintendoWiiU);
			return games.ToArray();
		}

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

	public class Games
	{
		[JsonProperty("amiiboUsage", Required = Required.Always)]
		public AmiiboUsage[] Usage { get; set; }
		[JsonProperty("gameID", Required = Required.Always)]
		public string[] Id { get; set; }
		[JsonProperty("gameName", Required = Required.Always)]
		public string Name { get; set; }
		public string Platform { get; set; }
		public string UsageString { get => GetUsageString(); }

		public class AmiiboUsage
		{
			[JsonProperty("Usage", Required = Required.Always)]
			public string Usage { get; set; }

			[JsonProperty("write", Required = Required.Always)]
			public bool Write { get; set; }
		}

		public string GetUsageString()
		{
			StringBuilder builder = new StringBuilder();

			foreach(var i in Usage)
			{
				builder.Append($"-{i.Usage}\n");
			}

			return builder.ToString();
		}
	}
}
