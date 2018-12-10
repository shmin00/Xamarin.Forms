using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TizenTestApp
{
	class TestPage : ContentPage
	{
		public TestPage()
		{
			Title = "VisualGallery";
			Visual = VisualMarker.Material;
			BackgroundColor = Color.White;


			var entry = new Entry
			{
				Text ="text",
				Placeholder="placeholder",
				BackgroundColor = Color.Blue,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			var entry2 = new Entry
			{
				Placeholder = "placeholder",
				BackgroundColor = Color.Green,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			var entry3 = new Entry
			{
				BackgroundColor = Color.Yellow,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			var tBtn = new Button
			{
				Text = "Text",
				BackgroundColor = Color.FromHex("#673AB7")
			};
			tBtn.Clicked += (s, e) =>
			{
				entry.TextColor = entry.TextColor.IsDefault ? Color.Red : Color.Default;
			};

			var pBtn = new Button
			{
				Text = "Placeholder"
			};
			pBtn.Clicked += (s, e) =>
			{
				entry.PlaceholderColor = entry.PlaceholderColor.IsDefault ? Color.Blue : Color.Default;
			};

			var bBtn = new Button
			{
				Text = "Background"
			};
			bBtn.Clicked += (s, e) =>
			{
				entry.BackgroundColor = entry.BackgroundColor.IsDefault ? Color.Yellow : Color.Default;
			};

			var textBtn = new Button
			{
				Text = "Text aaa"
			};
			textBtn.Clicked += (s, e) =>
			{
				if (String.IsNullOrEmpty(entry.Text))
				{
					entry.Text = "aaa";
				}					
				else
				{
					entry.Text = string.Empty;
				}
			};

			Content = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children =
				{
					new Label
					{
						BackgroundColor = Color.LightGray,
						HeightRequest = 100
					},
					entry,
					//entry2,
					//entry3,
					tBtn,
					pBtn,
					bBtn,
					textBtn
				}
			};

		}
	}
}
