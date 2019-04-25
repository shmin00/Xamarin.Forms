using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Material.Tizen;
using Xamarin.Forms.Material.Tizen.Native;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;
using XFEntry = Xamarin.Forms.Entry;


[assembly: ExportRenderer(typeof(XFEntry), typeof(MaterialEntryRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace Xamarin.Forms.Material.Tizen
{
	public class MaterialEntryRenderer : EntryRenderer
	{
		protected override ElmSharp.Entry CreateNativeControl()
		{
			return new MEntry(TForms.NativeParent)
			{
				IsSingleLine = true,
			};
		}

		protected override void UpdateTextColor()
		{
			(Control as MEntry).TextColor = Element.TextColor.ToNative();
			(Control as MEntry).TextFocusedColor = Element.TextColor.ToNative();
			(Control as MEntry).UnderlineColor = Element.TextColor.ToNative();
			(Control as MEntry).UnderlineFocusedColor = Element.TextColor.ToNative();
			(Control as MEntry).CursorColor = Element.TextColor.ToNative();
		}
	}
}
