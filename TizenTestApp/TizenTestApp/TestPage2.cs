using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TizenTestApp
{
	class TestPage2 : ContentPage
	{
		public TestPage2()
		{
			Title = "Card22";
			Visual = VisualMarker.Material;
			BackgroundColor = Color.White;

			var photo = new FileImageSource();
			photo.File = "photo.jpg";

			Padding = 16;

			Content = new StackLayout
			{
				Margin = 20,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children =
				{
					new Label //card_3
					{

						Margin = new Thickness(0,0,0,-10)
					},
					new Frame
					{
						Padding = 16,
						Content = new StackLayout
						{
							Spacing = 16,
							Children =
							{
								new Image
								{
									Source = photo,
									Margin = new Thickness(-16, -16, -16, 0),
									Aspect = Aspect.AspectFit
								},
								new Label
								{
									Text = "Walk below the arches",
									FontSize = 24
								},
								new Label
								{
									Text = "Card containers hold all card elements, and their size is determined by the space those elements occupy. Card elevation is expressed by the container.",
									TextColor = Color.FromHex("#757575"),
									FontSize = 14
								},
								new Button
								{
									Text = "more",
									HorizontalOptions =LayoutOptions.End
								}
							}
						}
					},
				}
			};

		}
	}
}
