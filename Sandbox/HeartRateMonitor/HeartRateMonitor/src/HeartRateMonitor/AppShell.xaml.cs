using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HeartRateMonitor.Views;
using System.Diagnostics;

namespace HeartRateMonitor
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
            if (Shell.Current.CurrentState.Location.ToString() != "//Measurement")
            {
                Shell.Current.GoToAsync("//Measurement");
                return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}