using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmiiPedia.Models
{
	public class AmiiboModel
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
		public DateTime Au { get; set; }
		// Europe
		public DateTime Eu { get; set; }
		// Japan
		public DateTime Jp { get; set; }
		// North America
		public DateTime Na { get; set; }
	}
}
