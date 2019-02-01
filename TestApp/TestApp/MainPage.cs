using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp
{
	class MainPage : ContentPage
	{

		public MainPage()
		{
			var list = new List<string>();
			list.Add("DefaultPage");
			list.Add("MaterialStylePage");

			var listView = new ListView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemsSource = list
			};

			listView.ItemSelected += async (s, args) =>
			{
				var tc = args.SelectedItem as string;
				tc = "TestApp." + tc;
				Console.WriteLine("tc: " + tc);

				Assembly asm = Assembly.GetExecutingAssembly();
				var page = asm.CreateInstance(tc) as Page;
				var naviPage = Application.Current.MainPage as NavigationPage;

				if (page != null && naviPage != null)
					await naviPage.PushAsync(page);

			};

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
				Children =
				{
					new Label
					{
						Text = "Test",
						VerticalOptions = LayoutOptions.Start,
						HorizontalOptions = LayoutOptions.Start
					},
					listView
				}
			};
		}

	}
}
