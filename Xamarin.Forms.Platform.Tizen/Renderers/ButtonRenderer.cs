using System;
using Xamarin.Forms.Platform.Tizen.Native;
using EButton = ElmSharp.Button;
using Specific = Xamarin.Forms.PlatformConfiguration.TizenSpecific.VisualElement;

namespace Xamarin.Forms.Platform.Tizen
{
	public class ButtonRenderer : ViewRenderer<Button, EButton>
	{
		public ButtonRenderer()
		{
			RegisterPropertyHandler(Button.TextProperty, UpdateText);
			RegisterPropertyHandler(Button.FontFamilyProperty, UpdateFontFamily);
			RegisterPropertyHandler(Button.FontSizeProperty, UpdateFontSize);
			RegisterPropertyHandler(Button.FontAttributesProperty, UpdateFontAttributes);
			RegisterPropertyHandler(Button.TextColorProperty, UpdateTextColor);
			RegisterPropertyHandler(Button.ImageProperty, UpdateBitmap);
			RegisterPropertyHandler(Button.BorderColorProperty, UpdateBorder);
			RegisterPropertyHandler(Button.CornerRadiusProperty, UpdateBorder);
			RegisterPropertyHandler(Button.BorderWidthProperty, UpdateBorder);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			if (Control == null)
			{
				SetNativeControl(CreateNativeControl());

				Control.Clicked += OnButtonClicked;
				Control.Pressed += OnButtonPressed;
				Control.Released += OnButtonReleased;
			}
			base.OnElementChanged(e);
		}

		protected virtual EButton CreateNativeControl()
		{
			if (Device.Idiom == TargetIdiom.Watch)
				return new Native.Watch.WatchButton(Forms.NativeParent);
			else
				return new Native.Button(Forms.NativeParent);
		}

		protected override Size MinimumSize()
		{
			return (Control as IMeasurable).Measure(Control.MinimumWidth, Control.MinimumHeight).ToDP();
		}

		protected override void UpdateThemeStyle()
		{
			var style = Specific.GetStyle(Element);
			if (!string.IsNullOrEmpty(style))
			{
				(Control as IButton).UpdateStyle(style);
				((IVisualElementController)Element).NativeSizeChanged();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Control != null)
				{
					Control.Clicked -= OnButtonClicked;
					Control.Pressed -= OnButtonPressed;
					Control.Released -= OnButtonReleased;
				}
			}
			base.Dispose(disposing);
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			(Element as IButtonController)?.SendClicked();
		}

		void OnButtonPressed(object sender, EventArgs e)
		{
			(Element as IButtonController)?.SendPressed();
		}

		void OnButtonReleased(object sender, EventArgs e)
		{
			(Element as IButtonController)?.SendReleased();
		}

		void UpdateText()
		{
			(Control as IButton).Text = Element.Text ?? "";
		}

		void UpdateFontSize()
		{
			(Control as IButton).FontSize = Element.FontSize;
		}

		void UpdateFontAttributes()
		{
			(Control as IButton).FontAttributes = Element.FontAttributes;
		}

		void UpdateFontFamily()
		{
			(Control as IButton).FontFamily = Element.FontFamily;
		}

		void UpdateTextColor()
		{
			(Control as IButton).TextColor = Element.TextColor.ToNative();
		}

		void UpdateBitmap()
		{
			if (!string.IsNullOrEmpty(Element.Image))
			{
				(Control as IButton).Image = new Native.Image(Control);
				var task = (Control as IButton).Image.LoadFromImageSourceAsync(Element.Image);
			}
			else
			{
				(Control as IButton).Image = null;
			}
		}

		void UpdateBorder()
		{
			/* The simpler way is to create some specialized theme for button in
			 * tizen-theme
			 */
			// TODO: implement border handling
		}
	}
}
