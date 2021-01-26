using System.Collections;
using Xamarin.Forms.Platform.Tizen.Native;

namespace Xamarin.Forms.Platform.Tizen.TV
{
	public class FlyoutItemTemplateAdaptor : ItemTemplateAdaptor
	{
		public FlyoutItemTemplateAdaptor(Element itemsView, IEnumerable items, DataTemplate template, bool hasHeader)
			: base(itemsView, items, template)
		{
			HasHeader = hasHeader;
		}

		public bool HasHeader { get; set; }

		protected override View CreateHeaderView()
		{
			if (!HasHeader)
				return null;

			View header = null;
			if (Element is Shell shell)
			{
				if (shell.FlyoutHeader != null)
				{
					if (shell.FlyoutHeader is View view)
					{
						header = view;
					}
					else if (shell.FlyoutHeaderTemplate != null)
					{
						header = shell.FlyoutHeaderTemplate.CreateContent() as View;
						header.BindingContext = shell.FlyoutHeader;
					}
				}
			}
			return header;
		}
	}
}
