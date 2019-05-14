using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Native;
using Xamarin.Forms.Material.Tizen;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;
using XFEntry = Xamarin.Forms.Entry;

[assembly: ExportRenderer(typeof(XFEntry), typeof(MaterialEntryRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace Xamarin.Forms.Material.Tizen
{
	public class MaterialEntryRenderer : EntryRenderer
	{
		protected override ElmSharp.Entry CreateNativeControl()
		{
			return new MaterialEntry(TForms.NativeParent)
			{
				IsSingleLine = true,
			};
		}

		protected override void UpdateTextColor()
		{
			(Control as MaterialEntry).TextColor = Element.TextColor.ToNative();
			(Control as MaterialEntry).TextFocusedColor = Element.TextColor.ToNative();
			(Control as MaterialEntry).UnderlineColor = Element.TextColor.ToNative();
			(Control as MaterialEntry).UnderlineFocusedColor = Element.TextColor.ToNative();
			(Control as MaterialEntry).CursorColor = Element.TextColor.ToNative();
		}
	}
}
