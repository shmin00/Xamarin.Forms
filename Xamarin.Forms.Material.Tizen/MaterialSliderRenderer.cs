using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Material;
using Tizen.NET.MaterialComponents;
using ESlider = ElmSharp.Slider;
using ElmSharp;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Slider), typeof(MaterialSliderRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	class MaterialSliderRenderer : SliderRenderer
	{
		public MaterialSliderRenderer()
		{
			Log.Debug("******************* MaterialSliderRenderer");
			VisualElement.VerifyVisualFlagEnabled();
		}

		protected override ESlider CreateNativeControl()
		{
			return new MSlider(Forms.NativeParent);
		}
	}
}
