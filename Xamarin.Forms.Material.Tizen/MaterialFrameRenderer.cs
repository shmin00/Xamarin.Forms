using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Material;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Frame), typeof(MaterialFrameRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	public class MaterialFrameRenderer : ViewRenderer<Frame, MaterialCard>
	{

		static readonly Color s_DefaultColor = Color.White;

		public MaterialFrameRenderer()
		{
			VisualElement.VerifyVisualFlagEnabled();

			RegisterPropertyHandler(Frame.BorderColorProperty, UpdateBorderColor);
			RegisterPropertyHandler(Frame.HasShadowProperty, UpdateShadowVisibility);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			if(Control == null)
			{
				SetNativeControl(new MaterialCard(Forms.NativeParent));
			}

			base.OnElementChanged(e);
		}

		void UpdateBorderColor()
		{
			Control.BorderColor = Element.BorderColor.ToNative();
		}

		void UpdateShadowVisibility()
		{
			Control.HasShadow = Element.HasShadow;
		}
	}
}
