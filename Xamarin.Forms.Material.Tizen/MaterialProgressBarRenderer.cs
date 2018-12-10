using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Native;
using Xamarin.Forms.Platform.Tizen.Material;
using Tizen.NET.MaterialComponents;
using EColor = ElmSharp.Color;
using ElmSharp;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ProgressBar), typeof(MaterialProgressBarRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	class MaterialProgressBarRenderer : ProgressBarRenderer
	{
		EColor _defaultColor = EColor.Black;

		public MaterialProgressBarRenderer()
		{
			Log.Debug("******************* MaterialProgressBarRenderer");
			VisualElement.VerifyVisualFlagEnabled();
		}

		protected override ElmSharp.ProgressBar CrateNativeControl()
		{
			return new MProgressIndicator(Forms.NativeParent);
		}
	}
}
