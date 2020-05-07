using System;
using ElmSharp;
using ElmSharp.Wearable;

namespace Xamarin.Forms.Platform.Tizen.Native.Watch
{
	public class WatchDateTimePicker : DateTimePicker, IRotaryInteraction
	{
		readonly CircleSurface _surface;
		public WatchDateTimePicker(EvasObject parent, CircleSurface surface) : base()
		{
			_surface = surface;
			Realize(parent);
			UpdateMode();
		}

		public CircleDateTimeSelector CircleSelector { get; private set; }

		public IRotaryActionWidget RotaryWidget => CircleSelector;

		protected override void UpdateMode()
		{
			if (Mode == DateTimePickerMode.Date)
			{
				Style = "datepicker/circle";
			}
			else
			{
				Style = "timepicker/circle";
			}
		}

		protected override IntPtr CreateHandle(EvasObject parent)
		{
			CircleSelector = new CircleDateTimeSelector(parent, _surface);
			if (CircleSelector.RealHandle != CircleSelector.Handle)
			{
				RealHandle = CircleSelector.RealHandle;
			}
			return CircleSelector.Handle;
		}
	}
}
