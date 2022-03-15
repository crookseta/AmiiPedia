using AmiiPedia.Api;
using AmiiPedia.Models;
using AmiiPedia.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
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
				var c = allAmiibos.Length();
				var i = (c / amiibosPerPage);
				return Convert.ToInt32(i);
			}
		}
		public int CurrentPage = 1;
		public List<Image> AmiiboImages = new List<Image>();

		AmiiboModel allAmiibos;
		List<Amiibo> SearchedAmiibos = new List<Amiibo>();
		Amiibo[] amiibosInPage = new Amiibo[amiibosPerPage];
		readonly Regex numOnly = new Regex("[^0-9]+");
		const int amiibosPerPage = 30;
		
		string lastSearch;

		public MainWindow()
		{
			InitializeComponent();
			Helper.InitializeClient();
		}
		
		private async Task LoadImages()
		{
			if(amiibosInPage == null)
			{
				amiibosInPage = new Amiibo[amiibosPerPage];
				AmiiboImages = new List<Image>(amiibosInPage.Length);
			}

			ImagesPanel.Children.Clear();

			for(int i = 0; i < amiibosPerPage; i++)
			{
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
		private async Task LoadImages(string name)
		{
			if (lastSearch != name)
			{
				if (SearchedAmiibos != null)
				{
					SearchedAmiibos.Clear();
				}
				lastSearch = name;
				ImagesPanel.Children.Clear();
				AmiiboImages.Clear();
				Uri uriSource;

				for (int i = 0; i < allAmiibos.Length(); i++)
				{
					if (allAmiibos.Amiibo[i].Character != name)
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

					uriSource = new Uri(allAmiibos.Amiibo[i].Image, UriKind.Absolute);

					AmiiboImages[i].Source = new BitmapImage(uriSource);

					ImagesPanel.Children.Add(AmiiboImages[i]);
				}
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
			for (int i = 0, j = 0; i < amiibosPerPage; i++)
			{
				amiibosInPage[j] = allAmiibos.Amiibo[i];

				j++;
			}
			OnAmiiboUpdate();
			await LoadImages();
		}

		private async Task PopulateAmiiboList(int page)
		{
			int indexMult = amiibosPerPage * (page - 1);
			int indexLimit = amiibosPerPage * page;
			for (int i = indexMult, j = 0; i < indexLimit; i++)
			{
				amiibosInPage[j] = allAmiibos.Amiibo[i];

				j++;
			}
			OnAmiiboUpdate();
			await LoadImages();
		}

		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			allAmiibos = await AmiiboProcessor.LoadAmiibos();
			
			await InitiateAmiiboList();
		}

		private async void searchBtn_Click(object sender, RoutedEventArgs e)
		{
			await LoadImages(searchBox.Text);
		}

		private void pageNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsTextAllowed(e.Text);
		}

		private bool IsTextAllowed(string text)
		{
			return !numOnly.IsMatch(text);
		}

		private void OnAmiiboUpdate()
		{
			pageNumber.Text = CurrentPage.ToString();
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
		}

		private async void nextBtn_Click(object sender, RoutedEventArgs e)
		{
			int page = Convert.ToInt32(CurrentPage) + 1;
			if (page <= MaxPage)
			{
				CurrentPage = page;
				await PopulateAmiiboList(page);
			}
		}

		private async void prevBtn_Click(object sender, RoutedEventArgs e)
		{
			int page = Convert.ToInt32(CurrentPage) - 1;
			if (page > 0)
			{
				CurrentPage = page;
				await PopulateAmiiboList(page);
			}
		}
	}
}
