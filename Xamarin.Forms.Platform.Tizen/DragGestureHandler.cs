using System;
using ElmSharp;
using Tizen.Common;

namespace Xamarin.Forms.Platform.Tizen
{
	public class DragGestureHandler : GestureHandler
	{
		IVisualElementRenderer _renderer;

		static CustomDragStateData _currentDragstateData;

		public override GestureHandlerType Type => GestureHandlerType.Drag;

		public DragGestureHandler(IGestureRecognizer recognizer, IVisualElementRenderer renderer) : base(recognizer)
		{
			_renderer = renderer;
		}

		public static CustomDragStateData CurrentStateData
		{
			get
			{
				return _currentDragstateData;
			}
		}

		public static void ResetCurrentStateData()
		{
			_currentDragstateData = null;
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

		protected override void OnStarted(View sender, object data)
		{
			if (!((DragGestureRecognizer)Recognizer).CanDrag)
				return;

			var target = GestureExtensions.DragDropContentType.Text;

			var StrData = GetStringValue();
			GestureExtensions.StartDrag(NativeView, target,
										StrData, GestureExtensions.DragDropActionType.Move,
										OnIconCallback, null, null, OnDragDoneCallback);
		}

		IntPtr OnIconCallback(IntPtr data, IntPtr window, ref int xoff, ref int yoff)
		{
			var arg = ((DragGestureRecognizer)Recognizer).SendDragStarting(_renderer.Element);

			if (arg.Cancel)
				return IntPtr.Zero;

			_currentDragstateData = new CustomDragStateData();
			_currentDragstateData.DataPackage = arg.Data;

			EvasObject icon = null;
			EvasObject parent = new CustomWindow(NativeView, window);

			if (_renderer.Element is IImageElement)
			{
				icon = GetImageIcon(parent);
			}
			else
			{
				icon = GetDefaultIcon(parent);
			}

			var bound = NativeView.Geometry;
			bound.X = 0;
			bound.Y = 0;
			icon.Geometry = bound;

			return icon;
		}

		EvasObject GetDefaultIcon(EvasObject parent)
		{
			if (IsTextElement())
			{
				var label = new Native.Label(parent);
				label.Text = GetStringValue();
				return label;
			}
			else
			{
				var box = new ElmSharp.Box(parent);
				box.BackgroundColor = ElmSharp.Color.Gray;
				box.Opacity = 50;
				return box;
			}
		}

		bool IsTextElement()
		{
			var element = _renderer.Element;
			if (element is Label || element is Entry
				|| element is Editor || element is TimePicker
				|| element is DatePicker || element is CheckBox
				|| element is Switch || element is RadioButton)
				return true;

			return false;
		}

		string GetStringValue()
		{
			var element = _renderer.Element;
			string text = (DotnetUtil.TizenAPIVersion < 5) ? " " : "";

			if (element is Label label)
				text = label.Text;
			else if (element is Entry entry)
				text = entry.Text;
			else if (element is Editor editor)
				text = editor.Text;
			else if (element is TimePicker tp)
				text = tp.Time.ToString();
			else if (element is DatePicker dp)
				text = dp.Date.ToString();
			else if (element is CheckBox cb)
				text = cb.IsChecked.ToString();
			else if (element is Switch sw)
				text = sw.IsToggled.ToString();
			else if (element is RadioButton rb)
				text = rb.IsChecked.ToString();

			return text;
		}

		EvasImage GetImageIcon(EvasObject parent)
		{
			var evasImg = new EvasImage(parent);
			evasImg.IsFilled = true;

			if (NativeView is Native.Image image)
			{
				var imgObj = image.ImageObject;
				evasImg.Size = imgObj.Size;
				var imgPtr = GestureExtensions.evas_object_image_data_get(imgObj, true);
				GestureExtensions.evas_object_image_data_set(evasImg, imgPtr);
			}

			return evasImg;
		}

		void OnDragDoneCallback(IntPtr data, IntPtr obj)
		{
			if (!((DragGestureRecognizer)Recognizer).CanDrag || _currentDragstateData == null)
				return;

			//Drag and drop callback sequence has been changed since Tizen 5.0
			// 4.0 : Drop - DragDone, 5.0 ~ : DragDone - Drop
			if (DotnetUtil.TizenAPIVersion < 5)
				ResetCurrentStateData();

			(Recognizer as DragGestureRecognizer)?.SendDropCompleted(new DropCompletedEventArgs());
		}

		#region GestureHandler
		protected override void OnCompleted(View sender, object data)
		{
		}

		protected override void OnMoved(View sender, object data)
		{
		}

		protected override void OnCanceled(View sender, object data)
		{
		}
		#endregion

		public class CustomWindow : EvasObject
		{
			IntPtr _handle;

			public CustomWindow(EvasObject parent, IntPtr handle) : base()
			{
				_handle = handle;
				Realize(parent);
			}

			public CustomWindow(EvasObject handle) : base(handle)
			{
			}

			protected override IntPtr CreateHandle(EvasObject parent)
			{
				return _handle;
			}
		}

		public class CustomDragStateData
		{
			public DataPackage DataPackage { get; set; }
			public DataPackageOperation AcceptedOperation { get; set; } = DataPackageOperation.Copy;
		}
	}
}