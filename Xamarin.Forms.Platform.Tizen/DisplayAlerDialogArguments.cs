using ElmSharp;
using System.Collections.Generic;
using Xamarin.Forms.Platform.Tizen.Native;

namespace Xamarin.Forms
{
	public class NativeDialogArguments
	{
		IList<object> _children = new List<object>();

		public IEnumerable<object> Children => _children;

		public Dialog Dialog { get; private set; }

		public NativeDialogArguments(Dialog dialog)
		{
			Dialog = dialog;
		}

		public void AddChild(object obj)
		{
			if (obj != null)
			{
				_children.Add(obj);
			}
		}
	}
}