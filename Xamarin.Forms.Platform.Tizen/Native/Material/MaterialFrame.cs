using System;
using ElmSharp;
using EColor = ElmSharp.Color;
using ELayout = ElmSharp.Layout;

namespace Xamarin.Forms.Platform.Tizen.Native
{
	public class MaterialFrame : Canvas
	{
		ELayout _layout;
		bool _hasShadow = true;
		EColor _borderColor = EColor.Default;

		public bool HasShadow
		{
			get
			{
				return _hasShadow;
			}
			set
			{
				_hasShadow = value;
				if(_hasShadow)
				{
					_layout.SignalEmit("elm,action,show,shadow", "");
				}
				else
				{
					_layout.SignalEmit("elm,action,hide,shadow", "");
				}
			}
		}

		public EColor BorderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				_borderColor = value;
				if (_borderColor == EColor.Default)
				{
					_layout.SetPartColor("border", EColor.Transparent);
				}
				else
				{
					_layout.SetPartColor("border", _borderColor);
				}
			}
		}

		public MaterialFrame(EvasObject parent) : base(parent)
		{
		}

		protected override IntPtr CreateHandle(EvasObject parent)
		{
			var handle = base.CreateHandle(parent);

			_layout = CreateLayout(parent);

			if (RealHandle == IntPtr.Zero)
			{
				RealHandle = handle;
			}
			Handle = handle;

			_layout.SetPartContent("elm.swallow.content", this);
			_layout.SignalEmit("elm,action,show,shadow", "");

			return _layout;
		}

		protected virtual ELayout CreateLayout(EvasObject parent)
		{
			var layout = new ELayout(parent);
			layout.SetTheme("layout", "frame", "material");

			return layout;
		}
	}
}