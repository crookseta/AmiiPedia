using AmiiPedia.Models;
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
	/// Lógica de interacción para AmiiboDirectoryPage.xaml
	/// </summary>
	public partial class AmiiboDirectoryPage : Page
	{
		

		private int MaxPage 
		{
			get
			{
				var c = amiiboList.Count;
				var i = Math.Round((double)(Convert.ToDouble(c) / Convert.ToDouble(amiibosPerPage)), 0, MidpointRounding.ToPositiveInfinity);
				return Convert.ToInt32(i);
			}
		}
		private int CurrentPage = 1;
		private List<Image> amiiboImages = new List<Image>();
		private List<Amiibo?> amiiboList;
		private Amiibo?[] amiibosInPage = new Amiibo[amiibosPerPage];
		private const int amiibosPerPage = 90;
		private readonly Regex numOnly = new Regex("[^0-9]+");

		public AmiiboDirectoryPage()
		{
			InitializeComponent();
			amiiboList =  new List<Amiibo?>();
		}
		public AmiiboDirectoryPage(List<Amiibo> amiibos)
		{
			InitializeComponent();
			amiiboList = amiibos;
		}

		#region Methods
		private async Task Start()
		{
			await InitiateAmiiboList();

			CurrentPage = 1;
			Update();
		}
		public async Task PopulateAmiiboList(int page)
		{
			int indexMult = amiibosPerPage * (page - 1);
			int indexLimit = amiibosPerPage * page;

			for (int i = indexMult, j = 0; i < indexLimit; i++)
			{
				if (i >= amiiboList.Count)
				{
					amiibosInPage[j] = null;
					continue;
				}

				amiibosInPage[j] = amiiboList[i];

				j++;
			}

			Update();
			await LoadImageArray();
		}
		/// <summary>
		/// Populates the amiibos list for the first time or when going back to the home page
		/// </summary>
		/// <returns></returns>
		public async Task InitiateAmiiboList()
		{
			for(int i = 0, j = 0; i < amiibosPerPage; i++)
			{
				if(i >= amiiboList.Count)
				{
					break;
				}

				amiibosInPage[j] = amiiboList[i];

				j++;
			}
			Update();
			await LoadImageArray();
		}
		/// <summary>
		/// Initiates amiibosInPage and AmiiboImages the first time its called.
		/// Clears the ImagesPanel's children and AmiiboImages.
		/// Calls the LoadImages method passing amiibosInPage.Length and amiibosInPage
		/// </summary>
		/// <returns></returns>
		public async Task LoadImageArray()
		{
			if (amiibosInPage == null)
			{
				amiibosInPage = new Amiibo[amiibosPerPage];
				amiiboImages = new List<Image>(amiibosInPage.Length);
			}

			ImagesPanel.Children.Clear();
			amiiboImages.Clear();

			await LoadImages(amiibosInPage.Length, amiibosInPage);
		}
		/// <summary>
		/// Adds images from <paramref name="source"/> to the ImagesPanel
		/// </summary>
		/// <param name="count">Max amount of images added</param>
		/// <param name="source">Source of the images</param>
		/// <returns></returns>
		public async Task LoadImages(int count, IList<Amiibo> source)
		{
			await Task.Run(() => {
				this.Dispatcher.Invoke(() =>
				{
					for (int i = 0; i < count; i++)
					{
						if (source[i] is null)
						{
							break;
						}

						amiiboImages.Add(GetAmiiboImageTemplate());

						amiiboImages[i].Source = new BitmapImage(
							new Uri(source[i].Image, UriKind.Absolute)
							);
						amiiboImages[i].ToolTip = source[i].Name;
						amiiboImages[i].Tag = source[i];

						ImagesPanel.Children.Add(amiiboImages[i]);
					}
				});
			});
		}

		public void Update()
		{
			pageNumber.Text = CurrentPage.ToString();

			if(amiiboList.Count <= amiibosPerPage)
			{
				PageButtonsVisibility(false);
			}
			else
			{
				PageButtonsVisibility(true);
			}

			
		}

		private Image GetAmiiboImageTemplate()
		{
			return Application.Current.Dispatcher.Invoke(
				  new Func<Image>(() =>
					{
						Image t = new Image();

						t.Height = amiiboImagesTemplate.Height;
						t.MinHeight = amiiboImagesTemplate.MinHeight;
						t.Width = amiiboImagesTemplate.Width;
						t.MinWidth = amiiboImagesTemplate.MinWidth;
						t.Margin = amiiboImagesTemplate.Margin;

						t.MouseEnter += amiiboImages_MouseEnter;
						t.MouseLeave += amiiboImages_MouseLeave;
						t.MouseLeftButtonDown += amiiboImages_MouseLeftButtonDown;

						t.Cursor = Cursors.Hand;

						return t;
					})
			);
		}

		private bool IsTextAllowed(string text)
		{
			return !numOnly.IsMatch(text);
		}

		private void PageButtonsVisibility(bool visible)
		{
			if (visible == false)
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
		#endregion

		#region WPF events
		private async void Page_Loaded(object sender, RoutedEventArgs e)
		{
			await Start();
		}
		private void Page_Unloaded(object sender, RoutedEventArgs e)
		{

		}

		private void amiiboImages_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			foreach (Image i in ImagesPanel.Children)
			{
				if (e.Source == i)
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						MainWindow.Instance.mainFrame.Content = new AmiiboInfoPage((Amiibo)i.Tag);
					});
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

		private void amiiboImages_MouseEnter(object sender, MouseEventArgs e)
		{
			foreach (Image i in ImagesPanel.Children)
			{
				if (e.Source == i)
				{
					i.OpacityMask = amiiboImagesTemplate.OpacityMask;

					break;
				}
			}
		}

		private async void prevBtn_Click(object sender, RoutedEventArgs e)
		{
			if(CurrentPage > 1)
			{
				int page = Convert.ToInt32(CurrentPage) - 1;
				CurrentPage = page;
				await PopulateAmiiboList(page);
				Update();
			}
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
				Update();
			}
		}

		private void pageNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsTextAllowed(e.Text);
		}

		private async void nextBtn_Click(object sender, RoutedEventArgs e)
		{
			if(CurrentPage < MaxPage)
			{
				int page = Convert.ToInt32(CurrentPage) + 1;
				CurrentPage = page;
				await PopulateAmiiboList(page);
				Update();
			}
		}
		#endregion

		
	}
}
