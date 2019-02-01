using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace TestApp
{
	public class MaterialStylePage : DefaultPage
	{

		public MaterialStylePage() : base()
		{
			Title = "MaterialStylePage";

			Visual = VisualMarker.Material;
		}

	}
}
