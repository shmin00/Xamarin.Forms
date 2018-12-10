using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Material;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Entry), typeof(MaterialEntryRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	class MaterialEntryRenderer : EntryRenderer
	{
		public MaterialEntryRenderer()
		{
			VisualElement.VerifyVisualFlagEnabled();
		}

		protected override Native.Entry CreateNativeControl()
		{
			return new MaterialTextField(Forms.NativeParent);
		}
	}
}
