using System;
using ElmSharp;
using Tizen.NET.MaterialComponents;
using Xamarin.Forms.Platform.Tizen.Native;
using EColor = ElmSharp.Color;
using ESize = ElmSharp.Size;
using NSpan = Xamarin.Forms.Platform.Tizen.Native.Span;
using NImage = Xamarin.Forms.Platform.Tizen.Native.Image;
using NTextAlignment = Xamarin.Forms.Platform.Tizen.Native.TextAlignment;
using TButtonStyle = Xamarin.Forms.PlatformConfiguration.TizenSpecific.ButtonStyle;

namespace Xamarin.Forms.Material.Tizen.Native
{
	public class MButton : global::Tizen.NET.MaterialComponents.MButton, IMeasurable, IBatchable, IButton
	{
		/// <summary>
		/// Holds the formatted text of the button.
		/// </summary>
		readonly NSpan _span = new NSpan();

		/// <summary>
		/// Optional image, if set will be drawn on the button.
		/// </summary>
		NImage _image;

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Forms.Platform.Tizen.Native.Button"/> class.
		/// </summary>
		/// <param name="parent">Parent evas object.</param>
		public MButton(EvasObject parent) : base(parent)
		{
		}

		/// <summary>
		/// Gets or sets the button's text.
		/// </summary>
		/// <value>The text.</value>
		public override string Text
		{
			get
			{
				return _span.Text;
			}

			set
			{
				if (value != _span.Text)
				{
					_span.Text = value;
					ApplyTextAndStyle();
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the text.
		/// </summary>
		/// <value>The color of the text.</value>
		public EColor TextColor
		{
			get
			{
				return _span.ForegroundColor;
			}

			set
			{
				if (!_span.ForegroundColor.Equals(value))
				{
					_span.ForegroundColor = value;
					ApplyTextAndStyle();
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the text background.
		/// </summary>
		/// <value>The color of the text background.</value>
		public EColor TextBackgroundColor
		{
			get
			{
				return _span.BackgroundColor;
			}

			set
			{
				if (!_span.BackgroundColor.Equals(value))
				{
					_span.BackgroundColor = value;
					ApplyTextAndStyle();
				}
			}
		}

		/// <summary>
		/// Gets or sets the font family.
		/// </summary>
		/// <value>The font family.</value>
		public string FontFamily
		{
			get
			{
				return _span.FontFamily;
			}

			set
			{
				if (value != _span.FontFamily)
				{
					_span.FontFamily = value;
					ApplyTextAndStyle();
				}
			}
		}

		/// <summary>
		/// Gets or sets the font attributes.
		/// </summary>
		/// <value>The font attributes.</value>
		public FontAttributes FontAttributes
		{
			get
			{
				return _span.FontAttributes;
			}

			set
			{
				if (value != _span.FontAttributes)
				{
					_span.FontAttributes = value;
					ApplyTextAndStyle();
				}
			}
		}

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>The size of the font.</value>
		public double FontSize
		{
			get
			{
				return _span.FontSize;
			}

			set
			{
				if (value != _span.FontSize)
				{
					_span.FontSize = value;
					ApplyTextAndStyle();
				}
			}
		}

		/// <summary>
		/// Gets or sets the image to be displayed next to the button's text.
		/// </summary>
		/// <value>The image displayed on the button.</value>
		public NImage Image
		{
			get
			{
				return _image;
			}

			set
			{
				_image = value;
				Icon = _image;
			}
		}

		/// <summary>
		/// Implementation of the IMeasurable.Measure() method.
		/// </summary>
		public virtual ESize Measure(int availableWidth, int availableHeight)
		{
			if (Style == TButtonStyle.Circle)
			{
				return new ESize(MinimumWidth, MinimumHeight);
			}
			else
			{
				if (Image != null)
					MinimumWidth += Image.Geometry.Width;

				var rawSize = TextHelper.GetRawTextBlockSize(this);
				return new ESize(rawSize.Width + MinimumWidth, Math.Max(MinimumHeight, rawSize.Height));
			}
		}

		void IBatchable.OnBatchCommitted()
		{
			ApplyTextAndStyle();
		}

		/// <summary>
		/// Applies the button's text and its style.
		/// </summary>
		void ApplyTextAndStyle()
		{
			if (!this.IsBatched())
			{
				SetInternalTextAndStyle(_span.GetDecoratedText(), _span.GetStyle());
			}
		}

		/// <summary>
		/// Sets the button's internal text and its style.
		/// </summary>
		/// <param name="formattedText">Formatted text, supports HTML tags.</param>
		/// <param name="textStyle">Style applied to the formattedText.</param>
		void SetInternalTextAndStyle(string formattedText, string textStyle)
		{
			string emission = "elm,state,text,visible";

			if (string.IsNullOrEmpty(formattedText))
			{
				formattedText = null;
				textStyle = null;
				emission = "elm,state,text,hidden";
			}

			base.Text = formattedText;

			var textblock = EdjeObject["elm.text"];

			if (textblock != null)
			{
				textblock.TextStyle = textStyle;
			}

			EdjeObject.EmitSignal(emission, "elm");
		}

		/// <summary>
		/// Update the button's style
		/// </summary>
		/// <param name="style">The style of button</param>
		public void UpdateStyle(string style)
		{
			if (Style != style)
			{
				Style = style;
				if (Style == TButtonStyle.Default)
					_span.HorizontalTextAlignment = NTextAlignment.Auto;
				else
					_span.HorizontalTextAlignment = NTextAlignment.Center;
				ApplyTextAndStyle();
			}
		}
	}
}
