using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace TizenTestApp
{
	class VisualGallery : ContentPage
	{
		bool isVisible = false;
		double percentage = 0.0;

		public double Counter => percentage * 10;

		public double PercentageCounter
		{
			get { return percentage; }
			set
			{
				percentage = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Counter));
			}
		}

		Color _primary = Color.FromHex("#673AB7");
		Color _secondary = Color.FromHex("#448AFF");
		Color _sText = Color.FromHex("#757575");
		Color _darkRed = Color.FromHex("#D32F2F");
		Color _lightRed = Color.FromHex("#FFCDD2");

		Color PrimaryColor { get { return _primary; } }
		Color SecondaryColor { get { return _secondary; } }
		Color SecondaryTextColor { get { return _sText; } }
		Color DarkRedColor { get { return _darkRed; } }
		Color LightRedColor { get { return _lightRed; } }


		public VisualGallery() : base()
		{
			Title = "VisualGallery";
			Visual = VisualMarker.Material;
			BackgroundColor = Color.White;
			Content = CreateView();			
		}

		protected override void OnAppearing()
		{
			isVisible = true;

			base.OnAppearing();

			Device.StartTimer(TimeSpan.FromSeconds(1), () =>
			{
				var progress = PercentageCounter + 0.1;
				if (progress > 1)
					progress = 0;

				PercentageCounter = progress;

				return isVisible;
			});
		}

		View CreateView()
		{
			var bank = new FileImageSource();
			bank.File = "bank.png";

			var photo = new FileImageSource();
			photo.File = "photo.jpg";

			var view = new ScrollView();

			var progressbar = new ProgressBar();
			progressbar.SetBinding(ProgressBar.ProgressProperty, new Binding("PercentageCounter", source:this));

			var layout = new StackLayout
			{
				Spacing = 20,
				Padding = new Thickness(10),
				Children =
				{
					//ActivityIndicator
					new Label
					{
						Text = "Activity Indicators",
						FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
					},
					new Label //ai_1
					{
						Text = "Default",
						Margin = new Thickness(0,0,0,-10)
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new ActivityIndicator
							{
								IsRunning = true,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new ActivityIndicator
							{
								IsRunning = false,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label //ai_2
					{
						Text = "Custom Primary Color",
						Margin = new Thickness(0,0,0,-10)
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new ActivityIndicator
							{
								Color = PrimaryColor,
								IsRunning = true,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new ActivityIndicator
							{
								Color = PrimaryColor,
								IsRunning = false,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label //ai_3
					{
						Text = "Custom Background Color",
						Margin = new Thickness(0,0,0,-10)
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new ActivityIndicator
							{
								Color = SecondaryColor,
								IsRunning = true,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new ActivityIndicator
							{
								Color = SecondaryColor,
								IsRunning = false,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label //ai_4
					{
						Text = "Custom",
						Margin = new Thickness(0,0,0,-10)
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new ActivityIndicator
							{
								Color = PrimaryColor,
								BackgroundColor = SecondaryColor,
								IsRunning = true,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new ActivityIndicator
							{
								Color = PrimaryColor,
								BackgroundColor = SecondaryColor,
								IsRunning = false,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					//progress bars
					new Label
					{
						Text ="Progress Bars",
						FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
					},
					new Label //pb_1
					{
						Text ="Animating",
						Margin = new Thickness(0,0,0,-10),
					},
					progressbar,
					new Label //pb_2
					{
						Text ="At 50%",
						Margin = new Thickness(0,0,0,-10),
					},
					new ProgressBar
					{
						Progress = 0.5
					},
					new Label //pb_3
					{
						Text ="At 0%",
						Margin = new Thickness(0,0,0,-10),
					},
					new ProgressBar(),
					new Label //pb_4
					{
						Text ="Custom Primary Color",
						Margin = new Thickness(0,0,0,-10),
					},
					new ProgressBar
					{
						Progress = 0.5,
						ProgressColor = PrimaryColor
					},
					new Label //pb_5
					{
						Text ="Custom Background Color",
						Margin = new Thickness(0,0,0,-10),
					},
					new ProgressBar
					{
						Progress = 0.5,
						BackgroundColor = SecondaryColor
					},
					new Label //pb_6
					{
						Text ="Custom",
						Margin = new Thickness(0,0,0,-10),
					},
					new ProgressBar
					{
						Progress = 0.5,
						ProgressColor = PrimaryColor,
						BackgroundColor = SecondaryColor,
					},
					//Buttons
					new Label
					{
						Text ="Buttons",
						FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
					},
					new Label // btn_1
					{
						Text = "Default",
						Margin = new Thickness(0,0,0,-10),
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Button
							{
								Text = "Enabled",
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Button
							{
								IsEnabled = false,
								Text = "Disabled",
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label // btn_2
					{
						Text = "Image",
						Margin = new Thickness(0,0,0,-10),
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Button
							{
								Image = bank,
								Text = "Enabled",
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Button
							{
								Image = bank,
								IsEnabled = false,
								Text = "Disabled",
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label // btn_3
					{
						Text = "Custom Background",
						Margin = new Thickness(0,0,0,-10),
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Button
							{
								Text = "Enabled",
								BackgroundColor = PrimaryColor,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Button
							{
								IsEnabled = false,
								Text = "Disabled",
								BackgroundColor = PrimaryColor,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label // btn_4
					{
						Text = "Custom Text",
						Margin = new Thickness(0,0,0,-10),
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Button
							{
								Text = "Enabled",
								TextColor = LightRedColor,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Button
							{
								IsEnabled = false,
								Text = "Disabled",
								TextColor = LightRedColor,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label // btn_5
					{
						Text = "Custom Text & Image",
						Margin = new Thickness(0,0,0,-10),
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Button
							{
								Text = "Enabled",
								Image = bank,
								TextColor = LightRedColor,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Button
							{
								IsEnabled = false,
								Text = "Disabled",
								Image = bank,
								TextColor = LightRedColor,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label // btn_6
					{
						Text = "Custom Background & Border",
						Margin = new Thickness(0,0,0,-10),
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Button
							{
								Text = "Enabled",
								BackgroundColor = PrimaryColor,
								BorderColor = SecondaryColor,
								BorderWidth = 1,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Button
							{
								IsEnabled = false,
								Text = "Disabled",
								BackgroundColor = PrimaryColor,
								BorderColor = SecondaryColor,
								BorderWidth = 1,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					//Cards
					new Label
					{
						Text = "Cards",
						FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
					},
					new Label //card_1
					{
						Text = "Default",
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
									TextColor = SecondaryTextColor,
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
					new Label //card_2
					{
						Text = "Default (No Shadow)",
						Margin = new Thickness(0,0,0,-10)
					},
					new Frame
					{
						Padding = 16,
						HasShadow = false,
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
									TextColor = SecondaryTextColor,
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
					new Label //card_3
					{
						Text = "Default With Border",
						Margin = new Thickness(0,0,0,-10)
					},
					new Frame
					{
						Padding = 16,
						BorderColor = DarkRedColor,
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
									TextColor = SecondaryTextColor,
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
					new Label //card_4
					{
						Text = "Default With Border (No Shadow)",
						Margin = new Thickness(0,0,0,-10)
					},
					new Frame
					{
						Padding = 16,
						BorderColor = DarkRedColor,
						HasShadow = false,
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
									TextColor = SecondaryTextColor,
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
					new Label //card_5
					{
						Text = "No Card Padding & Thin Content Padding",
						Margin = new Thickness(0,0,0,-10)
					},
					new Frame
					{
						Padding = 0,
						BorderColor = DarkRedColor,
						CornerRadius = 10,
						Content = new StackLayout
						{
							Spacing = 16,
							BackgroundColor = LightRedColor,
							Padding = 8,
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
									TextColor = SecondaryTextColor,
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
					new Label //card_6
					{
						Text = "No Padding / Spacing",
						Margin = new Thickness(0,0,0,-10)
					},
					new Frame
					{
						BackgroundColor = LightRedColor,
						Margin = new Thickness(0),
						Padding = 0,
						HasShadow = true,
						CornerRadius = 10,
						BorderColor = DarkRedColor,
						Content = new StackLayout
						{
							Spacing = 16,
							Margin = new Thickness(0),
							Padding = 0,
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
									TextColor = SecondaryTextColor,
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
					//Entry
					new Label
					{
						Text = "Entries",
						FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
					},
					new Label // entry_1
					{
						Text = "Default",
						Margin = new Thickness(0,0,0,-10)
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Entry
							{
								Placeholder = "Default Entry",
								Text = "With Text",
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Entry
							{
								Placeholder = "Default Entry",
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label // entry_2
					{
						Text = "Placeholder Color Variations",
						Margin = new Thickness(0,0,0,-10)
					},
					new StackLayout
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Entry
							{
								Placeholder = "Green Placeholder",
								PlaceholderColor = Color.Green,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Entry
							{
								Placeholder = "Purple Placeholder",
								PlaceholderColor = Color.Purple,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new Label
					{
						Text = "Text Color Variations",
						Margin = new Thickness(0,0,0,-10)
					},
					new StackLayout //entry_3
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Entry
							{
								Text = "Green Text Color",
								TextColor = Color.Green,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Entry
							{
								Text = "Purple Text Color",
								TextColor = Color.Purple,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new StackLayout // entry_4
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Entry
							{
								Placeholder = "Green Text/Red Placeholder",
								TextColor = Color.Green,
								PlaceholderColor = Color.Red,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Entry
							{
								Placeholder = "Purple Text/Purple Placeholder",
								TextColor = Color.Purple,
								PlaceholderColor = Color.Purple,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					new StackLayout // entry_5
					{
						Orientation = StackOrientation.Horizontal,
						Children =
						{
							new Entry
							{
								Text = "Yellow Background Color",
								BackgroundColor = Color.Yellow,
								HorizontalOptions = LayoutOptions.FillAndExpand
							},
							new Entry
							{
								Text = "Cyan Background Color",
								BackgroundColor = Color.Cyan,
								HorizontalOptions = LayoutOptions.FillAndExpand
							}
						}
					},
					//Entry
					new Label
					{
						Text = "Sliders",
						FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
					},
					new Label //sl_1
					{
						Text = "Default",
						Margin = new Thickness(0,0,0,-10)
					},
					new Slider
					{
						Minimum = -100,
						Maximum = 100
					},
					new Label //sl_2
					{
						Text = "Default (Disabled)",
						Margin = new Thickness(0,0,0,-10)
					},
					new Slider
					{
						Minimum = -100,
						Maximum = 100,
						IsEnabled = false
					},
					new Label //sl_3
					{
						Text = "Custom Primary Color",
						Margin = new Thickness(0,0,0,-10)
					},
					new Slider
					{
						Minimum = -100,
						Maximum = 100,
						ThumbColor = PrimaryColor,
						MinimumTrackColor = PrimaryColor
					},
					new Label //sl_4
					{
						Text = "Custom Primary Color (Disabled)",
						Margin = new Thickness(0,0,0,-10)
					},
					new Slider
					{
						Minimum = -100,
						Maximum = 100,
						IsEnabled= false,
						ThumbColor = PrimaryColor,
						MaximumTrackColor = PrimaryColor,
						MinimumTrackColor = PrimaryColor
					},
					new Label //sl_5
					{
						Text = "Custom",
						Margin = new Thickness(0,0,0,-10)
					},
					new Slider
					{
						Minimum = -100,
						Maximum = 100,
						ThumbColor = DarkRedColor,
						MaximumTrackColor = SecondaryColor,
						MinimumTrackColor = PrimaryColor
					},
					new Label //sl_6
					{
						Text = "Custom(Disabled)",
						Margin = new Thickness(0,0,0,-10)
					},
					new Slider
					{
						Minimum = -100,
						Maximum = 100,
						IsEnabled = false,
						ThumbColor = DarkRedColor,
						MaximumTrackColor = SecondaryColor,
						MinimumTrackColor = PrimaryColor
					},
				} //children
			}; //stacklayout

			view.Content = layout;
			return view;
		}
	}
}
