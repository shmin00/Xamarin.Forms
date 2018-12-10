using System;
using ElmSharp;
using Tizen.NET.MaterialComponents;
using NButton = Xamarin.Forms.Platform.Tizen.Native.Button;

namespace Xamarin.Forms.Platform.Tizen.Material
{
	public class MaterialButton : NButton
	{
		MButton _button;

		public MaterialButton(EvasObject parent) : base(parent)
		{
		}

		protected override IntPtr CreateHandle(EvasObject parent)
		{
			_button = new MButton(parent);
			RealHandle = _button.RealHandle;

			return _button.Handle;
		}
	}
}
