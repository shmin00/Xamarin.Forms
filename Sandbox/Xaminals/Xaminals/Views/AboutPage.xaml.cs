using System;
using System.Windows.Input;
//using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xaminals.Views
{
    public partial class AboutPage : ContentPage
    {
        public ICommand TapCommand => new Command<string>((str)=> { });

        public ICommand TapCommand21 => new Command(() => 
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

            stateButton.Text = $"Behavior: {Shell.Current.FlyoutBehavior.ToString()}, FlyoutIsPresented: {Shell.Current.FlyoutIsPresented}" ;
            Console.WriteLine($"#### Shell.Current.FlyoutBehavior={Shell.Current.FlyoutBehavior}");
        });

        public ICommand TapCommand22 => new Command(() =>
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;

            stateButton.Text = $"Behavior: {Shell.Current.FlyoutBehavior.ToString()}, FlyoutIsPresented: {Shell.Current.FlyoutIsPresented}";
            Console.WriteLine($"#### Shell.Current.FlyoutBehavior={Shell.Current.FlyoutBehavior}");
        });

        public ICommand TapCommand23 => new Command(() =>
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            stateButton.Text = $"Behavior: {Shell.Current.FlyoutBehavior.ToString()}, FlyoutIsPresented: {Shell.Current.FlyoutIsPresented}";
            Console.WriteLine($"#### Shell.Current.FlyoutBehavior={Shell.Current.FlyoutBehavior}");
        });

        public ICommand TapCommand3 => new Command(() => 
        {
            Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
            stateButton.Text = $"Behavior: {Shell.Current.FlyoutBehavior.ToString()}, FlyoutIsPresented: {Shell.Current.FlyoutIsPresented}";
        });

        public string ShellBehavior => Shell.Current.FlyoutBehavior.ToString();

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
