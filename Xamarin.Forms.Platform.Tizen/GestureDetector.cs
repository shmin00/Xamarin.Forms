using System.Collections.Generic;
using System.Linq;
using ElmSharp;

namespace Xamarin.Forms.Platform.Tizen
{
	public enum GestureHandlerType
	{
		Tap,
		LongTap,
		DoubleTap,
		TripleTap,
		Pan,
		Pinch,
		Swipe,
		Rotate,
		Drag,
		Drop
	}

	internal class GestureDetector
	{
		readonly IDictionary<GestureHandlerType, List<GestureHandler>> _handlerCache;

		readonly IVisualElementRenderer _renderer;
		GestureLayer _gestureLayer;
		double _doubleTapTime = 0;
		double _longTapTime = 0;
		bool _inputTransparent = false;
		bool _isEnabled;

		View View => _renderer.Element as View;

		EvasObject NativeView => _renderer.NativeView;

		public bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			set
			{
				_isEnabled = value;
				UpdateGestureLayerEnabled();
			}
		}

		public bool InputTransparent
		{
			get
			{
				return _inputTransparent;
			}
			set
			{
				_inputTransparent = value;
				UpdateGestureLayerEnabled();
			}
		}

		public GestureDetector(IVisualElementRenderer renderer)
		{
			_handlerCache = new Dictionary<GestureHandlerType, List<GestureHandler>>();
			_renderer = renderer;
			_isEnabled = View.IsEnabled;
			_inputTransparent = View.InputTransparent;
		}

		public void Clear()
		{
			// this will clear all callbacks in ElmSharp GestureLayer
			_gestureLayer?.Unrealize();
			_gestureLayer = null;
			foreach (var handlers in _handlerCache.Values)
			{
				foreach (var handler in handlers)
				{
					handler.PropertyChanged -= OnGestureRecognizerPropertyChanged;
				}
			}
			_handlerCache.Clear();

			if (Device.Idiom == TargetIdiom.TV)
			{
				_renderer.NativeView.KeyDown -= OnKeyDown;
			}
		}

		public void AddGestures(IEnumerable<IGestureRecognizer> recognizers)
		{
			if (_gestureLayer == null)
			{
				CreateGestureLayer();
			}

			foreach (var item in recognizers)
				AddGesture(item);
		}

		public void RemoveGestures(IEnumerable<IGestureRecognizer> recognizers)
		{
			foreach (var item in recognizers)
				RemoveGesture(item);
		}

		void CreateGestureLayer()
		{
			_gestureLayer = new GestureLayer(_renderer.NativeView);
			_gestureLayer.Attach(_renderer.NativeView);
			_gestureLayer.Deleted += (s, e) =>
			{
				_gestureLayer = null;
				Clear();
			};
			UpdateGestureLayerEnabled();

			if (Device.Idiom == TargetIdiom.TV)
			{
				_renderer.NativeView.KeyDown += OnKeyDown;
			}
		}

		void UpdateGestureLayerEnabled()
		{
			if (_gestureLayer != null)
			{
				_gestureLayer.IsEnabled = !_inputTransparent && _isEnabled;
			}
		}

		void AddGesture(IGestureRecognizer recognizer)
		{
			var handler = CreateHandler(recognizer);
			if (handler == null)
				return;

			var gestureType = handler.Type;
			var timeout = handler.Timeout;
			var cache = _handlerCache;

			if (!cache.ContainsKey(gestureType))
			{
				cache[gestureType] = new List<GestureHandler>();
			}

			handler.PropertyChanged += OnGestureRecognizerPropertyChanged;
			cache[gestureType].Add(handler);

			if (cache[gestureType].Count == 1)
			{
				switch (gestureType)
				{
					case GestureHandlerType.Tap:
					case GestureHandlerType.TripleTap:
						AddTapGesture(gestureType);
						break;

					case GestureHandlerType.DoubleTap:
						AddDoubleTapGesture(gestureType, timeout);
						break;

					case GestureHandlerType.LongTap:
						AddLongTapGesture(gestureType, timeout);
						break;

					case GestureHandlerType.Pan:
						AddMomentumGesture(gestureType);
						break;

					case GestureHandlerType.Pinch:
						AddPinchGesture(gestureType);
						break;

					case GestureHandlerType.Swipe:
						AddFlickGesture(gestureType, timeout);
						break;

					case GestureHandlerType.Rotate:
						AddRotateGesture(gestureType);
						break;

					case GestureHandlerType.Drag:
						AddDragGesture(gestureType);
						break;

					default:
						break;
				}
			}
		}

