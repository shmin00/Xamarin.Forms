using System;
using ElmSharp;
using EColor = ElmSharp.Color;
using ELayout = ElmSharp.Layout;

namespace Xamarin.Forms.Platform.Tizen.Native
{
	public class MaterialEntry : EditfieldEntry
	{
		ELayout _layout;

		public MaterialEntry(EvasObject parent) : base(parent, "material")
		{
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				if(!string.IsNullOrEmpty(value))
				{
					_layout.SignalEmit("elm,state,activated", "");
				}
			}
		}

		public override string Placeholder
		{
			get
			{
				return base.Placeholder;
			}
			set
			{
				base.Placeholder = value;
				_layout.SetPartText("elm.text.label", value);
			}
		}

		public override EColor TextColor
		{
			get
			{
				return base.TextColor;
			}
			set
			{
				base.TextColor = value;
				SetPartColor("cursor", value);
				_layout.SetPartColor("underline_focused", value);
			}
		}

		public override EColor PlaceholderColor
		{
			get
			{
				return base.PlaceholderColor;
			}
			set
			{
				base.PlaceholderColor = value;
				if(PlaceholderColor.IsDefault)
				{
					base.PlaceholderColor = TextColor;
				}

				_layout.SetPartColor("label", PlaceholderColor);
			}
		}

		protected override ElmSharp.Layout CreateEditFieldLayout(EvasObject parent)
		{
			_layout = base.CreateEditFieldLayout(parent);

			_layout.Focused += OnFocused;
			_layout.Unfocused += OnUnfocused;

			return _layout;
		}

		void OnFocused(object sender, EventArgs args)
		{
			if (string.IsNullOrEmpty(Text))
			{
				_layout.SignalEmit("elm,state,activated", "");
			}
			SetFocusOnTextBlock(true);
		}

		void OnUnfocused(object sender, EventArgs args)
		{
			if (!string.IsNullOrEmpty(Text))
			{
				_layout.SignalEmit("elm,state,activated", "");
			}
		}
	}
}