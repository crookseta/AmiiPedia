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
		const int amiibosPerPage = 90;

		string? lastSearch;

		public MainWindow()
		{
			InitializeComponent();
			Helper.InitializeClient();
			
			NotSearching = true;
		}
		#region Methods
		/// <summary>
		/// Initiates amiibosInPage and AmiiboImages the first time its called.
		/// Clears the ImagesPanel's children and AmiiboImages.
		/// Calls the LoadImages method passing amiibosInPage.Length and amiibosInPage
		/// </summary>
		/// <returns></returns>
		private async Task LoadImageArray()
		{
			if(amiibosInPage == null)
			{
				amiibosInPage = new Amiibo[amiibosPerPage];
				AmiiboImages = new List<Image>(amiibosInPage.Length);
			}

			ImagesPanel.Children.Clear();
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
		private async Task LoadImageArray(string name, Amiibo.Parameter parameter)
		{
			if (lastSearch == name)
			{
				return;
			}
			if(!allAmiibos.Contains(name, parameter))
			{
				return;
			}

			NotSearching = false;

			if (SearchedAmiibos != null)
			{
				SearchedAmiibos.Clear();
			}
			lastSearch = name;
			ImagesPanel.Children.Clear();
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
			/*
			for (int i = 0; i < allAmiibos.Length(); i++)
			{
				if (allAmiibos.Amiibos[i].GameSeries != name)
				{
					
					continue;
				}
				SearchedAmiibos?.Add(allAmiibos.Amiibos[i]);
			}
			*/
			await LoadImages(SearchedAmiibos.Count, Amiibos);
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
				if(source[i] == null)
				{
					break;
				}

				AmiiboImages.Add(GetAmiiboImageTemplate());
				/*
				AmiiboImages[i].Height = ImageTemplateParameters().height;
				AmiiboImages[i].Width = ImageTemplateParameters().width;
				AmiiboImages[i].MinHeight = ImageTemplateParameters().minheight;
				AmiiboImages[i].MinWidth = ImageTemplateParameters().minwidth;
				AmiiboImages[i].Margin = ImageTemplateParameters().margin;
				*/
				AmiiboImages[i].Source = new BitmapImage(
					  new Uri(source[i].Image, UriKind.Absolute)
					  );
				AmiiboImages[i].ToolTip = source[i].Name;

				ImagesPanel.Children.Add(AmiiboImages[i]);
			}
		}

		private (float width, float height, float minwidth, float minheight, Thickness margin) ImageTemplateParameters()
		{
			float width = 50f, height = 90, minwidth = 50, minheight = 80;
			Thickness margin = new Thickness(20, 20, 20, 20);

			return (width, height, minwidth, minheight, margin);
		}
		/// <summary>
		/// Populates the amiibos list for the first time or when going back to the home page
		/// </summary>
		/// <returns></returns>
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
		/// <summary>
		/// Populates the amiibo list with the amiibos that would be shown in the specified page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
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

		private Image GetAmiiboImageTemplate()
		{
			Image temp = new Image();

			temp.Height = amiiboImagesTemplate.Height;
			temp.MinHeight = amiiboImagesTemplate.MinHeight;
			temp.Width = amiiboImagesTemplate.Width;
			temp.MinWidth = amiiboImagesTemplate.MinWidth;
			temp.Margin = amiiboImagesTemplate.Margin;

			temp.MouseEnter += amiiboImages_MouseEnter;
			temp.MouseLeave += amiiboImages_MouseLeave;

			return temp;
		}

		private bool IsTextAllowed(string text)
		{
			return !numOnly.IsMatch(text);
		}
		/// <summary>
		/// Its called whenever the list of amiibos is changed
		/// </summary>
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

		void SearchButtonsVisibility(bool visible)
		{
			searchBtn.IsEnabled = visible;
			searchBox.IsEnabled = visible;
			
		}
		#endregion
		//Event for the WPF components
		#region WPF Events
		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			allAmiibos = await AmiiboProcessor.LoadAmiibos();
			
			await InitiateAmiiboList();

			homeBtn.IsEnabled = true;
			PageButtonsVisibility(true);
			SectionButtonsVisibility(true);
			SearchButtonsVisibility(true);
		}
		
		private async void searchBtn_Click(object sender, RoutedEventArgs e)
		{
			await LoadImageArray(searchBox.Text, Amiibo.Parameter.Character);
		}

		private async void searchBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				await LoadImageArray(searchBox.Text, Amiibo.Parameter.Character);
			}
		}

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
					await LoadImageArray(i.Content.ToString(), Amiibo.Parameter.GameSeries);
					await PopulateAmiiboList(1);
					break;
				}
			}
			OnAmiiboUpdate();
		}

		private void amiiboImages_MouseEnter(object sender, MouseEventArgs e)
		{
			foreach(Image i in ImagesPanel.Children)
			{
				if(e.Source == i)
				{
					i.OpacityMask = amiiboImagesTemplate.OpacityMask;

					break;
				}
			}
		}

		private void amiiboImages_MouseLeave(object sender, MouseEventArgs e)
		{
			foreach (Image i in ImagesPanel.Children)
			{
				if (e.Source == i)
				{
					i.OpacityMask = null;

					break;
				}
			}
		}

		#endregion

		
	}
}