		void RemoveGesture(IGestureRecognizer recognizer)
		{
			var cache = _handlerCache;
			var handler = LookupHandler(recognizer);
			var gestureType = cache.FirstOrDefault(x => x.Value.Contains(handler)).Key;

			handler.PropertyChanged -= OnGestureRecognizerPropertyChanged;
			cache[gestureType].Remove(handler);

			if (cache[gestureType].Count == 0)
			{
				switch (gestureType)
				{
					case GestureHandlerType.Tap:
					case GestureHandlerType.DoubleTap:
					case GestureHandlerType.TripleTap:
					case GestureHandlerType.LongTap:
						RemoveTapGesture(gestureType);
						break;

					case GestureHandlerType.Pan:
						RemoveMomentumGesture();
						break;

					case GestureHandlerType.Pinch:
						RemovePinchGesture();
						break;

					case GestureHandlerType.Swipe:
						RemoveFlickGesture();
						break;

					case GestureHandlerType.Rotate:
						RemoveRotateGesture();
						break;

					case GestureHandlerType.Drag:
						RemoveDragGesture();
						break;

					case GestureHandlerType.Drop:
						RemoveDragGesture();
						break;

					default:
						break;
				}
			}
		}

		void AddLineGesture(GestureHandlerType type)
		{
			_gestureLayer.SetLineCallback(GestureLayer.GestureState.Start, (data) => { OnGestureStarted(type, data); });
			_gestureLayer.SetLineCallback(GestureLayer.GestureState.Move, (data) => { OnGestureMoved(type, data); });
			_gestureLayer.SetLineCallback(GestureLayer.GestureState.End, (data) => { OnGestureCompleted(type, data); });
			_gestureLayer.SetLineCallback(GestureLayer.GestureState.Abort, (data) => { OnGestureCanceled(type, data); });
		}

		void AddPinchGesture(GestureHandlerType type)
		{
			_gestureLayer.SetZoomCallback(GestureLayer.GestureState.Start, (data) => { OnGestureStarted(type, data); });
			_gestureLayer.SetZoomCallback(GestureLayer.GestureState.Move, (data) => { OnGestureMoved(type, data); });
			_gestureLayer.SetZoomCallback(GestureLayer.GestureState.End, (data) => { OnGestureCompleted(type, data); });
			_gestureLayer.SetZoomCallback(GestureLayer.GestureState.Abort, (data) => { OnGestureCanceled(type, data); });
		}

