using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Material;
using EProgressBar = ElmSharp.ProgressBar;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ActivityIndicator), typeof(MaterialActivityIndicatorRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	class MaterialActivityIndicatorRenderer : ActivityIndicatorRenderer
	{
		public MaterialActivityIndicatorRenderer() : base()
		{
			VisualElement.VerifyVisualFlagEnabled();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
		{
			if (Control == null)
			{
				SetNativeControl(new EProgressBar(Forms.NativeParent)
				{
					Style = "process_material",
					IsPulseMode = true,
				});
			}
			base.OnElementChanged(e);
		}
	}
}
