using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Material;
using EColor = ElmSharp.Color;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ProgressBar), typeof(MaterialProgressBarRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	class MaterialProgressBarRenderer : ProgressBarRenderer
	{
		public MaterialProgressBarRenderer() : base()
		{
			VisualElement.VerifyVisualFlagEnabled();
		}
	}
}
