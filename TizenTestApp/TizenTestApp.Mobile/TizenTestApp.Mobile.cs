using System;
using Tizen.NET.MaterialComponents;

namespace TizenTestApp
{
	class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
	{
		protected override void OnCreate()
		{
			base.OnCreate();

			var resDir = DirectoryInfo.Resource;
			ThemeLoader.Initialize(resDir);
			LoadApplication(new App());
		}

		static void Main(string[] args)
		{
			var app = new Program();
			
			global::Xamarin.Forms.Platform.Tizen.Forms.SetFlags("Visual_Experimental");
			global::Xamarin.Forms.FormsMaterial.Init();
			global::Xamarin.Forms.Platform.Tizen.Forms.Init(app);
			app.Run(args);
		}
	}
}
