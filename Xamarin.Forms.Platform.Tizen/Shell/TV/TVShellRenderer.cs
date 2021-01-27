using System;

namespace Xamarin.Forms.Platform.Tizen.TV
{
	public class TVShellRenderer : ShellRenderer
	{

		public TVShellRenderer() : base()
		{
			RegisterPropertyHandler(Shell.FlyoutBehaviorProperty, UpdateFlyoutBehavior);
		}

		protected override INavigationDrawer CreateNavigationDrawer()
		{
			return new TVNavigationDrawer(Forms.NativeParent);
		}

		protected override ShellItemRenderer CreateShellItemRenderer(ShellItem item)
		{
			return new TVShellItemRenderer(item);
		}

		protected override INavigationView CreateNavigationView()
		{
			return new TVNavigationView(Forms.NativeParent, Element);
		}

		void UpdateFlyoutBehavior()
		{
			(NavigationDrawer as TVNavigationDrawer).DrawerBehavior = Element.FlyoutBehavior.ToNative();
		}
	}
}