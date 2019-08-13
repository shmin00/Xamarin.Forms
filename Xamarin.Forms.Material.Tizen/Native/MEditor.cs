using System;
using ElmSharp;
using Tizen.NET.MaterialComponents;
using Xamarin.Forms.Platform.Tizen.Native;

namespace Xamarin.Forms.Material.Tizen.Native
{
	public class MEditor : MaterialEntry
	{
		bool _isTexstBlockFocused = false;
		int _heightPadding = 0;

		public MEditor(EvasObject parent) : base(parent)
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
			_isTexstBlockFocused = false;
		}

		protected override void OnLayoutFocused(object sender, EventArgs args)
		{
			AllowFocus(false);
			Layout.SignalEmit(States.Focused, "");
		}

		void Initialize()
		{
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


		public override ElmSharp.Size Measure(int availableWidth, int availableHeight)
		{
			var textBlockSize = base.Measure(availableWidth, availableHeight);

			textBlockSize.Width += Layout.MinimumWidth;

			if (textBlockSize.Height < Layout.MinimumHeight)
				textBlockSize.Height = Layout.MinimumHeight;
			else
				textBlockSize.Height += _heightPadding;

			return textBlockSize;
		}
	}
}
