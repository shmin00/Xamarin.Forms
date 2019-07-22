using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Native;
using Xamarin.Forms.Material.Tizen;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;
using System;

[assembly: ExportRenderer(typeof(Editor), typeof(MaterialEditorRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace Xamarin.Forms.Material.Tizen
{
	public class MaterialEditorRenderer : EditorRenderer
	{
		protected override ElmSharp.Entry CreateNativeControl()
		{
			var entry = new MaterialEntry(TForms.NativeParent);
			entry.IsSingleLine = false;
			return entry;
		}

		protected override void UpdateTextColor()
		{
			if(Control is MaterialEntry me)
			{
				me.TextColor = Element.TextColor.ToNative();
				me.TextFocusedColor = Element.TextColor.ToNative();
				me.UnderlineColor = Element.TextColor.ToNative();
				me.UnderlineFocusedColor = Element.TextColor.ToNative();
				me.CursorColor = Element.TextColor.ToNative();
			}
		}

		protected override Size MinimumSize()
		{
			var size = base.MinimumSize();
			size.Height += 200;
			Console.WriteLine($"+++++++++++++ size={size}");

			return size;
		}
	}
}
