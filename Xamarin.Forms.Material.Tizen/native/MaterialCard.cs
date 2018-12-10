using System;
using ElmSharp;
using Tizen.NET.MaterialComponents;
using Xamarin.Forms.Platform.Tizen.Native;
using EColor = ElmSharp.Color;

namespace Xamarin.Forms.Platform.Tizen.Material
{
	public class MaterialCard : Canvas
	{

		MCard _card;

		public bool HasShadow
		{
			get
			{
				return _card.HasShadow;
			}
			set
			{
				_card.HasShadow = value;
			}
		}

		public EColor BorderColor
		{
			get
			{
				return _card.BorderColor;
			}
			set
			{
				_card.BorderColor = value;
			}
		}

		public MaterialCard(EvasObject parent) : base(parent)
		{
		}

		protected override IntPtr CreateHandle(EvasObject parent)
		{
			_card = new MCard(parent);
			RealHandle = _card.RealHandle;

			return _card.Handle;
		}
	}
}
