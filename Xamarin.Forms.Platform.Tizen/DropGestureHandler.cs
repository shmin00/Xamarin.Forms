using System;
using ElmSharp;
using Tizen.Common;

namespace Xamarin.Forms.Platform.Tizen
{
	public class DropGestureHandler : GestureHandler
	{
		IVisualElementRenderer _renderer;

		public override GestureHandlerType Type => GestureHandlerType.Drop;

		public DropGestureHandler(IGestureRecognizer recognizer, IVisualElementRenderer renderer) : base(recognizer)
		{
			_renderer = renderer;

			var target = GestureExtensions.DragDropContentType.Text;

			if (DotnetUtil.TizenAPIVersion < 5)
				target = GestureExtensions.DragDropContentType.Targets;

			GestureExtensions.AddDropTarget(NativeView, target, OnEnterCallback, OnLeaveCallback, null, OnDropCallback);

		}

		EvasObject NativeView
		{
			get
			{
				var native = _renderer.NativeView;
				if (Forms.UseSkiaSharp)
				{
					if (_renderer is SkiaSharp.ICanvasRenderer canvasRenderer)
					{
						native = canvasRenderer.RealNativeView;
					}
				}
				return native;
			}
		}

		void OnEnterCallback(IntPtr data, IntPtr obj)
		{
			var currentStateData = DragGestureHandler.CurrentStateData;
			if (currentStateData == null)
				return;

			var arg = new DragEventArgs(currentStateData.DataPackage);
			var dropRecognizer = Recognizer as DropGestureRecognizer;

			if (dropRecognizer != null || dropRecognizer.AllowDrop)
				dropRecognizer.SendDragOver(arg);

			DragGestureHandler.CurrentStateData.AcceptedOperation = arg.AcceptedOperation;
		}

		void OnLeaveCallback(IntPtr data, IntPtr obj)
		{
			var currentStateData = DragGestureHandler.CurrentStateData;
			if (currentStateData == null)
				return;

			var arg = new DragEventArgs(currentStateData.DataPackage);
			var dropRecognizer = Recognizer as DropGestureRecognizer;

			if (dropRecognizer != null && dropRecognizer.AllowDrop)
				dropRecognizer.SendDragLeave(arg);

			DragGestureHandler.CurrentStateData.AcceptedOperation = arg.AcceptedOperation;
		}

		bool OnDropCallback(IntPtr data, IntPtr obj, IntPtr selectionData)
		{
			var currentStateData = DragGestureHandler.CurrentStateData;

			if (currentStateData.DataPackage == null || currentStateData.AcceptedOperation == DataPackageOperation.None)
				return false;

			var dropRecognizer = Recognizer as DropGestureRecognizer;
			
			if (dropRecognizer != null && dropRecognizer.AllowDrop)
				 dropRecognizer.SendDrop(new DropEventArgs(currentStateData.DataPackage.View)).Wait();

			if (DotnetUtil.TizenAPIVersion >= 5)
				DragGestureHandler.ResetCurrentStateData();

			return true;
		}

		#region GestureHandler
		protected override void OnStarted(View sender, object data)
		{
		}

		protected override void OnMoved(View sender, object data)
		{
		}

		protected override void OnCompleted(View sender, object data)
		{
		}

		protected override void OnCanceled(View sender, object data)
		{
		}
		#endregion
	}
}