using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Timer = System.Threading.Timer;
#pragma warning disable IDE0044

namespace Ivirius_Text_Editor
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer DT = new DispatcherTimer();

        public MainPage()
        {
            InitializeComponent();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += MainPage_CloseRequested;
            try
            {
                DT.Interval = TimeSpan.FromSeconds(0.1);
                DT.Start();
                DT.Tick += DT_Tick;
            }
            catch { }

            Window.Current.SetTitleBar(CustomDragRegion);
            var AppTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            AppTitleBar.ButtonBackgroundColor = Colors.Transparent;
            AppTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            AppTitleBar.ButtonInactiveForegroundColor = Colors.LightGray;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var LS = ApplicationData.Current.LocalSettings;
            TabbedView.TabItems.Clear();
            if (LS.Values["Username"] != null || (string)LS.Values["Username"] != "" || LS != null)
            {
                try
                {
                    UserWelcomeBox.Text = $"Welcome, {LS.Values["Username"]}!";
                }
                catch (Exception)
                {

                }
            }
            else
            {
                UserWelcomeBox.Text = "Welcome, user!";
            }
            if (LS.Values["SetupFinish"] != null)
            {
                if ((string)LS.Values["SetupFinish"] == "No")
                {
                    WelcomeToSetup.Visibility = Visibility.Visible;
                    SetupMainGrid.Visibility = Visibility.Visible;
                    var TBView = ApplicationView.GetForCurrentView();
                    _ = TBView.TryEnterFullScreenMode();
                }
                if ((string)LS.Values["SetupFinish"] == "Yes")
                {
                    WelcomeToSetup.Visibility = Visibility.Collapsed;
                    SetupMainGrid.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LS.Values["SetupFinish"] = "No";
                WelcomeToSetup.Visibility = Visibility.Visible;
                SetupMainGrid.Visibility = Visibility.Visible;
            }
            if (LS.Values["Password"] == null || (string)LS.Values["Password"] == "" || (string)LS.Values["Password"] == "args:passwordNullOrEmpty" || (string)LS.Values["Remember_me"] == "true")
            {
                LoginGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginGrid.Visibility = Visibility.Visible;
            }
            //Accent border settings
            if (LS.Values["AccentBorder"] != null)
            {
                if ((string)LS.Values["AccentBorder"] == "On")
                {
                    AccentBorder.Visibility = Visibility.Visible;
                }
                if ((string)LS.Values["AccentBorder"] == "Off")
                {
                    AccentBorder.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LS.Values["AccentBorder"] = "Off";
                AccentBorder.Visibility = Visibility.Collapsed;
            }
            //Theme settings
            if (LS.Values["Theme"] != null)
            {
                if ((string)LS.Values["Theme"] == "Light")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.White;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.White;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.GhostWhite;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 98;
                    AB.FallbackColor = Colors.GhostWhite;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.Black;
                    TabForegroundSelect2.Color = Colors.Black;
                    TabForegroundSelect3.Color = Colors.Black;
                    TabForegroundSelect4.Color = Colors.Black;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = AB.TintOpacity;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
                if ((string)LS.Values["Theme"] == "Dark")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.DarkGray;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.DarkGray;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.DimGray;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 90;
                    AB.FallbackColor = Colors.DimGray;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.White;
                    TabForegroundSelect2.Color = Colors.White;
                    TabForegroundSelect3.Color = Colors.White;
                    TabForegroundSelect4.Color = Colors.White;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = AB.TintOpacity;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
                if ((string)LS.Values["Theme"] == "Transparent")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.LightSteelBlue;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.LightSkyBlue;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.DarkSlateBlue;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.DarkGray;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.DarkGray;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.DimGray;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.01;
                    AB.FallbackColor = Colors.SteelBlue;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.White;
                    TabForegroundSelect2.Color = Colors.White;
                    TabForegroundSelect3.Color = Colors.White;
                    TabForegroundSelect4.Color = Colors.White;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = AB.TintOpacity;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
                if ((string)LS.Values["Theme"] == "Nostalgic Windows-old")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Color.FromArgb(50, 41, 84, 255);
                    AppTitleBar.ButtonPressedBackgroundColor = Color.FromArgb(50, 49, 77, 189);
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.Green;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.1;
                    AB.FallbackColor = Colors.LightSeaGreen;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Visible;
                    TabForegroundSelect.Color = Colors.Black;
                    TabForegroundSelect2.Color = Colors.Black;
                    TabForegroundSelect3.Color = Colors.Black;
                    TabForegroundSelect4.Color = Colors.Black;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = Colors.White;
                            TabAcrylicBrush.FallbackColor = Colors.White;
                            TabAcrylicBrush.TintOpacity = 0.6;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                    TabbedView.Margin = new Thickness(7, 0, 7, 7);
                    AeroShine.Visibility = Visibility.Visible;
                    AeroButtons.Visibility = Visibility.Visible;
                    AeroCorners.Visibility = Visibility.Visible;
                }
                if ((string)LS.Values["Theme"] == "Nostalgic Windows")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.White;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.White;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.GhostWhite;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.1;
                    AB.FallbackColor = Colors.GhostWhite;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.Black;
                    TabForegroundSelect2.Color = Colors.Black;
                    TabForegroundSelect3.Color = Colors.Black;
                    TabForegroundSelect4.Color = Colors.Black;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = 0.3;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
                if ((string)LS.Values["Theme"] == "Acrylic")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Color.FromArgb(100, 79, 146, 255);
                    AppTitleBar.ButtonPressedBackgroundColor = Color.FromArgb(100, 81, 117, 176);
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.Transparent;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.01;
                    AB.FallbackColor = Colors.Transparent;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Visible;
                    TabForegroundSelect.Color = Colors.Black;
                    TabForegroundSelect2.Color = Colors.Black;
                    TabForegroundSelect3.Color = Colors.Black;
                    TabForegroundSelect4.Color = Colors.Black;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = Colors.White;
                            TabAcrylicBrush.FallbackColor = Colors.White;
                            TabAcrylicBrush.TintOpacity = 0.6;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                    TabbedView.Margin = new Thickness(7, 0, 7, 7);
                    AeroShine.Visibility = Visibility.Visible;
                    AeroButtons.Visibility = Visibility.Visible;
                    AeroCorners.Visibility = Visibility.Visible;
                }
                if ((string)LS.Values["Theme"] == "Old")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.Gray;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.Blue;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.LightBlue;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.LightBlue;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.8;
                    AB.FallbackColor = Colors.LightBlue;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.Black;
                    TabForegroundSelect2.Color = Colors.Black;
                    TabForegroundSelect3.Color = Colors.Black;
                    TabForegroundSelect4.Color = Colors.Black;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = AB.TintOpacity;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
                if ((string)LS.Values["Theme"] == "Full Dark")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.Black;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.Black;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.4;
                    AB.FallbackColor = Colors.Black;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Dark;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.White;
                    TabForegroundSelect2.Color = Colors.White;
                    TabForegroundSelect3.Color = Colors.White;
                    TabForegroundSelect4.Color = Colors.White;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = AB.TintOpacity;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
                if ((string)LS.Values["Theme"] == "Slate Green")
                {
                    AppTitleBar.ButtonForegroundColor = Color.FromArgb(255, 191, 255, 209);
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.Black;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Color.FromArgb(255, 40, 54, 44);
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.4;
                    AB.FallbackColor = Color.FromArgb(255, 40, 54, 44);
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Dark;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.White;
                    TabForegroundSelect2.Color = Colors.White;
                    TabForegroundSelect3.Color = Colors.White;
                    TabForegroundSelect4.Color = Colors.White;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = 0.7;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
            }
            else
            {
                if (GetSysThemeBorder.RequestedTheme == ElementTheme.Dark)
                {
                    LS.Values["Theme"] = "Light";
                    AppTitleBar.ButtonForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.White;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.White;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.White;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 90;
                    AB.FallbackColor = Colors.White;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.Black;
                    TabForegroundSelect2.Color = Colors.Black;
                    TabForegroundSelect3.Color = Colors.Black;
                    TabForegroundSelect4.Color = Colors.Black;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = AB.TintOpacity;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
                else
                {
                    LS.Values["Theme"] = "Full Dark";
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.Black;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.Black;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.4;
                    AB.FallbackColor = Colors.Black;
                    MainPageComponent.Background = AB;
                    RequestedTheme = ElementTheme.Dark;
                    AeroBlue.Visibility = Visibility.Collapsed;
                    TabForegroundSelect.Color = Colors.White;
                    TabForegroundSelect2.Color = Colors.White;
                    TabForegroundSelect3.Color = Colors.White;
                    TabForegroundSelect4.Color = Colors.White;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    try
                    {
                        if ((string)LS.Values["AccentBackground"] == "On")
                        {
                            TabAcrylicBrush.TintColor = (AccentBorder.Background as AcrylicBrush).TintColor;
                            TabAcrylicBrush.FallbackColor = (AccentBorder.Background as AcrylicBrush).FallbackColor;
                            TabAcrylicBrush.TintOpacity = (AccentBorder.Background as AcrylicBrush).TintOpacity;
                            TabAcrylicBrush.BackgroundSource = (AccentBorder.Background as AcrylicBrush).BackgroundSource;
                        }
                        else
                        {
                            TabAcrylicBrush.TintColor = AB.TintColor;
                            TabAcrylicBrush.FallbackColor = AB.FallbackColor;
                            TabAcrylicBrush.TintOpacity = AB.TintOpacity;
                            TabAcrylicBrush.BackgroundSource = AB.BackgroundSource;
                        }
                    }
                    catch { return; }
                }
            }
        }

        private void DT_Tick(object sender, object e)
        {
            //var Y = TabbedView.TabItems;
            //foreach (var X in Y.Cast<TabViewItem>())
            //{
            //    var Z = ((X.Content as Frame).Content as TabbedMainPage).FileNameTextBlock.Text;
            //    X.Name = Z;
            //}
        }

        //public string answer;

        //public void GetAlphabet()
        //{
        //    string reversedalphabet = "";
        //    string alphabet = "!dellorkcir tog tsuj uoy loL ,nwod uoy tel annog reveN ,pu uoy evig annog reveN";
        //    while (alphabet != "")
        //    {
        //        string section = alphabet.Substring(alphabet.Length - 1);
        //        alphabet = alphabet.Remove(alphabet.Length - 1, 1);
        //        reversedalphabet += section;
        //    }
        //    answer = reversedalphabet;
        //}

        private async void MainPage_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            e.Handled = true;
            var X = new TabViewItem();
            if (SetupMainGrid.Visibility == Visibility.Collapsed)
            {
                if (TabbedView.TabItems.Count > 1)
                {
                    try
                    {
                        CloseWarningBox1.Open();
                        CloseWarningBox1.Title = "Are you sure you want to close?";
                        CWFContent1.Text = "There are multiple tabs open. If you haven't, save your work before closing";
                        //GetAlphabet();
                        //CWFContent1.Text = answer;
                    }
                    catch
                    {

                    }
                }
                if (TabbedView.TabItems.Count == 1)
                {
                    SysSender = TabbedView;
                    X = (TabViewItem)SysSender.TabItems[0];
                    if (((X.Content as Frame).Content as TabbedMainPage).WorkSaved.Visibility == Visibility.Collapsed)
                    {
                        try
                        {
                            CloseWarningBox2.Open();
                            CloseWarningBox2.Title = "Do you want to save your file?";
                            CWFContent2.Text = "You will have to close the app manually after saving and this dialog might appear again";
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        TabbedView.TabItems.Clear();
                    }
                }
                if (TabbedView.TabItems.Count < 1)
                {
                    _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }
                else if (TabbedView.TabItems.Count == 0)
                {
                    _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }
            }
            else
            {
                _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
            }
        }

        public async void RemoveCurrentTab(TabView Sender, TabViewTabCloseRequestedEventArgs EvArgs)
        {
            SysArgs = EvArgs;
            SysSender = Sender;
            if (((SysArgs.Tab.Content as Frame).Content as TabbedMainPage).WorkSaved.Visibility == Visibility.Collapsed)
            {
                CloseWarningBox3.Open();
                CloseWarningBox3.Title = "Ivirius Text Editor";
                CWFContent3.Text = "Do you want to save your file?";
            }
            else
            {
                _ = SysSender.TabItems.Remove(SysArgs.Tab);
                if (TabbedView.TabItems.Count == 0)
                {
                    _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }
            }
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            CloseWarningBox1.Close();
            CloseWarningBox2.Close();
            CloseWarningBox3.Close();
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (FlowDirection == FlowDirection.LeftToRight)
            {
                CustomDragRegion.MinWidth = sender.SystemOverlayRightInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayLeftInset;
            }
            else
            {
                CustomDragRegion.MinWidth = sender.SystemOverlayLeftInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayRightInset;
            }

            CustomDragRegion.Height = ShellTitlebarInset.Height = sender.Height;
        }

        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            sender.TabItems.Add(CreateNewTab());
            TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;
        }

        protected override void OnNavigatedTo(NavigationEventArgs EvArgs)
        {
            //Catch file
            base.OnNavigatedTo(EvArgs);
            if (EvArgs.Parameter is IActivatedEventArgs Args && Args.Kind == ActivationKind.File)
            {
                //Write file properties
                var NewTabItem = new TabViewItem();
                NewTabItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };
                var RF = new Frame();
                _ = RF.Navigate(typeof(TabbedMainPage), Args);
                RF.CornerRadius = new CornerRadius(4, 4, 4, 4);
                NewTabItem.Content = RF;
                _ = RF.Content as TabbedMainPage;
                NewTabItem.Header = "New Tab";
                TabbedView.TabItems.Add(NewTabItem);
                TabbedView.UpdateLayout();
            }
            else { }
        }

        public void AddTabForFile(FileActivatedEventArgs EvArgs)
        {
            //Catch file
            try
            {
                //Write file properties
                var NewTabItem = new TabViewItem();
                NewTabItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Windows.UI.Xaml.Controls.Symbol.Document };
                var RF = new Frame();
                _ = RF.Navigate(typeof(TabbedMainPage), EvArgs);
                _ = RF.Navigate(typeof(TabbedMainPage));
                RF.CornerRadius = new CornerRadius(4, 4, 4, 4);
                NewTabItem.Content = RF;
                NewTabItem.Header = "New Tab";
                TabbedView.TabItems.Add(NewTabItem);
                TabbedView.UpdateLayout();
                TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;
            }
            catch
            {

            }
        }

        public Task AddExternalTabAsync()
        {
            TabbedView.TabItems.Add(CreateNewTab());
            TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;
            return Task.CompletedTask;
        }

        public TabViewItem CreateNewTab()
        {
            var NewTabItem = new TabViewItem();
            NewTabItem.Header = "New Tab";
            NewTabItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Windows.UI.Xaml.Controls.Symbol.Document };
            var RF = new Frame();
            _ = RF.Navigate(typeof(TabbedMainPage));
            RF.CornerRadius = new CornerRadius(4, 4, 4, 4);
            NewTabItem.Content = RF;
            _ = RF.Content as TabbedMainPage;
            TabbedView.UpdateLayout();
            TabbedView.SelectedIndex = TabbedView.TabItems.Count;
            return NewTabItem;
        }

        private void TabbedView_Loaded(object sender, RoutedEventArgs e)
        {
            var S = sender as TabView;
            if (S.TabItems.Count == 0)
            {
                S.TabItems.Add(CreateNewTab());
            }
            TabbedView.UpdateLayout();
        }

        TabView SysSender;

        TabViewTabCloseRequestedEventArgs SysArgs;

        private void TabbedView_TabCloseRequested(TabView Sender, TabViewTabCloseRequestedEventArgs EvArgs)
        {
            RemoveCurrentTab(Sender, EvArgs);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WelcomeToSetup.Visibility = Visibility.Collapsed;
            ChooseYourTheme.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChooseYourTheme.Visibility = Visibility.Collapsed;
            GetLogIn.Visibility = Visibility.Visible;
            var LS = ApplicationData.Current.LocalSettings;
            if (ThemeListBox.SelectedIndex == 0)
            {
                LS.Values["Theme"] = "Light";
            }
            if (ThemeListBox.SelectedIndex == 1)
            {
                LS.Values["Theme"] = "Dark";
            }
            if (ThemeListBox.SelectedIndex == 2)
            {
                LS.Values["Theme"] = "Full Dark";
            }
            if (ThemeListBox.SelectedIndex == 3)
            {
                LS.Values["Theme"] = "Old";
            }
            if (ThemeListBox.SelectedIndex == 4)
            {
                LS.Values["Theme"] = "Nostalgic Windows";
            }
            if (ThemeListBox.SelectedIndex == 5)
            {
                LS.Values["Theme"] = "Acrylic";
            }
            if (ThemeListBox.SelectedIndex == 6)
            {
                LS.Values["Theme"] = "Transparent";
            }
            if (ThemeListBox.SelectedIndex == 7)
            {
                LS.Values["Theme"] = "Nostalgic Windows-old";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AccentChoose.Visibility = Visibility.Collapsed;
            AccesibilitySettings.Visibility = Visibility.Visible;
            var LS = ApplicationData.Current.LocalSettings;
            if (TitleBarAccent.IsChecked == true) LS.Values["AccentBorder"] = "On";
            if (TitleBarAccent.IsChecked == false) LS.Values["AccentBorder"] = "Off";
            if (BackgroundAccent.IsChecked == true) LS.Values["AccentBackground"] = "On";
            if (BackgroundAccent.IsChecked == false) LS.Values["AccentBackground"] = "Off";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AccesibilitySettings.Visibility = Visibility.Collapsed;
            EXPSettings.Visibility = Visibility.Visible;
            //Changelog settings
            var LS = ApplicationData.Current.LocalSettings;
            if (ChangelogCheck.IsChecked == true) LS.Values["Changelog"] = "On";
            if (ChangelogCheck.IsChecked == false) LS.Values["Changelog"] = "Off";
            if (HomepageCheck.IsChecked == true) LS.Values["HomePage"] = "On";
            if (HomepageCheck.IsChecked == false) LS.Values["HomePage"] = "Off";
            if (RulerCheck.IsChecked == true) LS.Values["Ruler"] = "On";
            if (RulerCheck.IsChecked == false) LS.Values["Ruler"] = "Off";
            if (StatusbarCheck.IsChecked == true) LS.Values["StatusBar"] = "On";
            if (StatusbarCheck.IsChecked == false) LS.Values["StatusBar"] = "Off";
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            EXPSettings.Visibility = Visibility.Collapsed;
            TipsAndNews.Visibility = Visibility.Visible;
            var LS = ApplicationData.Current.LocalSettings;
            if (EXPFeatures.IsChecked == true) LS.Values["EXP"] = "On";
            if (EXPFeatures.IsChecked == false) LS.Values["EXP"] = "Off";
            if (DevMode.IsChecked == true) LS.Values["DEV"] = "On";
            if (DevMode.IsChecked == false) LS.Values["DEV"] = "Off";
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            TipsAndNews.Visibility = Visibility.Collapsed;
            FinishPanel.Visibility = Visibility.Visible;
            var LS = ApplicationData.Current.LocalSettings;
            if (NewsCheck.IsChecked == true) LS.Values["News"] = "On";
            if (NewsCheck.IsChecked == false) LS.Values["News"] = "Off";
            if (TipsCheck.IsChecked == true) LS.Values["Tips"] = "On";
            if (TipsCheck.IsChecked == false) LS.Values["Tips"] = "Off";
        }

        string RestartArgs;

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            FinishPanel.Visibility = Visibility.Collapsed;
            SetupMainGrid.Visibility = Visibility.Collapsed;
            var LS = ApplicationData.Current.LocalSettings;
            LS.Values["SetupFinish"] = "Yes";
            WelcomeToSetup.Visibility = Visibility.Collapsed;
            SetupMainGrid.Visibility = Visibility.Collapsed;
            RestartArgs = "e";
            _ = await CoreApplication.RequestRestartAsync(RestartArgs);
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            WelcomeToSetup.Visibility = Visibility.Collapsed;
            SetupMainGrid.Visibility = Visibility.Collapsed;
            var LS = ApplicationData.Current.LocalSettings;
            LS.Values["SetupFinish"] = "Yes";
            LS.Values["Username"] = "user";
            LS.Values["Password"] = "args:passwordNullOrEmpty";
            WelcomeToSetup.Visibility = Visibility.Collapsed;
            SetupMainGrid.Visibility = Visibility.Collapsed;
            var TBView = ApplicationView.GetForCurrentView();
            TBView.ExitFullScreenMode();
            RestartArgs = "e";
            _ = await CoreApplication.RequestRestartAsync(RestartArgs);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            ChooseYourTheme.Visibility = Visibility.Collapsed;
            GetLogIn.Visibility = Visibility.Visible;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            AccentChoose.Visibility = Visibility.Collapsed;
            AccesibilitySettings.Visibility = Visibility.Visible;
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            AccesibilitySettings.Visibility = Visibility.Collapsed;
            EXPSettings.Visibility = Visibility.Visible;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EXPFeatures.IsEnabled = true;
            DevMode.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EXPFeatures.IsEnabled = false;
            DevMode.IsEnabled = false;
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/experimental-options-terms-and-conditions/"));
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            EXPSettings.Visibility = Visibility.Collapsed;
            TipsAndNews.Visibility = Visibility.Visible;
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            TipsAndNews.Visibility = Visibility.Collapsed;
            FinishPanel.Visibility = Visibility.Visible;
        }

        private void TabbedView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabbedView.UpdateLayout();
            TabbedView.AllowDropTabs = true;
            _ = TabbedView.Focus(FocusState.Pointer);
            TabbedView.IsEnabled = true;
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            var LS = ApplicationData.Current.LocalSettings;
            if (UPBox.Password == (string)LS.Values["Password"])
            {
                LoginGrid.Visibility = Visibility.Collapsed;
                PasswordWrongBox.Visibility = Visibility.Collapsed;
                LS.Values["Remember_me"] = "true";
            }
            else
            {
                PasswordWrongBox.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            var LS = ApplicationData.Current.LocalSettings;
            if (UPBox.Password == (string)LS.Values["Password"])
            {
                LoginGrid.Visibility = Visibility.Collapsed;
                PasswordWrongBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordWrongBox.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            var LS = ApplicationData.Current.LocalSettings;
            LS.Values["Username"] = "user";
            LS.Values["Password"] = "args:passwordNullOrEmpty";
            GetLogIn.Visibility = Visibility.Collapsed;
            AccentChoose.Visibility = Visibility.Visible;
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            var LS = ApplicationData.Current.LocalSettings;
            if (UserBox.Text != null || UserBox.Text != "") LS.Values["Username"] = UserBox.Text;
            else LS.Values["Username"] = "user";
            if (UserBox.Text != null || UserBox.Text != "") LS.Values["Password"] = CreatePWBox.Password;
            else LS.Values["Password"] = "args:passwordNullOrEmpty";
            GetLogIn.Visibility = Visibility.Collapsed;
            AccentChoose.Visibility = Visibility.Visible;
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            var FS = ApplicationView.GetForCurrentView().IsFullScreenMode;
            switch (FS)
            {
                case false:
                    _ = ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                    break;
                default:
                    ApplicationView.GetForCurrentView().ExitFullScreenMode();
                    break;
            }
        }

        private async void Button_Click_18(object sender, RoutedEventArgs e)
        {
            IList<AppDiagnosticInfo> infos = await AppDiagnosticInfo.RequestInfoForAppAsync();
            IList<AppResourceGroupInfo> resourceInfos = infos[0].GetResourceGroups();
            await resourceInfos[0].StartSuspendAsync();
        }

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            SetupCloseBox.Open();
        }

        private async void SetupCloseBox_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            var Win = ApplicationView.GetForCurrentView();
            await Win.TryConsolidateAsync();
        }

        private async void CloseWarningBox1_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
            CloseWarningBox1.Close();
        }

        private async void CloseWarningBox2_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            await (((TabbedView.SelectedItem as TabViewItem).Content as Frame).Content as TabbedMainPage).SaveFile(false, false, false, false);
            CloseWarningBox2.Close();
        }

        private async void CloseWarningBox2_SecondButtonClick(object sender, RoutedEventArgs e)
        {
            _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
            CloseWarningBox2.Close();
        }

        private async void CloseWarningBox3_FirstButtonClick(object sender, RoutedEventArgs e)
        {
            await ((SysArgs.Tab.Content as Frame).Content as TabbedMainPage).SaveFile(false, false, false, false);
            CloseWarningBox3.Close();
        }

        private async void CloseWarningBox3_SecondButtonClick(object sender, RoutedEventArgs e)
        {
            _ = SysSender.TabItems.Remove(SysArgs.Tab);
            if (TabbedView.TabItems.Count == 0) _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
            CloseWarningBox3.Close();
        }
    }
}