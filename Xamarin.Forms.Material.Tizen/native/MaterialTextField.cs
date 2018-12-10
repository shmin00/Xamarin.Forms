using System;
using ElmSharp;
using Tizen.NET.MaterialComponents;
using Xamarin.Forms.Platform.Tizen.Native;
using ELayout = ElmSharp.Layout;
using EColor = ElmSharp.Color;

namespace Xamarin.Forms.Platform.Tizen.Material
{
	public class TF : MTextField
	{
		public new ELayout Layout { get => base.Layout; }

		public TF(EvasObject parent) : base(parent)
		{
			Log.Debug("############################ TF");
		}
	}

	public class MaterialTextField : EditfieldEntry
	{
		TF _textfield;

		public MaterialTextField(EvasObject parent) : base(parent)
		{
		}

		public override string Placeholder
		{
			get
			{
				return _textfield.Label;
			}
			set
			{
				base.Placeholder = value;
				_textfield.Label = value;
			}
		}

		public override EColor PlaceholderColor
		{
			get
			{
				return _textfield.LabelColor;
			}
			set
			{
				base.PlaceholderColor = value;
				_textfield.LabelColor = value;
			}
		}

		public override EColor TextColor
		{
			get
			{
				return _textfield.TextColor;
			}
			set
			{
				_textfield.TextColor = value;
				base.TextColor = value;
			}
		}

		public override string Text
		{
			get
			{
				return _textfield.Text;
			}
			set
			{
				_textfield.Text = value;
				base.Text = value;
			}
		}

		public override EColor BackgroundColor
		{
			get
			{
				return _textfield.BackgroundColor;
			}
			set
			{
				Log.Debug($"*********************************set background {value}");
				//base.BackgroundColor = value;
				_textfield.BackgroundColor = value;
			}
		}

		protected override IntPtr CreateHandle(EvasObject parent)
		{
			_textfield = new TF(parent);
			RealHandle = _textfield.RealHandle;

			base.CreateHandle(parent);
			return _textfield.Handle;
		}

		protected override ElmSharp.Layout CreateEditFieldLayout(EvasObject parent)
		{
			return _textfield.Layout;
		}
	}
}
