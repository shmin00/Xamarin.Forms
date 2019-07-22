using System;
using ElmSharp;
using Tizen.NET.MaterialComponents;
using Xamarin.Forms.Platform.Tizen.Native;

namespace Xamarin.Forms.Material.Tizen.Native
{
	public class MPicker : MaterialEntry
	{
		bool _isTexstBlockFocused = false;

		public MPicker(EvasObject parent) : base(parent)
		{
			Initialize();
		}

		protected override void OnFocused(object sender, EventArgs args)
		{
			Layout.RaiseTop();
			Layout.SignalEmit(States.Focused, "");
		}

		protected override void OnUnfocused(object sender, EventArgs args)
		{
			Layout.SignalEmit(States.Unfocused, "");
		}

		protected override void OnLayoutFocused(object sender, EventArgs args)
		{
			AllowFocus(false);
			Layout.SignalEmit(States.Focused, "");
		}

		protected override void OnLayoutUnFocused(object sender, EventArgs args)
		{
			SetFocusOnTextBlock(false);
			Layout.SignalEmit(States.Unfocused, "");
		}

		void Initialize()
		{
			IsSingleLine = true;
			InputPanelShowByOnDemand = true;
			HorizontalTextAlignment = Platform.Tizen.Native.TextAlignment.Center;
			SetVerticalTextAlignment(Parts.Entry.TextEdit, 0.5);

			Layout.KeyDown += (s, e) =>
			{
				if (e.KeyName == "Return")
				{
					if (!_isTexstBlockFocused)
					{
						SetFocusOnTextBlock(true);
						e.Flags |= EvasEventFlag.OnHold;
					}
				}
			};

			Clicked += (s, e) => SetFocusOnTextBlock(true);
		}

		void SetFocusOnTextBlock(bool isFocused)
		{
			AllowFocus(isFocused);
			SetFocus(isFocused);
			_isTexstBlockFocused = isFocused;

			if (isFocused)
				InvokeTextBlockFocused();
			else
				InvokeTextBlcokUnfocused();
		}
	}
}
