using System;
using System.Linq;
using System.Collections.Generic;
using ElmSharp;
using EBox = ElmSharp.Box;

namespace Xamarin.Forms.Platform.Tizen.Native
{
	public class BoxContainer : EBox, IContainable<EvasObject>
	{
		IList<EvasObject> IContainable<EvasObject>.Children => base.Children.ToList<EvasObject>();

		public BoxContainer(EvasObject parent) : base(parent)
		{
		}

		
	}
}
