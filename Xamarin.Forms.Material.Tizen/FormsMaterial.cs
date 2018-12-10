using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Tizen;

namespace Xamarin.Forms
{
	public static class FormsMaterial
	{
		static bool _initialized;

		public static void Init()
		{
			if(!_initialized)
			{
				//global::Xamarin.Forms.Platform.Tizen.Forms.SetFlags("Visual_Experimental");

				Log.Debug($" ****************************************** 1={System.Reflection.Assembly.GetExecutingAssembly().Location}");

				//var resources = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
				//foreach ( var r in resources)
				//{
				//	Log.Debug($" ****************************************** r={r}");
				//}



				_initialized = true;
			}			
		}
	}
}
