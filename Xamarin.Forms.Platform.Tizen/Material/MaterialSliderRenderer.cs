using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Material;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Slider), typeof(MaterialSliderRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	class MaterialSliderRenderer : SliderRenderer
	{
		public MaterialSliderRenderer() : base()
		{
			VisualElement.VerifyVisualFlagEnabled();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			base.OnElementChanged(e);
			Control.Style = "material";
		}
	}
}
