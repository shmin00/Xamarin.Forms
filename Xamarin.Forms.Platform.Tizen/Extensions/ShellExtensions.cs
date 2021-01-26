namespace Xamarin.Forms.Platform.Tizen
{
	public static class ShellExtensions
	{
		public static DrawerBehavior ToNative(this FlyoutBehavior b)
		{
			if (b == FlyoutBehavior.Disabled)
				return DrawerBehavior.Disabled;
			else if (b == FlyoutBehavior.Locked)
				return DrawerBehavior.Locked;
			else
				return DrawerBehavior.Default;
		}
	}
}
