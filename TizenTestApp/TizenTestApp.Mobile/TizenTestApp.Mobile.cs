using System;

namespace TizenTestApp
{
	class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
	{
		protected override void OnCreate()
		{
			base.OnCreate();

			LoadApplication(new App());
		}

		static void Main(string[] args)
		{
			var app = new Program();
			try
			{
				ElmSharp.Elementary.AddThemeOverlay("xamarin-forms-material-tizen");
			} catch(Exception e)
			{
				Console.WriteLine("Error: " + e.ToString());
			}
			
			global::Xamarin.Forms.Platform.Tizen.Forms.SetFlags("Visual_Experimental");
			global::Xamarin.Forms.Platform.Tizen.Forms.Init(app);
			app.Run(args);
		}
	}
}
