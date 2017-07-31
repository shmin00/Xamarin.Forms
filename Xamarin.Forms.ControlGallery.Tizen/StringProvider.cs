using Xamarin.Forms;
using Xamarin.Forms.ControlGallery.Tizen;
using Xamarin.Forms.Controls;

[assembly: Dependency(typeof(StringProvider))]

namespace Xamarin.Forms.ControlGallery.Tizen
{
	public class StringProvider : IStringProvider
	{
		public string CoreGalleryTitle
		{
			get { return "Tizen Core Gallery"; }
		}
	}
}