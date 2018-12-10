using System.ComponentModel;

using SpecificVE = Xamarin.Forms.PlatformConfiguration.TizenSpecific.VisualElement;
using Specific = Xamarin.Forms.PlatformConfiguration.TizenSpecific.ProgressBar;
using EProgressBar = ElmSharp.ProgressBar;
using EColor = ElmSharp.Color;

namespace Xamarin.Forms.Platform.Tizen
{
	public class ProgressBarRenderer : ViewRenderer<ProgressBar, EProgressBar>
	{
		EColor _defaultColor;

		public ProgressBarRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
		{
			if (Control == null)
			{
				SetNativeControl(CrateNativeControl());
			}

			_defaultColor = Control.GetPartColor("bar");

			if (e.NewElement != null)
			{
				if (e.NewElement.MinimumWidthRequest == -1 &&
				e.NewElement.MinimumHeightRequest == -1 &&
				e.NewElement.WidthRequest == -1 &&
				e.NewElement.HeightRequest == -1)
				{
					Log.Warn("Need to size request");
				}

				UpdateAll();
			}

			base.OnElementChanged(e);
		}

		protected virtual EProgressBar CrateNativeControl()
		{
			return new EProgressBar(Forms.NativeParent);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName)
			{
				UpdateProgress();
			}
			else if (e.PropertyName == ProgressBar.ProgressColorProperty.PropertyName)
			{
				UpdateProgressColor();
			}
			else if (e.PropertyName == Specific.ProgressBarPulsingStatusProperty.PropertyName)
			{
				UpdatePulsingStatus();
			}
			base.OnElementPropertyChanged(sender, e);
		}

		protected override void UpdateThemeStyle()
		{
			var themeStyle = SpecificVE.GetStyle(Element);
			if (!string.IsNullOrEmpty(themeStyle))
				Control.Style = themeStyle;
		}

		void UpdateAll()
		{
			UpdateProgress();
			UpdateProgressColor();
			UpdatePulsingStatus();
		}

		protected virtual void UpdateProgressColor()
		{
			Control.Color = Element.ProgressColor.IsDefault ? _defaultColor : Element.ProgressColor.ToNative();
		}

		protected virtual void UpdateProgress()
		{
			Control.Value = Element.Progress;
		}

		protected virtual void UpdatePulsingStatus()
		{
			bool isPulsing = Specific.GetPulsingStatus(Element);
			if (isPulsing)
			{
				Control.PlayPulse();
			}
			else
			{
				Control.StopPulse();
			}
		}
	}
}

