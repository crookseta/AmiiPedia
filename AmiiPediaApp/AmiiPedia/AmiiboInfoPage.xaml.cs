using AmiiPedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AmiiPedia
{
	/// <summary>
	/// Lógica de interacción para AmiiboInfoPage.xaml
	/// </summary>
	public partial class AmiiboInfoPage : Page
	{
		private readonly Amiibo _amiibo = new Amiibo();
		private Games[] usedIn;

		public AmiiboInfoPage()
		{
			InitializeComponent();
			usedIn = new Games[0];
		}
		public AmiiboInfoPage(Amiibo amiibo)
		{
			InitializeComponent();
			_amiibo = amiibo;
			usedIn = amiibo.GetGames();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			amiiboImage.Source = new BitmapImage(new Uri(_amiibo.Image, UriKind.Absolute));

			amiiboName.Text = _amiibo.Name;
			amiiboSeries.Text = $"Series: {_amiibo.AmiiboSeries}";
			amiiboFranchise.Text = $"Franchise: {_amiibo.GameSeries}";

			releaseAU.Text = "AU:\t" + _amiibo.Release.Au;
			releaseEU.Text = "EU:\t" + _amiibo.Release.Eu;
			releaseJP.Text = "JP:\t" + _amiibo.Release.Jp;
			releaseNA.Text = "NA:\t" + _amiibo.Release.Na;

			gamesListView.ItemsSource = usedIn;

			gameTitleColumn.DisplayMemberBinding = new Binding("Name");
			platformColumn.DisplayMemberBinding = new Binding("Platform");
		}

		private void gamesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if(gamesListView.SelectedItem != null)
			{
				amiiboUsage.Text = usedIn[gamesListView.SelectedIndex].UsageString.Trim();
			}
		}

		private void gamesListView_KeyDown(object sender, KeyEventArgs e)
		{

		}
	}
}
