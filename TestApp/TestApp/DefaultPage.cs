using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace TestApp
{
	public class DefaultPage : ContentPage
	{

		bool isVisible = false;
		ProgressBar progressBar = new ProgressBar();

		public DefaultPage()
		{
			Title = "DefaultPage";
			BackgroundColor = Color.White;

			var fileImageSource = new FileImageSource();
			fileImageSource.File = "icon_home.png";

			var button = new Button
			{
				Text = "learn more",
				Image = fileImageSource
			};

			button.Clicked += (s, e) =>
			{
				Console.WriteLine(" ************* clicked");
			};

			var frame = new Frame
			{
				HasShadow = true,
				CornerRadius = 0,
				Margin = 10,
				Padding = 0,
				Content = new StackLayout
				{
					BackgroundColor = Color.White,
					Padding = 20,
					Spacing = 20,
					Children =
					{
						new Image
						{
							Source = ImageSource.FromFile("photo.jpg"),
							Aspect = Aspect.AspectFit,
						},
						new Label
						{
							Text = "Walk below the arches",
							Margin = new Thickness(0, 10, 0, 0),
							FontSize = 28,
							TextColor = Color.Black
						},
						new Label
						{
							Text="Discover modern interpretations of traditional architectural styles.",
							TextColor = Color.DarkGray,
							FontSize = 18
						},
						button
					}
				}
			};

			var entry1 = new Entry
			{
				Placeholder = "Title",
				PlaceholderColor = Color.Green,
				Text = "Vintage Dress",
				BackgroundColor = Color.LightGray, //#ddd
				TextColor = Color.Green,
			};

			var entry2 = new Entry
			{
				Placeholder = "Price",
				PlaceholderColor = Color.Purple,
				Text = "$10",
				BackgroundColor = Color.LightGray, //#ddd
				TextColor = Color.Purple
			};

			var entry3 = new Entry
			{
				Placeholder = "Location",
				PlaceholderColor = Color.Black,
				BackgroundColor = Color.LightGray, //#ddd
				TextColor = Color.Black
			};
			FlexLayout.SetGrow(entry1, 1);
			FlexLayout.SetBasis(entry2, new FlexBasis(0.3f, true));
			FlexLayout.SetBasis(entry3, new FlexBasis(0.6f, true));

			var flexLayout = new FlexLayout
			{
				Direction = FlexDirection.Row,
				Margin = 10,
				Wrap = FlexWrap.Wrap,
				JustifyContent = FlexJustify.SpaceBetween,
				Children =
				{
					entry1,
					entry2,
					entry3,
				}
			};

			Content = new ScrollView
			{
				Content = new StackLayout
				{
					Spacing = 20,
					Children =
					{
						progressBar,
						frame,
						flexLayout
					}
				}
			};
		}

		protected override void OnAppearing()
		{
			isVisible = true;
			base.OnAppearing();
			Device.StartTimer(TimeSpan.FromSeconds(1), () =>
			{
				var progress = progressBar.Progress + 0.1;
				if (progress > 1)
					progress = 0;

				progressBar.Progress = progress;
				return isVisible;
			});
		}

		protected override void OnDisappearing()
		{
			isVisible = false;
			base.OnDisappearing();
		}

	}
}