		void AddTapGesture(GestureHandlerType type)
		{
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.Start, (data) => { OnGestureStarted(type, data); });
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.End, (data) => { OnGestureCompleted(type, data); });
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.Abort, (data) => { OnGestureCanceled(type, data); });
		}

		void AddDoubleTapGesture(GestureHandlerType type, double timeout)
		{
			if (timeout > 0)
				_gestureLayer.DoubleTapTimeout = timeout;

			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.Start, (data) => { OnDoubleTapStarted(type, data); });
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.End, (data) => { OnDoubleTapCompleted(type, data); });
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.Abort, (data) => { OnGestureCanceled(type, data); });
		}

		void AddLongTapGesture(GestureHandlerType type, double timeout)
		{
			if (timeout > 0)
				_gestureLayer.LongTapTimeout = timeout;

			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.Start, (data) => { OnLongTapStarted(type, data); });
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.End, (data) => { OnLongTapCompleted(type, data); });
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.Abort, (data) => { OnGestureCanceled(type, data); });
		}

		void AddFlickGesture(GestureHandlerType type, double timeout)
		{
			if (timeout > 0)
				_gestureLayer.FlickTimeLimit = (int)(timeout * 1000);

			// Task to correct wrong coordinates information when applying EvasMap(Xamarin ex: Translation, Scale, Rotate property)
			// Always change to the absolute coordinates of the pointer.
			int startX = 0;
			int startY = 0;
			_gestureLayer.SetFlickCallback(GestureLayer.GestureState.Start, (data) =>
			{
				startX = _gestureLayer.EvasCanvas.Pointer.X;
				startY = _gestureLayer.EvasCanvas.Pointer.Y;
				data.X1 = startX;
				data.Y1 = startY;
				OnGestureStarted(type, data);
			});
			_gestureLayer.SetFlickCallback(GestureLayer.GestureState.Move, (data) =>
			{
				data.X1 = startX;
				data.Y1 = startY;
				data.X2 = _gestureLayer.EvasCanvas.Pointer.X;
				data.Y2 = _gestureLayer.EvasCanvas.Pointer.Y;
				OnGestureMoved(type, data);
			});
			_gestureLayer.SetFlickCallback(GestureLayer.GestureState.End, (data) =>
			{
				data.X1 = startX;
				data.Y1 = startY;
				data.X2 = _gestureLayer.EvasCanvas.Pointer.X;
				data.Y2 = _gestureLayer.EvasCanvas.Pointer.Y;
				OnGestureCompleted(type, data);
			});
			_gestureLayer.SetFlickCallback(GestureLayer.GestureState.Abort, (data) => { OnGestureCanceled(type, data); });
		}

		void AddRotateGesture(GestureHandlerType type)
		{
			_gestureLayer.SetRotateCallback(GestureLayer.GestureState.Start, (data) => { OnGestureStarted(type, data); });
			_gestureLayer.SetRotateCallback(GestureLayer.GestureState.Move, (data) => { OnGestureMoved(type, data); });
			_gestureLayer.SetRotateCallback(GestureLayer.GestureState.End, (data) => { OnGestureCompleted(type, data); });
			_gestureLayer.SetRotateCallback(GestureLayer.GestureState.Abort, (data) => { OnGestureCanceled(type, data); });
		}

		void AddMomentumGesture(GestureHandlerType type)
		{
			// Task to correct wrong coordinates information when applying EvasMap(Xamarin ex: Translation, Scale, Rotate property)
			// Always change to the absolute coordinates of the pointer.
			int startX = 0;
			int startY = 0;
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Start, (data) =>
			{
				startX = _gestureLayer.EvasCanvas.Pointer.X;
				startY = _gestureLayer.EvasCanvas.Pointer.Y;
				OnGestureStarted(type, data);
			});
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Move, (data) =>
			{
				data.X1 = startX;
				data.Y1 = startY;
				data.X2 = _gestureLayer.EvasCanvas.Pointer.X;
				data.Y2 = _gestureLayer.EvasCanvas.Pointer.Y;
				OnGestureMoved(type, data);
			});
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.End, (data) => { OnGestureCompleted(type, data); });
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Abort, (data) => { OnGestureCanceled(type, data); });
		}

		void AddDragGesture(GestureHandlerType type)
		{
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Start, (data) => { OnGestureStarted(type, data); });
		}

		void RemoveLineGesture()
		{
			_gestureLayer.SetLineCallback(GestureLayer.GestureState.Start, null);
			_gestureLayer.SetLineCallback(GestureLayer.GestureState.Move, null);
			_gestureLayer.SetLineCallback(GestureLayer.GestureState.End, null);
			_gestureLayer.SetLineCallback(GestureLayer.GestureState.Abort, null);
		}

		void RemovePinchGesture()
		{
			_gestureLayer.SetZoomCallback(GestureLayer.GestureState.Start, null);
			_gestureLayer.SetZoomCallback(GestureLayer.GestureState.Move, null);
			_gestureLayer.SetZoomCallback(GestureLayer.GestureState.End, null);
			_gestureLayer.SetZoomCallback(GestureLayer.GestureState.Abort, null);
		}

		void RemoveTapGesture(GestureHandlerType type)
		{
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.Start, null);
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.End, null);
			_gestureLayer.SetTapCallback(type.ConvertToNative(), GestureLayer.GestureState.Abort, null);
		}

		void RemoveFlickGesture()
		{
			_gestureLayer.SetFlickCallback(GestureLayer.GestureState.Start, null);
			_gestureLayer.SetFlickCallback(GestureLayer.GestureState.Move, null);
			_gestureLayer.SetFlickCallback(GestureLayer.GestureState.End, null);
			_gestureLayer.SetFlickCallback(GestureLayer.GestureState.Abort, null);
		}

		void RemoveRotateGesture()
		{
			_gestureLayer.SetRotateCallback(GestureLayer.GestureState.Start, null);
			_gestureLayer.SetRotateCallback(GestureLayer.GestureState.Move, null);
			_gestureLayer.SetRotateCallback(GestureLayer.GestureState.End, null);
			_gestureLayer.SetRotateCallback(GestureLayer.GestureState.Abort, null);
		}

		void RemoveMomentumGesture()
		{
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Start, null);
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Move, null);
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.End, null);
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Abort, null);
		}

		void RemoveDragGesture()
		{
			_gestureLayer.SetMomentumCallback(GestureLayer.GestureState.Start, null);
		}

		#region GestureCallback

		void OnGestureStarted(GestureHandlerType type, object data)
		{
			var cache = _handlerCache;
			if (cache.ContainsKey(type))
			{
				foreach (var handler in cache[type])
				{
					(handler as IGestureController)?.SendStarted(View, data);
				}
			}
		}

		void OnGestureMoved(GestureHandlerType type, object data)
		{
			var cache = _handlerCache;
			if (cache.ContainsKey(type))
			{
				foreach (var handler in cache[type])
				{
					(handler as IGestureController)?.SendMoved(View, data);
				}
			}
		}

		void OnGestureCompleted(GestureHandlerType type, object data)
		{
			var cache = _handlerCache;
			if (cache.ContainsKey(type))
			{
				foreach (var handler in cache[type])
				{
					(handler as IGestureController)?.SendCompleted(View, data);
				}
			}
		}

		void OnGestureCanceled(GestureHandlerType type, object data)
		{
			var cache = _handlerCache;
			if (cache.ContainsKey(type))
			{
				foreach (var handler in cache[type])
				{
					(handler as IGestureController)?.SendCanceled(View, data);
				}
			}
		}

		void OnDoubleTapStarted(GestureHandlerType type, object data)
		{
			_doubleTapTime = ((GestureLayer.TapData)data).Timestamp;
			OnGestureStarted(type, data);
		}

		void OnDoubleTapCompleted(GestureHandlerType type, object data)
		{
			_doubleTapTime = ((GestureLayer.TapData)data).Timestamp - _doubleTapTime;
			var cache = _handlerCache;

			if (cache.ContainsKey(type))
			{
				foreach (var handler in cache[type])
				{
					if ((handler.Timeout * 1000) >= _longTapTime)
						(handler as IGestureController)?.SendCompleted(View, data);
					else
						(handler as IGestureController)?.SendCanceled(View, data);
				}
			}
		}

		void OnLongTapStarted(GestureHandlerType type, object data)
		{
			_longTapTime = ((GestureLayer.TapData)data).Timestamp;
			OnGestureStarted(type, data);
		}

		void OnLongTapCompleted(GestureHandlerType type, object data)
		{
			_longTapTime = ((GestureLayer.TapData)data).Timestamp - _longTapTime;
			var cache = _handlerCache;

			if (cache.ContainsKey(type))
			{
				foreach (var handler in cache[type])
				{
					if ((handler.Timeout * 1000) <= _longTapTime)
						(handler as IGestureController)?.SendCompleted(View, data);
					else
						(handler as IGestureController)?.SendCanceled(View, data);
				}
			}
		}

		#endregion GestureCallback

		GestureHandler CreateHandler(IGestureRecognizer recognizer)
		{
			if (recognizer is TapGestureRecognizer)
			{
				return new TapGestureHandler(recognizer);
			}
			else if (recognizer is PinchGestureRecognizer)
			{
				return new PinchGestureHandler(recognizer);
			}
			else if (recognizer is PanGestureRecognizer)
			{
				return new PanGestureHandler(recognizer);
			}
			else if (recognizer is SwipeGestureRecognizer)
			{
				return new SwipeGestureHandler(recognizer);
			}
			else if (recognizer is DragGestureRecognizer)
			{
				return new DragGestureHandler(recognizer, _renderer);
			}
			else if (recognizer is DropGestureRecognizer)
			{
				return new DropGestureHandler(recognizer, _renderer);
			}
			return Forms.GetHandlerForObject<GestureHandler>(recognizer, recognizer);
		}

		GestureHandler LookupHandler(IGestureRecognizer recognizer)
		{
			var cache = _handlerCache;

			foreach (var handlers in cache.Values)
			{
				foreach (var handler in handlers)
				{
					if (handler.Recognizer == recognizer)
						return handler;
				}
			}
			return null;
		}

		void UpdateTapGesture(GestureHandler handler)
		{
			RemoveGesture(handler.Recognizer);
			AddGesture(handler.Recognizer);

			if (handler.Timeout > _gestureLayer.DoubleTapTimeout)
				_gestureLayer.DoubleTapTimeout = handler.Timeout;
		}

		void UpdateLongTapGesture(GestureHandler handler)
		{
			if (handler.Timeout > 0 && handler.Timeout < _gestureLayer.LongTapTimeout)
				_gestureLayer.LongTapTimeout = handler.Timeout;
		}

		void UpdateFlickGesture(GestureHandler handler)
		{
			if (handler.Timeout > _gestureLayer.FlickTimeLimit)
				_gestureLayer.FlickTimeLimit = (int)(handler.Timeout * 1000);
		}

		void OnGestureRecognizerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var handler = sender as GestureHandler;
			if (handler != null)
			{
				switch (handler.Type)
				{
					case GestureHandlerType.Tap:
					case GestureHandlerType.DoubleTap:
					case GestureHandlerType.TripleTap:
						UpdateTapGesture(handler);
						break;

					case GestureHandlerType.LongTap:
						UpdateLongTapGesture(handler);
						break;

					case GestureHandlerType.Swipe:
						UpdateFlickGesture(handler);
						break;

					default:
						break;
				}
			}
		}

		void OnKeyDown(object sender, EvasKeyEventArgs e)
		{
			if (e.KeyName == "Return" && _gestureLayer.IsEnabled)
			{
				var cache = _handlerCache;
				if (cache.ContainsKey(GestureHandlerType.Tap))
				{
					foreach (var handler in cache[GestureHandlerType.Tap])
					{
						(handler as IGestureController)?.SendStarted(View, null);
						(handler as IGestureController)?.SendCompleted(View, null);
					}
				}
			}
		}
	}
}