using AmiiPedia.Api;
using AmiiPedia.Extensions;
using AmiiPedia.Models;
using AmiiPedia.Processor;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AmiiPedia
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static MainWindow Instance { get; private set; }
		public int MaxPage
		{
			get
			{
				var c = Amiibos.Count;
				var i = Math.Round((double)(Convert.ToDouble(c) / Convert.ToDouble(amiibosPerPage)), 0, MidpointRounding.ToPositiveInfinity);
				return Convert.ToInt32(i);
			}
		}
		public List<string> Franchises;

		private List<Image> AmiiboImages = new List<Image>();
		private bool NotSearching = true;
		private AmiiboModel allAmiibos = new AmiiboModel();

		private AmiiboDirectoryPage _amiiboDirectory;
		private List<Amiibo?> Amiibos
		{
			get
			{
				if (NotSearching)
				{
					return allAmiibos.GetAmiibosAsList();
				}
				else
				{
					return SearchedAmiibos;
				}
			}
		}

		private List<Amiibo> SearchedAmiibos = new List<Amiibo>();
		private Amiibo?[] amiibosInPage = new Amiibo[amiibosPerPage];
		private const int amiibosPerPage = 90;
		private string? lastSearch;

		public MainWindow()
		{
			InitializeComponent();
			Helper.InitializeClient();
			Instance = this;

			NotSearching = true;
		}
		#region Methods
		private async Task ProcessAmiibos()
		{
			try
			{
				allAmiibos = await AmiiboProcessor.LoadAmiibos();
			}
			catch (ApiConnectionException ex)
			{
				var mbox = MessageBox.Show(this,
					   $"There has been an error connecting to the API \n{ex.Message}\n{ex.InnerException?.Message}",
						"API Error",
						 MessageBoxButton.OK, MessageBoxImage.Error);
				if (mbox == MessageBoxResult.OK)
				{
					Application.Current.Shutdown();
				}
				if(mbox == MessageBoxResult.None)
				{
					Application.Current.Shutdown();
				}
			}
		}
		/// <summary>
		/// Initiates amiibosInPage and AmiiboImages the first time its called.
		/// Clears the ImagesPanel's children and AmiiboImages.
		/// Calls the LoadImages method passing amiibosInPage.Length and amiibosInPage
		/// </summary>
		/// <returns></returns>
		private async Task LoadImageArray()
		{
			if (amiibosInPage == null)
			{
				amiibosInPage = new Amiibo[amiibosPerPage];
				AmiiboImages = new List<Image>(amiibosInPage.Length);
			}

			AmiiboImages.Clear();

			await LoadImages(amiibosInPage.Length, amiibosInPage);
		}
		/// <summary>
		/// If <paramref name="name"/> is different to the last searched name,
		/// clears the SearchedAmiibos list and AmiibosImages, and
		/// clears the ImagesPanel's children.
		/// Populates SearchedAmiibos with amiibos in the specified Game Series
		/// </summary>
		/// <param name="name">Game series to search</param>
		/// <returns></returns>
		private async Task SearchAmiibos(string name, Amiibo.Parameter parameter)
		{
			if (lastSearch == name)
			{
				return;
			}
			await Task.Run(() =>
			{
				if (!allAmiibos.Contains(name, parameter))
				{
					return;
				}

				NotSearching = false;

				if (SearchedAmiibos != null)
				{
					SearchedAmiibos.Clear();
				}
				lastSearch = name;
				AmiiboImages.Clear();

				switch (parameter)
				{
					case Amiibo.Parameter.AmiiboSeries:
						for (int i = 0; i < allAmiibos.Length(); i++)
						{
							if (allAmiibos.Amiibos[i].AmiiboSeries != name)
							{
								continue;
							}
							SearchedAmiibos?.Add(allAmiibos.Amiibos[i]);
						}
						break;
					case Amiibo.Parameter.Character:
						for (int i = 0; i < allAmiibos.Length(); i++)
						{
							if (!allAmiibos.Amiibos[i].Character.Contains(name, StringComparison.CurrentCultureIgnoreCase)) //!= name
							{
								continue;
							}
							SearchedAmiibos?.Add(allAmiibos.Amiibos[i]);
						}
						break;
					case Amiibo.Parameter.GameSeries:
						for (int i = 0; i < allAmiibos.Length(); i++)
						{
							if (allAmiibos.Amiibos[i].GameSeries != name)
							{
								continue;
							}
							SearchedAmiibos?.Add(allAmiibos.Amiibos[i]);
						}
						break;
					case Amiibo.Parameter.Name:
						for (int i = 0; i < allAmiibos.Length(); i++)
						{
							if (allAmiibos.Amiibos[i].Name != name)
							{
								continue;
							}
							SearchedAmiibos?.Add(allAmiibos.Amiibos[i]);
						}
						break;
					case Amiibo.Parameter.Type:
						for (int i = 0; i < allAmiibos.Length(); i++)
						{
							if (allAmiibos.Amiibos[i].Type != name)
							{
								continue;
							}
							SearchedAmiibos?.Add(allAmiibos.Amiibos[i]);
						}
						break;
					default:
						throw new Exception("Parameter type " + parameter + " not supported.");
				}
			});
			mainFrame.Content = new AmiiboDirectoryPage(SearchedAmiibos);
		}
		/// <summary>
		/// Adds images from <paramref name="source"/> to the ImagesPanel
		/// </summary>
		/// <param name="count">Max amount of images added</param>
		/// <param name="source">Source of the images</param>
		/// <returns></returns>
		private async Task LoadImages(int count, IList<Amiibo> source)
		{
			for (int i = 0; i < count; i++)
			{
				if (source[i] == null)
				{
					break;
				}

				AmiiboImages[i].Source = new BitmapImage(
					  new Uri(source[i].Image, UriKind.Absolute)
					  );
				AmiiboImages[i].ToolTip = source[i].Name;
				AmiiboImages[i].Tag = source[i];
			}
		}

		private async Task LoadFranchisesPanel()
		{
			Franchises = await AmiiboProcessor.LoadFranchises();
			franchisesPanel.Children.Clear();
			for(int i = 0; i < Franchises.Count; i++)
			{
				var btn = GetSimpleButtonTemplate();

				btn.Content = Franchises[i];
				btn.ToolTip = Franchises[i];

				franchisesPanel.Children.Add(btn);
			}
		}

		private SimpleButton GetSimpleButtonTemplate()
		{
			SimpleButton temp = new SimpleButton();

			temp.VerticalAlignment = VerticalAlignment.Center;
			temp.HorizontalContentAlignment = HorizontalAlignment.Left;
			temp.FontSize = 20;
			temp.Foreground = franchiseButtonTemplate.Foreground;
			temp.FontWeight = FontWeights.Bold;
			temp.HoverColor = franchiseButtonTemplate.HoverColor;
			temp.Style = franchiseButtonTemplate.Style;
			temp.Click += franchiseBtn_Click;

			return temp;
		}
		/// <summary>
		/// Its called whenever the list of amiibos is changed
		/// </summary>
		private void OnAmiiboUpdate()
		{
			_amiiboDirectory?.Update();

			searchBox.Text = string.Empty;
		}

		private void SectionButtonsVisibility(bool visible)
		{
			if (visible == false)
				franchisesPanel.Visibility = Visibility.Collapsed;
			else
				franchisesPanel.Visibility = Visibility.Visible;
		}

		private void SearchButtonsVisibility(bool visible)
		{
			searchBtn.IsEnabled = visible;
			searchBox.IsEnabled = visible;

		}
		#endregion
		//Event for the WPF components
		#region WPF Events
		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			await LoadFranchisesPanel();
			
			await ProcessAmiibos();

			_amiiboDirectory = new AmiiboDirectoryPage(allAmiibos.Amiibos);

			homeBtn.IsEnabled = true;
			SectionButtonsVisibility(true);
			SearchButtonsVisibility(true);

			mainFrame.Content = _amiiboDirectory;
		}

		private async void searchBtn_Click(object sender, RoutedEventArgs e)
		{
			await SearchAmiibos(searchBox.Text, Amiibo.Parameter.Character);

			OnAmiiboUpdate();
		}

		private async void searchBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				await SearchAmiibos(searchBox.Text, Amiibo.Parameter.Character);

				OnAmiiboUpdate();
			}
		}

		private void homeBtn_Click(object sender, RoutedEventArgs e)
		{
			if(mainFrame.Content != _amiiboDirectory)
			{
				lastSearch = null;
				NotSearching = true;

				mainFrame.Content = _amiiboDirectory;
				OnAmiiboUpdate();
			}
		}

		private async void franchiseBtn_Click(object sender, RoutedEventArgs e)
		{
			foreach (Button i in franchisesPanel.Children)
			{
				if (e.Source == i)
				{
					await SearchAmiibos(i.Content.ToString(), Amiibo.Parameter.GameSeries);
					break;
				}
			}
			OnAmiiboUpdate();
		}
		#endregion
	}
}
