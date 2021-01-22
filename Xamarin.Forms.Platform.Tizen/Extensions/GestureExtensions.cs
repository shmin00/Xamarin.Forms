using System;
using System.Runtime.InteropServices;
using ElmSharp;
using EGestureType = ElmSharp.GestureLayer.GestureType;

namespace Xamarin.Forms.Platform.Tizen
{

	public static class GestureExtensions
	{
		public static EGestureType ConvertToNative(this GestureHandlerType type)
		{
			if (type == GestureHandlerType.Tap)
				return EGestureType.Tap;
			else if (type == GestureHandlerType.LongTap)
				return EGestureType.LongTap;
			else if (type == GestureHandlerType.DoubleTap)
				return EGestureType.DoubleTap;
			else if (type == GestureHandlerType.TripleTap)
				return EGestureType.TripleTap;
			else if (type == GestureHandlerType.Pan)
				return EGestureType.Momentum;
			else if (type == GestureHandlerType.Pinch)
				return EGestureType.Zoom;
			else if (type == GestureHandlerType.Swipe)
				return EGestureType.Flick;
			else if (type == GestureHandlerType.Rotate)
				return EGestureType.Rotate;
			else if (type == GestureHandlerType.Drag)
				return EGestureType.Momentum;
			return EGestureType.Tap;
		}

		public static void AddDropTarget(EvasObject obj, DragDropContentType contentType,
			Interop.DragStateCallback enterCallback,
			Interop.DragStateCallback leaveCallback,
			Interop.DragPositionCallback positionCallback,
			Interop.DropCallback dropCallback)
		{
			Interop.elm_drop_target_add(obj.RealHandle, contentType,
				enterCallback, IntPtr.Zero,
				leaveCallback, IntPtr.Zero,
				positionCallback, IntPtr.Zero,
				dropCallback, IntPtr.Zero);
		}

		public static void StartDrag(EvasObject obj, DragDropContentType contentType,
			string data, DragDropActionType actionType,
			Interop.DragIconCreateCallback iconCallback,
			Interop.DragPositionCallback positionCallback,
			Interop.DragAcceptCallback acceptCallback,
			Interop.DragStateCallback statCallback)
		{
			var strData = Marshal.StringToHGlobalAnsi(data);
			Interop.elm_drag_start(obj.RealHandle, contentType, strData, actionType,
				iconCallback, IntPtr.Zero,
				positionCallback, IntPtr.Zero,
				acceptCallback, IntPtr.Zero,
				statCallback, IntPtr.Zero);
		}

		public enum DragDropContentType
		{
			Targets = -1,
			None = 0,
			Text = 1,
			MarkUp = 2,
			Image = 4,
			VCard = 8,
			Html = 16
		}

		public enum DragDropActionType
		{
			Unknown = 0,
			Copy,
			Move,
			Private,
			Ask,
			List,
			Link,
			Description
		}

		public class Interop
		{
			public const string LibElementary = "libelementary.so.1";

			public delegate IntPtr DragIconCreateCallback(IntPtr data, IntPtr window, ref int xoff, ref int yoff);
			public delegate void DragPositionCallback(IntPtr data, IntPtr obj, int x, int y, int actionType);
			public delegate void DragAcceptCallback(IntPtr data, IntPtr obj, bool accept);
			public delegate void DragStateCallback(IntPtr data, IntPtr obj);
			public delegate bool DropCallback(IntPtr data, IntPtr obj, IntPtr selectionData);

			[DllImport(LibElementary)]
			internal static extern void elm_drop_target_add(IntPtr obj,
				DragDropContentType type,
				DragStateCallback enterCallback,
				IntPtr enterData,
				DragStateCallback leaveCallback,
				IntPtr leaveData,
				DragPositionCallback positionCallback,
				IntPtr positionData,
				DropCallback dropcallback,
				IntPtr dropData);

			[DllImport(LibElementary)]
			internal static extern void elm_drag_start(IntPtr obj,
				DragDropContentType contentType,
				IntPtr data,
				DragDropActionType actionType,
				DragIconCreateCallback iconCreateCallback,
				IntPtr iconCreateData,
				DragPositionCallback dragPositionCallback,
				IntPtr dragPositonData,
				DragAcceptCallback dragAcceptCallback,
				IntPtr dragAcceptData,
				DragStateCallback dragStateCallback,
				IntPtr dragStateData);

			[DllImport(LibElementary)]
			internal static extern void evas_object_geometry_get(IntPtr obj, out int x, out int y, out int w, out int h);

			[DllImport(LibElementary)]
			internal static extern IntPtr evas_object_image_data_get(IntPtr obj, bool forWriting);

			[DllImport(LibElementary)]
			internal static extern void evas_object_image_data_set(IntPtr obj, IntPtr data);

			[DllImport(LibElementary)]
			internal static extern void evas_object_image_size_set(IntPtr obj, int w, int h);
		}
	}
}
