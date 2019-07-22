using Xamarin.Forms;
using Xamarin.Forms.Material.Tizen;
using Xamarin.Forms.Material.Tizen.Native;
using Xamarin.Forms.Platform.Tizen;
using TForms = Xamarin.Forms.Platform.Tizen.Forms;

[assembly: ExportRenderer(typeof(Picker), typeof(MaterialPckerRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace Xamarin.Forms.Material.Tizen
{
	public class MaterialPckerRenderer : PickerRenderer
	{
		Color _defaultTitleColor = Color.Black;

		protected override ElmSharp.Entry CreateNativeControl()
		{
			return new MPicker(TForms.NativeParent);
		}

		protected override void UpdateSelectedIndex()
		{
			if (Control is MPicker mp)
			{
				mp.Placeholder = (Element.SelectedIndex == -1 || Element.Items == null ?
				"" : Element.Items[Element.SelectedIndex]);
			}
		}

		protected override void UpdateTitleColor()
		{
			if(Control is MPicker mp)
			{
				if (Element.TitleColor.IsDefault)
				{
					mp.PlaceholderColor = _defaultTitleColor.ToNative();
				}
				else
				{
					mp.PlaceholderColor = Element.TitleColor.ToNative();
				}
			}
		}
	}
}
