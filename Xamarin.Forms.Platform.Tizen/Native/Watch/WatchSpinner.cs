using System;
using ElmSharp;
using ElmSharp.Wearable;
using TizenDotnetUtil = Tizen.Common.DotnetUtil;

namespace Xamarin.Forms.Platform.Tizen.Native.Watch
{
	public class WatchSpinner : CircleSpinner, IRotaryInteraction
	{
		SmartEvent _listShow, _listHide;

		public event EventHandler ListShow;

		public event EventHandler ListHide;

		public IRotaryActionWidget RotaryWidget { get => this; }

		public WatchSpinner(EvasObject parent, CircleSurface surface) : base(parent, surface)
		{
			Style = "circle";

			if (TizenDotnetUtil.TizenAPIVersion == 4)
			{
				_listShow = new ElmSharp.SmartEvent(this, "genlist,show");
				_listHide = new ElmSharp.SmartEvent(this, "genlist,hide");
			}
			else
			{
				_listShow = new ElmSharp.SmartEvent(this, "list,show");
				_listHide = new ElmSharp.SmartEvent(this, "list,hide");
			}

			_listShow.On += (s, e) => ListShow?.Invoke(this, EventArgs.Empty);
			_listHide.On += (s, e) => ListHide?.Invoke(this, EventArgs.Empty);
		}
	}
}