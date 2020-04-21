using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoiceMemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            if (Shell.Current.CurrentState.Location.ToString() != "//Main")
            {
                Shell.Current.GoToAsync("//Main");
                return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}