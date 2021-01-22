using System;
using System.ComponentModel;
using ElmSharp;

namespace Xamarin.Forms.Platform.Tizen
{
	public class TapGestureHandler : GestureHandler
	{
		public TapGestureHandler(IGestureRecognizer recognizer) : base(recognizer)
		{
		}

		public override GestureHandlerType Type
		{
			get
			{
				var recognizer = Recognizer as TapGestureRecognizer;
				if (recognizer != null)
				{
					int numberOfTaps = recognizer.NumberOfTapsRequired;

					if (numberOfTaps > 2)
						return GestureHandlerType.TripleTap;
					else if (numberOfTaps > 1)
						return GestureHandlerType.DoubleTap;
				}
				return GestureHandlerType.Tap;
			}
		}

		protected override void OnStarted(View sender, object data)
		{
		}

		protected override void OnMoved(View sender, object data)
		{
		}

		protected override void OnCompleted(View sender, object data)
		{
			(Recognizer as TapGestureRecognizer)?.SendTapped(sender);
		}

		protected override void OnCanceled(View sender, object data)
		{
		}
	}
}