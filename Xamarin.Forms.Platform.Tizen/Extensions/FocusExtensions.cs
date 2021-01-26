using System;
using System.Runtime.InteropServices;
using ElmSharp;
using ERect = ElmSharp.Rect;

namespace Xamarin.Forms.Platform.Tizen
{
	public static class FocusExtensions
	{
		public static bool IsFocusedObjectExist(this EvasObject obj)
		{
			var objPtr = elm_object_focused_object_get(obj);
			if (objPtr != IntPtr.Zero)
				return true;
			else
				return false;
		}

		public static ERect GetFocusedObjectGeometry(this EvasObject obj)
		{
			var objPtr = elm_object_focused_object_get(obj);
			evas_object_geometry_get(objPtr, out int x, out int y, out int w, out int h);
			return new ERect(x, y, w, h);
		}

		public static bool IsFocusedObjectVisible(this EvasObject obj)
		{
			var objPtr = elm_object_focused_object_get(obj);
			return evas_object_visible_get(obj);
		}

		[DllImport("libelementary.so.1")]
		internal static extern IntPtr elm_object_focused_object_get(IntPtr obj);

		[DllImport("libelementary.so.1")]
		internal static extern void evas_object_geometry_get(IntPtr obj, out int x, out int y, out int w, out int h);

		[DllImport("libevas.so.1")]
		internal static extern bool evas_object_visible_get(IntPtr obj);
	}
}
