using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace TizenTestApp
{
	class MainPage : ContentPage
	{

		public MainPage()
		{
			var list = new List<string>();
			list.Add("DefaultPage");
			list.Add("MaterialStylePage");
			list.Add("VisualGallery");
			list.Add("DefaultViewGallery");
			list.Add("TestPage");
			list.Add("TestPage2");

			var listView = new ListView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemsSource = list
			};

			listView.ItemSelected += async (s, args) =>
			{
				var tc = args.SelectedItem as string;
				tc = "TizenTestApp." + tc;
				Console.WriteLine("tc: " + tc);

				Assembly asm = Assembly.GetExecutingAssembly();
				var page = asm.CreateInstance(tc) as Page;

				if (page != null)
					await Navigation.PushAsync(page);
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
