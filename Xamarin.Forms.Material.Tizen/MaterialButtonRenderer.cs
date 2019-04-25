using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Material.Tizen;
using Xamarin.Forms.Material.Tizen.Native;
using EButton = ElmSharp.Button;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;

[assembly: ExportRenderer(typeof(Button), typeof(MaterialButtonRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace Xamarin.Forms.Material.Tizen
{
	public class MaterialButtonRenderer : ButtonRenderer
	{
		protected override EButton CreateNativeControl()
		{
			return new MButton(TForms.NativeParent);
		}
	}
}
