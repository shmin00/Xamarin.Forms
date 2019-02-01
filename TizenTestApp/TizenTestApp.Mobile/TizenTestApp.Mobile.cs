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

			global::Xamarin.Forms.Platform.Tizen.Forms.SetFlags("Visual_Experimental");
			global::Xamarin.Forms.Platform.Tizen.Forms.Init(app);

			ElmSharp.Elementary.AddThemeOverlay("/usr/share/elementary/themes/xamarin-forms-material-tizen.edj");
			Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ add theme after forms.init");
			app.Run(args);
		}
	}
}
