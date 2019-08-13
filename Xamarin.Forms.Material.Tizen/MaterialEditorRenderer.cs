using Xamarin.Forms;
using Xamarin.Forms.Material.Tizen;
using Xamarin.Forms.Material.Tizen.Native;
using Xamarin.Forms.Platform.Tizen;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;

[assembly: ExportRenderer(typeof(Editor), typeof(MaterialEditorRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace Xamarin.Forms.Material.Tizen
{
	public class MaterialEditorRenderer : EditorRenderer
	{
		protected override ElmSharp.Entry CreateNativeControl()
		{
			var entry = new MEditor(TForms.NativeParent);
			entry.IsSingleLine = false;
			entry.LineWrapType = ElmSharp.WrapType.Mixed;
			return entry;
		}

		protected override void UpdateTextColor()
		{
			if (Control is MEditor me)
			{
				me.TextColor = Element.TextColor.ToNative();
				me.TextFocusedColor = Element.TextColor.ToNative();
				me.UnderlineColor = Element.TextColor.ToNative();
				me.UnderlineFocusedColor = Element.TextColor.ToNative();
				me.CursorColor = Element.TextColor.ToNative();
			}
		}
	}
}
