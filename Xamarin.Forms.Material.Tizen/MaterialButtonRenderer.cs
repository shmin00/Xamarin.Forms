using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Material;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(MaterialButtonRenderer), new[] { typeof(VisualRendererMarker.Material) })]
namespace Xamarin.Forms.Platform.Tizen.Material
{
	class MaterialButtonRenderer : ButtonRenderer
	{
		public MaterialButtonRenderer() : base()
		{
			VisualElement.VerifyVisualFlagEnabled();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			if (Control == null)
			{
				SetNativeControl(new MaterialButton(Forms.NativeParent));
			}

			base.OnElementChanged(e);
		}

		protected override void UpdateText()
		{
			Control.Text = Element.Text.ToUpper() ?? "";
		}
	}
}
