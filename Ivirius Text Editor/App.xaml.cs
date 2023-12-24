//System usings
using System;

//Windows usings
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IviriusTextEditor.Pages;

namespace Ivirius_Text_Editor
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs Args)
        {
            var LocalSettings = ApplicationData.Current.LocalSettings;
            if ((string)LocalSettings.Values["Theme"] == "Slate Green")
            {
                var brush = new SolidColorBrush(Color.FromArgb(255, 92, 255, 138));
                Application.Current.Resources["SystemAccentColor"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorDark1"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorDark2"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorLight1"] = Color.FromArgb(255, 92, 255, 138);
                Application.Current.Resources["SystemAccentColorLight2"] = Color.FromArgb(255, 92, 255, 138);
            }
            //Configuring the title bar
            var TB = ApplicationView.GetForCurrentView().TitleBar;
            var CTB = CoreApplication.GetCurrentView().TitleBar;
            TB.ButtonInactiveBackgroundColor = Colors.Transparent;
            TB.ButtonBackgroundColor = Colors.Transparent;
            CTB.ExtendViewIntoTitleBar = true;

            if (!(Window.Current.Content is Frame RF))
            {
                //Sets the window content
                RF = new Frame();
                RF.NavigationFailed += OnNavigationFailed;
                if (Args.PreviousExecutionState == ApplicationExecutionState.Terminated) { }
                Window.Current.Content = RF;
            }

            if (Args.PrelaunchActivated == false)
            {
                if (RF.Content == null) { _ = RF.Navigate(typeof(MainPage), Args.Arguments); }
                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object Sender, NavigationFailedEventArgs Args)
        {
            throw new Exception($"Failed to load page {Args.SourcePageType.FullName}. Please report this bug as soon as possible");
        }

        private void OnSuspending(object Sender, SuspendingEventArgs Args)
        {
            try { var MP = new MainPage(); }
            finally
            {
                var Deff = Args.SuspendingOperation.GetDeferral();
                Deff.Complete();
            }
        }

        protected override void OnFileActivated(FileActivatedEventArgs Args)
        {
            base.OnFileActivated(Args);
            if (Window.Current.Content is Frame RF)
            {
                var X = RF.Content as MainPage;
                X.AddTabForFile(Args);
            }
            else
            {
                RF = new Frame();
                Window.Current.Content = RF;
                _ = RF.Navigate(typeof(MainPage), Args);
                Window.Current.Activate();
            }
        }

        protected override void OnActivated(IActivatedEventArgs Args)
        {
            if (Args.Kind == ActivationKind.Protocol)
            {
                if (!(Window.Current.Content is Frame))
                {
                    var RF = new Frame();
                    Window.Current.Content = RF;
                    _ = RF.Navigate(typeof(MainPage), Args);
                    Window.Current.Activate();
                }
                else { }
            }
        }
    }
}