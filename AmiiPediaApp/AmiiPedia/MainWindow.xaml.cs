using AmiiPedia.Api;
using AmiiPedia.Models;
using AmiiPedia.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public int MaxPage
		{
			get
			{
				var c = Amiibos.Count;
				var i = Math.Round((double)(Convert.ToDouble(c) / Convert.ToDouble(amiibosPerPage)),0,MidpointRounding.ToPositiveInfinity);
				return Convert.ToInt32(i);
			}
		}
		public int CurrentPage = 1;
		public List<Image> AmiiboImages = new List<Image>();
		public bool NotSearching = true;

		AmiiboModel allAmiibos;
		List<Amiibo?> Amiibos { get
			{
				if (NotSearching)
				{
					return allAmiibos.GetAmiibosAsList();
				}
				else
				{
					return SearchedAmiibos;
				}
			} }

		List<Amiibo> SearchedAmiibos = new List<Amiibo>();
		Amiibo?[] amiibosInPage = new Amiibo[amiibosPerPage];
		readonly Regex numOnly = new Regex("[^0-9]+");
		const int amiibosPerPage = 25;

		string? lastSearch;

		public MainWindow()
		{
			InitializeComponent();
			Helper.InitializeClient();
			
			NotSearching = true;
		}
		#region Methods
		private async Task LoadImageArray()
		{
			if(amiibosInPage == null)
			{
				amiibosInPage = new Amiibo[amiibosPerPage];
				AmiiboImages = new List<Image>(amiibosInPage.Length);
			}

			ImagesPanel.Children.Clear();
			AmiiboImages.Clear();
			for(int i = 0; i < amiibosInPage.Length; i++)
			{
				if(amiibosInPage[i] == null)
				{
					break;
				}

				AmiiboImages.Add(new Image());
				AmiiboImages[i].Height = ImageTemplateParameters().height;
				AmiiboImages[i].Width = ImageTemplateParameters().width;
				AmiiboImages[i].MinHeight = ImageTemplateParameters().minheight;
				AmiiboImages[i].MinWidth = ImageTemplateParameters().minwidth;
				AmiiboImages[i].Margin = ImageTemplateParameters().margin;

				AmiiboImages[i].Source = new BitmapImage(
					  new Uri(amiibosInPage[i].Image, UriKind.Absolute)
					  );

				ImagesPanel.Children.Add(AmiiboImages[i]);
			}
		}
		private async Task LoadImageArray(string name)
		{
			if (lastSearch != name)
			{
				NotSearching = false;

				if (SearchedAmiibos != null)
				{
					SearchedAmiibos.Clear();
				}
				lastSearch = name;
				ImagesPanel.Children.Clear();
				AmiiboImages.Clear();

				for (int i = 0; i < allAmiibos.Length(); i++)
				{
					if (allAmiibos.Amiibo[i].GameSeries != name)
					{
						continue;
					}
					SearchedAmiibos.Add(allAmiibos.Amiibo[i]);

					AmiiboImages.Add(new Image());
				}

				for(int i = 0; i < SearchedAmiibos.Count; i++)
				{
					AmiiboImages[i].Height = ImageTemplateParameters().height;
					AmiiboImages[i].Width = ImageTemplateParameters().width;
					AmiiboImages[i].MinHeight = ImageTemplateParameters().minheight;
					AmiiboImages[i].MinWidth = ImageTemplateParameters().minwidth;
					AmiiboImages[i].Margin = ImageTemplateParameters().margin;

					AmiiboImages[i].Source = new BitmapImage(new Uri(Amiibos[i].Image, UriKind.Absolute));

					ImagesPanel.Children.Add(AmiiboImages[i]);
				}
			}
		}

		private async Task LoadImages(int count)
		{
			Uri uriSource;

			for (int i = 0; i < count; i++)
			{
				AmiiboImages[i].Height = ImageTemplateParameters().height;
				AmiiboImages[i].Width = ImageTemplateParameters().width;
				AmiiboImages[i].MinHeight = ImageTemplateParameters().minheight;
				AmiiboImages[i].MinWidth = ImageTemplateParameters().minwidth;
				AmiiboImages[i].Margin = ImageTemplateParameters().margin;

				uriSource = new Uri(Amiibos[i].Image, UriKind.Absolute);

				AmiiboImages[i].Source = new BitmapImage(new Uri(amiibosInPage[i].Image, UriKind.Absolute));

				ImagesPanel.Children.Add(AmiiboImages[i]);
			}
		}

		private (float width, float height, float minwidth, float minheight, Thickness margin) ImageTemplateParameters()
		{
			float width = 50f, height = 90, minwidth = 50, minheight = 80;
			Thickness margin = new Thickness(20, 20, 20, 20);

			return (width, height, minwidth, minheight, margin);
		}

		private async Task InitiateAmiiboList()
		{
			//Amiibos = allAmiibos.GetAmiibosAsList();

			for (int i = 0, j = 0; i < amiibosPerPage; i++)
			{
				if (i >= Amiibos.Count)
				{
					break;
				}

				amiibosInPage[j] = Amiibos[i];

				j++;
			}
			OnAmiiboUpdate();
			await LoadImageArray();
		}

		private async Task PopulateAmiiboList(int page)
		{
			int indexMult = amiibosPerPage * (page - 1);
			int indexLimit = amiibosPerPage * page;
			for (int i = indexMult, j = 0; i < indexLimit; i++)
			{
				if(i >= Amiibos.Count)
				{
					amiibosInPage[j] = null;
					continue;
				}

				amiibosInPage[j] = Amiibos[i];

				j++;
			}
			OnAmiiboUpdate();
			await LoadImageArray();
		}

		private bool IsTextAllowed(string text)
		{
			return !numOnly.IsMatch(text);
		}

		private void OnAmiiboUpdate()
		{
			pageNumber.Text = CurrentPage.ToString();

			if(Amiibos.Count <= amiibosPerPage)
			{
				PageButtonsVisibility(false);
			}
			else
			{
				PageButtonsVisibility(true);
			}


		}

		void PageButtonsVisibility(bool visible)
		{
			if(visible == false)
			{
				prevBtn.Visibility = Visibility.Hidden;
				pageNumber.Visibility = Visibility.Hidden;
				nextBtn.Visibility = Visibility.Hidden;
			}
			else
			{
				prevBtn.Visibility = Visibility.Visible;
				pageNumber.Visibility = Visibility.Visible;
				nextBtn.Visibility = Visibility.Visible;
			}
				
		}

		void SectionButtonsVisibility(bool visible)
		{
			if (visible == false)
				franchisesPanel.Visibility = Visibility.Hidden;
			else
				franchisesPanel.Visibility = Visibility.Visible;
		}
		#endregion

		#region WPF Events
		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			allAmiibos = await AmiiboProcessor.LoadAmiibos();
			
			await InitiateAmiiboList();

			homeBtn.IsEnabled = true;
			PageButtonsVisibility(true);
			SectionButtonsVisibility(true);
		}
		/*
		private async void searchBtn_Click(object sender, RoutedEventArgs e)
		{
			NotSearching = false;
			await LoadImageArray(searchBox.Text);
		}
		*/
		private void pageNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsTextAllowed(e.Text);
		}

		private async void pageNumber_KeyDown(object sender, KeyEventArgs e)
		{
			var page = Convert.ToInt32(pageNumber.Text);
			if(e.Key == Key.Enter && page > 0)
			{
				if(page > MaxPage)
				{
					page = MaxPage;
				}
				CurrentPage = page;
				await PopulateAmiiboList(page);
			}
			OnAmiiboUpdate();
		}

		private async void nextBtn_Click(object sender, RoutedEventArgs e)
		{
			int page = Convert.ToInt32(CurrentPage) + 1;
			if (page <= MaxPage)
			{
				CurrentPage = page;
				await PopulateAmiiboList(page);
			}
			OnAmiiboUpdate();
		}

		private async void prevBtn_Click(object sender, RoutedEventArgs e)
		{
			int page = Convert.ToInt32(CurrentPage) - 1;
			if (page > 0)
			{
				CurrentPage = page;
				await PopulateAmiiboList(page);
			}
			OnAmiiboUpdate();
		}

		private async void homeBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!NotSearching)
			{
				lastSearch = null;
				CurrentPage = 1;
				NotSearching = true;

				PageButtonsVisibility(true);

				await InitiateAmiiboList();
				OnAmiiboUpdate();
			}
		}

		private async void franchiseBtn_Click(object sender, RoutedEventArgs e)
		{
			foreach(Button i in franchisesPanel.Children)
			{
				if(e.Source == i)
				{
					NotSearching = false;
					CurrentPage = 1;
					await LoadImageArray(i.Content.ToString());
					await PopulateAmiiboList(1);
					break;
				}
			}
			OnAmiiboUpdate();
		}
		#endregion
	}
}
