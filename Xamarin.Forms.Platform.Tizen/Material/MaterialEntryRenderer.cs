using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Material;
using Xamarin.Forms.Platform.Tizen.Native;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Entry), typeof(MaterialEntryRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	class MaterialEntryRenderer : EntryRenderer
	{
		public MaterialEntryRenderer() : base()
		{
			VisualElement.VerifyVisualFlagEnabled();
		}

		protected override Native.Entry CreateNativeControl()
		{
			return new Native.MaterialEntry(Forms.NativeParent)
			{
				IsSingleLine = true,
			};
		}
	}
}
