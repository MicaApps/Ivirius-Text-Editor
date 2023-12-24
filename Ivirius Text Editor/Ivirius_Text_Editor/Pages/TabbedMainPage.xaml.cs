//Microsoft toolkit usings
using ColorCode.Compilation.Languages;
using Humanizer;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
//System data usings
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//Windows components usings
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Chat;
using Windows.ApplicationModel.Core;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Graphics.Printing;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;
using static Humanizer.In;
#pragma warning disable IDE0044

namespace Ivirius_Text_Editor
{
    public sealed partial class TabbedMainPage : Page
    {
        DispatcherTimer DT = new DispatcherTimer();
        readonly DispatcherTimer DTSave = new DispatcherTimer();
        public bool IsCloseRequestComplete = false;

        #region Page

        public TabbedMainPage()
        {
            #region Miscellaneous

            //Component initialization
            InitializeComponent();
            RequestedTheme = ElementTheme.Light;
            OutputBox.Text = "> Output \n";
            REB.Document.GetText(TextGetOptions.FormatRtf, out var REBTextValue);
            DTSave.Interval = new TimeSpan(0, 0, (int)1.0);
            DTSave.Tick += DTSave_Tick;
            TriggerAutosave();
            LoadZippySetting();

            //Variables
            var LocalSettings = ApplicationData.Current.LocalSettings;
            var AppTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            var AppCoreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            //List initialization

            ThemeBox.Items.Add("Light");
            ThemeBox.Items.Add("Dark");
            ThemeBox.Items.Add("Full Dark");
            ThemeBox.Items.Add("Slate Green");
            ThemeBox.Items.Add("Acrylic");
            ThemeBox.Items.Add("Acrylic Glass");
            ThemeBox.Items.Add("Old");
            ThemeBox.Items.Add("Transparent");
            ThemeBox.Items.Add("Nostalgic Windows-old");


            //Get username
            if (LocalSettings.Values["Username"] == null || (string)LocalSettings.Values["Username"] == "" || !(LocalSettings == null))
            {
                HomeWelcomeText.Text = "Welcome, user!";
            }
            if (!(LocalSettings.Values["Username"] == null) || !((string)LocalSettings.Values["Username"] == "") || !(LocalSettings == null))
            {
                try { HomeWelcomeText.Text = $"Welcome, {LocalSettings.Values["Username"]}!"; }
                catch (Exception) { }
            }
            else { HomeWelcomeText.Text = "Welcome, user!"; }

            //Get password
            if (LocalSettings.Values["Password"] == null || (string)LocalSettings.Values["Password"] == "" || (string)LocalSettings.Values["Password"] == "args:passwordNullOrEmpty" || LocalSettings != null)
            {
                LogOutItem.IsEnabled = false;
                LogOutItem.Visibility = Visibility.Collapsed;
            }
            if (!(LocalSettings.Values["Password"] == null) || !((string)LocalSettings.Values["Password"] == "") || !((string)LocalSettings.Values["Password"] == "args:passwordNullOrEmpty") || !(LocalSettings == null))
            {
                LogOutItem.IsEnabled = true;
                LogOutItem.Visibility = Visibility.Visible;
            }
            else
            {
                LogOutItem.IsEnabled = false;
                LogOutItem.Visibility = Visibility.Collapsed;
            }

            #endregion Miscellaneous

            #region Settings

            //Theme settings
            if (!(LocalSettings.Values["Theme"] == null))
            {
                if ((string)LocalSettings.Values["Theme"] == "Light")
                {
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
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.GhostWhite;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 98;
                    AB2.FallbackColor = Colors.GhostWhite;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
                if ((string)LocalSettings.Values["Theme"] == "Dark")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.DarkGray;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.DarkGray;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.LightGray;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 98;
                    AB.FallbackColor = Colors.LightGray;
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.DimGray;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 90;
                    AB2.FallbackColor = Colors.DimGray;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
                if ((string)LocalSettings.Values["Theme"] == "Nostalgic Windows")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.White;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.White;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.White;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.6;
                    AB.FallbackColor = Colors.White;
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.GhostWhite;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 0.3;
                    AB2.FallbackColor = Colors.GhostWhite;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
                if ((string)LocalSettings.Values["Theme"] == "Acrylic")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.White;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.8;
                    AB.FallbackColor = Colors.White;
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.White;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 0.6;
                    AB2.FallbackColor = Colors.White;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
                if ((string)LocalSettings.Values["Theme"] == "Old")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.Black;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.Gray;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.Blue;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.LightBlue;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.LightCyan;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.9;
                    AB.FallbackColor = Colors.LightCyan;
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.LightBlue;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 0.8;
                    AB2.FallbackColor = Colors.LightBlue;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
                if ((string)LocalSettings.Values["Theme"] == "Slate Green")
                {
                    AppTitleBar.ButtonForegroundColor = Color.FromArgb(255, 191, 255, 209);
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.Black;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.Black;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.9;
                    AB.FallbackColor = Colors.Black;
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Color.FromArgb(255, 40, 54, 44);
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 0.7;
                    AB2.FallbackColor = Color.FromArgb(255, 40, 54, 44);
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    RequestedTheme = ElementTheme.Dark;
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
                if ((string)LocalSettings.Values["Theme"] == "Full Dark")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.Black;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.Black;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.9;
                    AB.FallbackColor = Colors.Black;
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.Black;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 0.4;
                    AB2.FallbackColor = Colors.Black;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    RequestedTheme = ElementTheme.Dark;
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
                if ((string)LocalSettings.Values["Theme"] == "Transparent")
                {
                    AppTitleBar.ButtonForegroundColor = Colors.SteelBlue;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.SkyBlue;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.DarkSlateBlue;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.DarkGray;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.DarkGray;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.SteelBlue;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.01;
                    AB.FallbackColor = Colors.SteelBlue;
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.Black;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 0.01;
                    AB2.FallbackColor = Colors.WhiteSmoke;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    AeroBlue.Visibility = Visibility.Collapsed;
                    RequestedTheme = ElementTheme.Light;
                }
                if ((string)LocalSettings.Values["Theme"] == "Nostalgic Windows-old")
                {
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.Black;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 0.4;
                    AB2.FallbackColor = Colors.Black;
                    PageTitleBar.Background = AB2;
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    AeroBlue.Visibility = Visibility.Collapsed;
                    RequestedTheme = ElementTheme.Light;
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
                    RequestedTheme = ElementTheme.Light;
                    AeroBlue.Visibility = Visibility.Visible;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                }
            }
            else
            {
                if (GetSysThemeBorder.RequestedTheme == ElementTheme.Dark)
                {
                    LocalSettings.Values["Theme"] = "Light";
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
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.GhostWhite;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 98;
                    AB2.FallbackColor = Colors.GhostWhite;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.Black);
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
                else
                {
                    LocalSettings.Values["Theme"] = "Full Dark";
                    AppTitleBar.ButtonForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverForegroundColor = Colors.White;
                    AppTitleBar.ButtonPressedForegroundColor = Colors.White;
                    AppTitleBar.ButtonHoverBackgroundColor = Colors.Black;
                    AppTitleBar.ButtonPressedBackgroundColor = Colors.Black;
                    var AB = new AcrylicBrush();
                    AB.TintColor = Colors.Black;
                    AB.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB.TintOpacity = 0.9;
                    AB.FallbackColor = Colors.Black;
                    ToolbarAccent.Background = AB;
                    var AB2 = new AcrylicBrush();
                    AB2.TintColor = Colors.Black;
                    AB2.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    AB2.TintOpacity = 0.4;
                    AB2.FallbackColor = Colors.Black;
                    PageTitleBar.Background = AB2;
                    ToolbarTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    BorTextAccent.Foreground = new SolidColorBrush(Colors.White);
                    RequestedTheme = ElementTheme.Dark;
                    AeroBlue.Visibility = Visibility.Collapsed;
                }
            }

            //Accent border settings
            if (!(LocalSettings.Values["AccentBorder"] == null))
            {
                if ((string)LocalSettings.Values["AccentBorder"] == "On")
                {
                    AccentBorder.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["AccentBorder"] == "Off")
                {
                    AccentBorder.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["AccentBorder"] = "Off";
                AccentBorder.Visibility = Visibility.Collapsed;
            }

            //Happiness
            if (LocalSettings.Values["Happiness"] != null)
            {
                CheckZippyHappiness();
            }
            else
            {
                LocalSettings.Values["Happiness"] = 150;
            }

            //Accent border settings
            if (!(LocalSettings.Values["Autosave"] == null))
            {
                if ((string)LocalSettings.Values["Autosave"] == "On")
                {
                    AutoSaveSwitch.IsOn = true;
                }
                if ((string)LocalSettings.Values["Autosave"] == "Off")
                {
                    AutoSaveSwitch.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Autosave"] = "On";
                AutoSaveSwitch.IsOn = true;
            }

            //Console settings
            if (!(LocalSettings.Values["ConsoleBoot"] == null))
            {
                if ((string)LocalSettings.Values["ConsoleBoot"] == "On")
                {
                    try
                    {
                        DT.Interval = TimeSpan.FromSeconds(1.0);
                        DT.Start();
                        DT.Tick += ConsoleBootDT_Tick;
                    }
                    catch { }
                }
                if ((string)LocalSettings.Values["ConsoleBoot"] == "Off") { }
            }
            else { LocalSettings.Values["ConsoleBoot"] = "Off"; }

            //Dev settings
            if (!(LocalSettings.Values["DEV"] == null))
            {
                if ((string)LocalSettings.Values["DEV"] == "On")
                {
                    ConsoleItem.IsEnabled = true;
                    ConsoleItem.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["DEV"] == "Off")
                {
                    ConsoleItem.IsEnabled = false;
                    ConsoleItem.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["DEV"] = "Off";
                ConsoleItem.IsEnabled = false;
                ConsoleItem.Visibility = Visibility.Collapsed;
            }

            //Accent background settings
            if (!(LocalSettings.Values["AccentBackground"] == null))
            {
                if ((string)LocalSettings.Values["AccentBackground"] == "On")
                {
                    AccentBackground.Visibility = Visibility.Visible;
                    HomeAccentBackground.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["AccentBackground"] == "Off")
                {
                    AccentBackground.Visibility = Visibility.Collapsed;
                    HomeAccentBackground.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["AccentBackground"] = "Off";
                AccentBackground.Visibility = Visibility.Collapsed;
                HomeAccentBackground.Visibility = Visibility.Collapsed;
            }

            //Changelog settings
            if (LocalSettings.Values["Changelog"] != null)
            {
                if ((string)LocalSettings.Values["Changelog"] == "On")
                {
                    ChangelogButton.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Changelog"] == "Off")
                {
                    ChangelogButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Changelog"] = "On";
                ChangelogButton.Visibility = Visibility.Visible;
            }

            //Cut settings
            if (LocalSettings.Values["Cut"] != null)
            {
                if ((string)LocalSettings.Values["Cut"] == "On")
                {
                    CTBBar.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Cut"] == "Off")
                {
                    CTBBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Cut"] = "On";
                CTBBar.Visibility = Visibility.Visible;
            }

            //Copy settings
            if (LocalSettings.Values["Copy"] != null)
            {
                if ((string)LocalSettings.Values["Copy"] == "On")
                {
                    CBBar.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Copy"] == "Off")
                {
                    CBBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Copy"] = "On";
                CBBar.Visibility = Visibility.Visible;
            }

            //Paste settings
            if (LocalSettings.Values["Paste"] != null)
            {
                if ((string)LocalSettings.Values["Paste"] == "On")
                {
                    PBBar.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Paste"] == "Off")
                {
                    PBBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Paste"] = "On";
                PBBar.Visibility = Visibility.Visible;
            }

            //New settings
            if (LocalSettings.Values["New"] != null)
            {
                if ((string)LocalSettings.Values["New"] == "On")
                {
                    TBNewB.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["New"] == "Off")
                {
                    TBNewB.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["New"] = "Off";
                TBNewB.Visibility = Visibility.Collapsed;
            }

            //Open settings
            if (LocalSettings.Values["Open"] != null)
            {
                if ((string)LocalSettings.Values["Open"] == "On")
                {
                    TBOpenB.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Open"] == "Off")
                {
                    TBOpenB.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Open"] = "Off";
                TBOpenB.Visibility = Visibility.Collapsed;
            }

            //Print settings
            if (LocalSettings.Values["Print"] != null)
            {
                if ((string)LocalSettings.Values["Print"] == "On")
                {
                    TBPrintB.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Print"] == "Off")
                {
                    TBPrintB.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Print"] = "Off";
                TBPrintB.Visibility = Visibility.Collapsed;
            }

            //Delete settings
            if (LocalSettings.Values["Delete"] != null)
            {
                if ((string)LocalSettings.Values["Delete"] == "On")
                {
                    DelBBar.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Delete"] == "Off")
                {
                    DelBBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Delete"] = "On";
                DelBBar.Visibility = Visibility.Visible;
            }

            //Home page settings
            if (LocalSettings.Values["HomePage"] != null)
            {
                if ((string)LocalSettings.Values["HomePage"] == "On")
                {
                    HomePage.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["HomePage"] == "Off")
                {
                    HomePage.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["HomePage"] = "Off";
                HomePage.Visibility = Visibility.Collapsed;
            }

            //Ruler settings
            if (!(LocalSettings.Values["Ruler"] == null))
            {
                if ((string)LocalSettings.Values["Ruler"] == "On")
                {
                    SCR3.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Ruler"] == "Off")
                {
                    SCR3.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Ruler"] = "On";
                SCR3.Visibility = Visibility.Visible;
            }

            //Toolbar settings
            if (!(LocalSettings.Values["Toolbar"] == null))
            {
                if ((string)LocalSettings.Values["Toolbar"] == "On")
                {

                }
                if ((string)LocalSettings.Values["Toolbar"] == "Off")
                {
                    Bor.Background = null;
                }
            }
            else
            {
                LocalSettings.Values["Toolbar"] = "On";
            }

            //Status bar settings
            if (!(LocalSettings.Values["StatusBar"] == null))
            {
                if ((string)LocalSettings.Values["StatusBar"] == "On")
                {
                    StatusBar.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["StatusBar"] == "Off")
                {
                    StatusBar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["StatusBar"] = "On";
                StatusBar.Visibility = Visibility.Visible;
            }

            //EXP settings
            if (LocalSettings.Values["EXP"] != null)
            {
                if ((string)LocalSettings.Values["EXP"] == "On")
                {
                    OldHome.Visibility = Visibility.Collapsed;
                    NewHome.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["EXP"] == "Off")
                {
                    OldHome.Visibility = Visibility.Visible;
                    NewHome.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["EXP"] = "On";
                OldHome.Visibility = Visibility.Collapsed;
                NewHome.Visibility = Visibility.Visible;
            }

            //News settings
            if (LocalSettings.Values["News"] != null)
            {
                if ((string)LocalSettings.Values["News"] == "On")
                {
                    NewsItem.Visibility = Visibility.Visible;
                    StoreItem.Visibility = Visibility.Visible;
                    WebsiteItem.Visibility = Visibility.Visible;
                    BugItem.Visibility = Visibility.Visible;
                    NewsGrid.Visibility = Visibility.Visible;
                    YoutubeItem.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["News"] == "Off")
                {
                    NewsItem.Visibility = Visibility.Collapsed;
                    StoreItem.Visibility = Visibility.Collapsed;
                    WebsiteItem.Visibility = Visibility.Collapsed;
                    BugItem.Visibility = Visibility.Collapsed;
                    NewsGrid.Visibility = Visibility.Collapsed;
                    YoutubeItem.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["News"] = "On";
                NewsItem.Visibility = Visibility.Visible;
                StoreItem.Visibility = Visibility.Visible;
                WebsiteItem.Visibility = Visibility.Visible;
                BugItem.Visibility = Visibility.Visible;
                NewsGrid.Visibility = Visibility.Visible;
                YoutubeItem.Visibility = Visibility.Visible;
            }

            //Tips settings
            if (LocalSettings.Values["Tips"] != null)
            {
                if ((string)LocalSettings.Values["Tips"] == "On")
                {
                    TipsGrid.Visibility = Visibility.Visible;
                }
                if ((string)LocalSettings.Values["Tips"] == "Off")
                {
                    TipsGrid.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LocalSettings.Values["Tips"] = "On";
                TipsGrid.Visibility = Visibility.Visible;

            }

            #endregion Settings

            #region Components

            //Document config
            REB.Document.Selection.CharacterFormat.Size = (float)10.5;
            RTB.Document.Selection.CharacterFormat.Size = (float)10.5;
            BackPicker.Color = Color.FromArgb(0, 255, 255, 255);

            //Title bar config
            AppCoreTitleBar.ExtendViewIntoTitleBar = true;

            //Fonts config
            var Fonts = CanvasTextFormat.GetSystemFontFamilies().OrderBy(Font => Font).ToList();
            Fonts.Add("");
            FontBox.ItemsSource = Fonts;
            FontBox.SelectedItem = "Segoe UI";

            //Size config
            SizeBox.Items.Add("A4");
            SizeBox.Items.Add("Letter");
            SizeBox.Items.Add("Tabloid");
            SizeBox.SelectedItem = "A4";

            #endregion Components

            #region SettingsComponents

            //Theme settings
            if (!(LocalSettings.Values["Theme"] == null))
            {
                if ((string)LocalSettings.Values["Theme"] == "Light")
                {
                    ThemeBox.SelectedItem = "Light";
                }
                if ((string)LocalSettings.Values["Theme"] == "Dark")
                {
                    ThemeBox.SelectedItem = "Dark";
                }
                if ((string)LocalSettings.Values["Theme"] == "Nostalgic Windows")
                {
                    ThemeBox.SelectedItem = "Acrylic";
                }
                if ((string)LocalSettings.Values["Theme"] == "Nostalgic Windows-old")
                {
                    ThemeBox.SelectedItem = "Nostalgic Windows-old";
                }
                if ((string)LocalSettings.Values["Theme"] == "Transparent")
                {
                    ThemeBox.SelectedItem = "Transparent";
                }
                if ((string)LocalSettings.Values["Theme"] == "Acrylic")
                {
                    ThemeBox.SelectedItem = "Acrylic Glass";
                }
                if ((string)LocalSettings.Values["Theme"] == "Old")
                {
                    ThemeBox.SelectedItem = "Old";
                }
                if ((string)LocalSettings.Values["Theme"] == "Full Dark")
                {
                    ThemeBox.SelectedItem = "Full Dark";
                }
                if ((string)LocalSettings.Values["Theme"] == "Slate Green")
                {
                    ThemeBox.SelectedItem = "Slate Green";
                }
            }
            else
            {
                LocalSettings.Values["Theme"] = "Light";
                ThemeBox.SelectedItem = "Light";
            }

            //Spell check
            if (LocalSettings.Values["SCheck"] != null)
            {
                if ((string)LocalSettings.Values["SCheck"] == "On")
                {
                    SCheckBox.IsChecked = true;
                    SCheckOn();

                }
                if ((string)LocalSettings.Values["SCheck"] == "Off")
                {
                    SCheckBox.IsChecked = false;
                    SCheckOff();
                }
            }
            else
            {
                LocalSettings.Values["SCheck"] = "Off";
                SCheckBox.IsChecked = false;
                SCheckOff();
            }

            //Accent border settings
            if (!(LocalSettings.Values["AccentBorder"] == null))
            {
                if ((string)LocalSettings.Values["AccentBorder"] == "On")
                {
                    AccentBorderToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["AccentBorder"] == "Off")
                {
                    AccentBorderToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["AccentBorder"] = "Off";
                AccentBorderToggle.IsOn = false;
            }

            //Accent background settings
            if (!(LocalSettings.Values["AccentBackground"] == null))
            {
                if ((string)LocalSettings.Values["AccentBackground"] == "On")
                {
                    AccentBackgroundToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["AccentBackground"] == "Off")
                {
                    AccentBackgroundToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["AccentBackground"] = "Off";
                AccentBackgroundToggle.IsOn = false;
            }

            //Changelog settings
            if (!(LocalSettings.Values["Changelog"] == null))
            {
                if ((string)LocalSettings.Values["Changelog"] == "On")
                {
                    ChangelogToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Changelog"] == "Off")
                {
                    ChangelogToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Changelog"] = "On";
                ChangelogToggle.IsOn = true;
            }

            //Cut settings
            if (!(LocalSettings.Values["Cut"] == null))
            {
                if ((string)LocalSettings.Values["Cut"] == "On")
                {
                    CutToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Cut"] == "Off")
                {
                    CutToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Cut"] = "On";
                CutToggle.IsOn = true;
            }

            //Copy settings
            if (!(LocalSettings.Values["Copy"] == null))
            {
                if ((string)LocalSettings.Values["Copy"] == "On")
                {
                    CopyToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Copy"] == "Off")
                {
                    CopyToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Copy"] = "On";
                CopyToggle.IsOn = true;
            }

            //Paste settings
            if (!(LocalSettings.Values["Paste"] == null))
            {
                if ((string)LocalSettings.Values["Paste"] == "On")
                {
                    PasteToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Paste"] == "Off")
                {
                    PasteToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Paste"] = "On";
                PasteToggle.IsOn = true;
            }

            //New settings
            if (!(LocalSettings.Values["New"] == null))
            {
                if ((string)LocalSettings.Values["New"] == "On")
                {
                    NewToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["New"] == "Off")
                {
                    NewToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["New"] = "On";
                NewToggle.IsOn = true;
            }

            //Open settings
            if (!(LocalSettings.Values["Open"] == null))
            {
                if ((string)LocalSettings.Values["Open"] == "On")
                {
                    OpenToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Open"] == "Off")
                {
                    OpenToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Open"] = "On";
                OpenToggle.IsOn = true;
            }

            //Print settings
            if (!(LocalSettings.Values["Print"] == null))
            {
                if ((string)LocalSettings.Values["Print"] == "On")
                {
                    PrintToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Print"] == "Off")
                {
                    PrintToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Print"] = "On";
                PrintToggle.IsOn = true;
            }

            //Delete settings
            if (!(LocalSettings.Values["Delete"] == null))
            {
                if ((string)LocalSettings.Values["Delete"] == "On")
                {
                    DeleteToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Delete"] == "Off")
                {
                    DeleteToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Delete"] = "On";
                DeleteToggle.IsOn = true;
            }

            //Home page settings
            if (!(LocalSettings.Values["HomePage"] == null))
            {
                if ((string)LocalSettings.Values["HomePage"] == "On")
                {
                    HomeToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["HomePage"] == "Off")
                {
                    HomeToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["HomePage"] = "Off";
                HomeToggle.IsOn = false;
            }

            //Ruler settings
            if (!(LocalSettings.Values["Ruler"] == null))
            {
                if ((string)LocalSettings.Values["Ruler"] == "On")
                {
                    RulerToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Ruler"] == "Off")
                {
                    RulerToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Ruler"] = "On";
                RulerToggle.IsOn = true;
            }

            //Toolbar settings
            if (!(LocalSettings.Values["Toolbar"] == null))
            {
                if ((string)LocalSettings.Values["Toolbar"] == "On")
                {
                    ToolbarBackgroundToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["Toolbar"] == "Off")
                {
                    ToolbarBackgroundToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["Toolbar"] = "On";
                ToolbarBackgroundToggle.IsOn = true;
            }

            //Status bar settings
            if (!(LocalSettings.Values["StatusBar"] == null))
            {
                if ((string)LocalSettings.Values["StatusBar"] == "On")
                {
                    StatusBarToggle.IsOn = true;
                }
                if ((string)LocalSettings.Values["StatusBar"] == "Off")
                {
                    StatusBarToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["StatusBar"] = "On";
                StatusBarToggle.IsOn = true;
            }

            //EXP settings
            if (!(LocalSettings.Values["EXP"] == null))
            {
                if ((string)LocalSettings.Values["EXP"] == "On")
                {

                }
                if ((string)LocalSettings.Values["EXP"] == "Off")
                {

                }
            }
            else
            {
                LocalSettings.Values["EXP"] = "On";
            }

            //News settings
            if (!(LocalSettings.Values["News"] == null))
            {
                if ((string)LocalSettings.Values["News"] == "On")
                {

                }
                if ((string)LocalSettings.Values["News"] == "Off")
                {

                }
            }
            else
            {
                LocalSettings.Values["News"] = "On";
            }

            //Tips settings
            if (!(LocalSettings.Values["Tips"] == null))
            {
                if ((string)LocalSettings.Values["Tips"] == "On")
                {

                }
                if ((string)LocalSettings.Values["Tips"] == "Off")
                {

                }
            }
            else
            {
                LocalSettings.Values["Tips"] = "Off";
            }

            #endregion SettingsComponents
        }

        #endregion Page

        #region File

        public StorageFile TXTFile;

        public IRandomAccessStream RAS;

        private readonly PrintHelperOptions PP = new PrintHelperOptions();

        #region Buttons

        private void NewFile_Click(object Sender, RoutedEventArgs EvArgs)
        {
            Zippy.Source = new Uri("ms-appx:///ZippyFolder.mp4");

            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
            if (Value == SecValue)
            {
                RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                var ValueRTB = RichEditBoxConverter.GetText(RTB);

                REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                REB.Document.SetText(TextSetOptions.FormatRtf, "");
                var ValueREB = RichEditBoxConverter.GetText(REB);

                RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                HomePage.Visibility = Visibility.Collapsed;
                TXTFile = null;
                CheckForSaving();
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, false, true, false);
                    else if (TXTFile == null) await SaveFile(false, false, true, false);
                    HomePage.Visibility = Visibility.Collapsed;
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                    var ValueRTB = RichEditBoxConverter.GetText(RTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, "");
                    var ValueREB = RichEditBoxConverter.GetText(REB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                    HomePage.Visibility = Visibility.Collapsed;
                    TXTFile = null;
                    CheckForSaving();
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
        }

        private void DelB_Click(object sender, RoutedEventArgs e)
        {
            if (TXTFile != null)
            {
                if (TXTFile.IsAvailable != true)
                {
                    FileNotSavedInfoBar.Title = "There is no file available";
                    FileNotSavedInfoBar.IsOpen = true;
                    CheckForSaving();
                }
                ActionWarningBox.Open();
                ActionWarningMessage.Text = "Are you sure you want to delete this file?";
                ActionWarningBox.FirstButtonClick += ED2_PrimaryButtonClick;
                void ED2_PrimaryButtonClick(object Sender, RoutedEventArgs EvArgs)
                {
                    try
                    {
                        if (TXTFile != null)
                        {
                            _ = TXTFile.DeleteAsync();
                            TXTFile = null;
                            CheckForSaving();
                            ActionWarningBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                        }
                        else
                        {
                            CheckForSaving();
                            new ToastContentBuilder()
                            .SetToastScenario(ToastScenario.Reminder)
                            .AddText($"Your file could not be found on this computer or is currently in use by another application")
                            .AddButton(new ToastButton()
                                .SetDismissActivation().SetContent("Close"))
                            .Show();
                            ActionWarningBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                        }
                    }
                    catch (Exception Ex)
                    {
                        try
                        {
                            new ToastContentBuilder()
                            .SetToastScenario(ToastScenario.Reminder)
                            .AddText("It seems like Ivirius Text Editor has crashed.")
                            .AddText("These are the crash details:")
                            .AddText($"{Ex.Message}")
                            .AddButton(new ToastButton()
                                .SetDismissActivation().SetContent("Close"))
                            .AddButton(new ToastButton()
                            .SetProtocolActivation(new Uri("https://ivirius.webnode.page/contact/")).SetContent("Send bug report"))
                            .Show();
                            CheckForSaving();
                            ActionWarningBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                        }
                        catch
                        {
                            ActionWarningBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                        }
                        return;
                    }
                }
            }
            else
            {
                FileNotSavedInfoBar.Title = "There is no file available";
                FileNotSavedInfoBar.IsOpen = true;
                CheckForSaving();
            }
        }

        private async void OB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            Zippy.Source = new Uri("ms-appx:///ZippyFolder.mp4");

            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            if (Value == SecValue)
            {
                await Open();
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, false, false, false); else await SaveFile(false, false, false, false);
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                async void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    await Open();
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
        }

        PrintHelper PH;

        private async void PntB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure printing dialog
            Zippy.Source = new Uri("ms-appx:///ZippyPrint.mp4");


            // Calculate the available area for printing
            var printRect = PrintManager.GetForCurrentView();

            // Split the content into separate pages
            SCR.Content = null;
            var contentPages = SplitRichEditBoxForPrinting(REB, REB.Height);

            var PH = new PrintHelper(Container);
            foreach (FrameworkElement frameworkElement in contentPages)
            {
                PH.AddFrameworkElementToPrint(frameworkElement);
            }
            // Subscribe to the PrintHelper events
            PH.OnPrintFailed += PrintHelper_OnPrintFailed;
            PH.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;
            PH.OnPrintCanceled += PH_OnPrintCanceled;

            // Show the print UI
            await PH.ShowPrintUIAsync("New Rich Text File", PP);
        }

        private void PrintHelper_OnPrintSucceeded()
        {
            // Printing succeeded event
            PH.Dispose();
            SCR.Content = REB;
            PrintingSuccessBar.IsOpen = true;
        }

        private void PrintHelper_OnPrintFailed()
        {
            // Printing failed event
            PH.Dispose();
            PrintingFailBar.IsOpen = true;
            SCR.Content = REB;
        }

        private void PH_OnPrintCanceled()
        {
            // Printing canceled event
            SCR.Content = REB;
        }

        public string GetRTFContent(RichEditBox richEditBox, int startIndex, int endIndex)
        {
            // Get the RTF content from the specified start index to end index
            ITextRange textRange = richEditBox.Document.GetRange(startIndex, endIndex);
            textRange.GetText(TextGetOptions.FormatRtf, out string value);
            return value;
        }

        public int CalculateTotalPages(RichEditBox richEditBox)
        {
            double richEditBoxHeight = richEditBox.ActualHeight;
            double textContentHeight = GetTextContentHeight(richEditBox);

            int totalPages = (int)Math.Ceiling(textContentHeight / richEditBoxHeight);
            return totalPages;
        }

        public double GetTextDocumentHeight(RichEditBox richEditBox)
        {
            double documentHeight = 0;
            richEditBox.Document.GetText(TextGetOptions.None, out string text);

            var tb = new TextBlock();
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = text;
            tb.Measure(new Size(richEditBox.ActualWidth, double.PositiveInfinity));
            documentHeight = tb.DesiredSize.Height;

            return documentHeight;
        }

        public List<RichEditBox> SplitRichEditBoxForPrinting(RichEditBox richEditBox, double pageHeight)
        {
            List<RichEditBox> richEditBoxes = new List<RichEditBox>();
            int totalLines = EstimateTotalLines(richEditBox);
            int totalPages = 1;
            double textDocumentHeight = REB.ActualHeight;

            while (textDocumentHeight > REB.Height)
            {
                totalPages++;
                textDocumentHeight -= REB.Height;
            }

            int startIndex = 0;
            int remainingLines = totalLines;

            for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
            {
                int linesToCopy = GetLinesToFitPage(richEditBox, startIndex, remainingLines, pageHeight);
                int endIndex = GetEndIndexForLines(richEditBox, startIndex, linesToCopy);

                // Create a new RichEditBox for the current page
                RichEditBox pageRichEditBox = new RichEditBox();
                pageRichEditBox.Width = richEditBox.Width;
                pageRichEditBox.Height = richEditBox.Height;

                // Set the content of the current page from the original RichEditBox
                string pageContent = GetRTFContent(richEditBox, startIndex, endIndex);
                pageRichEditBox.Document.SetText(TextSetOptions.FormatRtf, pageContent);

                // Add the RichEditBox to the list
                richEditBoxes.Add(pageRichEditBox);

                // Update the start index for the next page
                startIndex = endIndex;
                remainingLines -= linesToCopy;
            }

            return richEditBoxes;
        }

        public int EstimateTotalLines(RichEditBox richEditBox)
        {
            double contentHeight = GetTextContentHeight(richEditBox);
            double lineHeight = richEditBox.ActualHeight / richEditBox.FontSize;

            int totalLines = (int)Math.Ceiling(contentHeight / lineHeight);
            return totalLines;
        }

        public int GetLinesToFitPage(RichEditBox richEditBox, int startIndex, int remainingLines, double pageHeight)
        {
            ITextDocument textDocument = richEditBox.Document;
            textDocument.GetText(TextGetOptions.None, out string value);
            ITextRange textRange = textDocument.GetRange(startIndex, value.Length - startIndex);
            int linesToCopy = 0;
            double totalHeight = 0;

            while (linesToCopy < remainingLines && totalHeight < pageHeight)
            {
                int found = textRange.FindText("\r", textRange.Length, FindOptions.None);
                if (found == -1)
                    break;
                linesToCopy++;
                totalHeight += richEditBox.ActualHeight / richEditBox.FontSize;
                textRange = textDocument.GetRange(startIndex, startIndex + found + 1);
                startIndex += found + 1;
            }

            return linesToCopy;
        }

        public int GetEndIndexForLines(RichEditBox richEditBox, int startIndex, int lines)
        {
            ITextDocument textDocument = richEditBox.Document;
            textDocument.GetText(TextGetOptions.None, out string value);
            ITextRange textRange = textDocument.GetRange(startIndex, value.Length - startIndex);
            int endIndex = startIndex;

            for (int i = 0; i < lines; i++)
            {
                int found = textRange.FindText("\r", textRange.Length, FindOptions.None);
                if (found == -1)
                    break;
                endIndex += found + 1;
                textRange = textDocument.GetRange(endIndex, value.Length - endIndex);
            }

            return endIndex;
        }

        public double GetTextContentHeight(RichEditBox richEditBox)
        {
            // Calculate the height of the text content
            double textHeight = 0.0;

            ITextDocument textDocument = richEditBox.Document;
            if (textDocument != null)
            {
                // Create a temporary RichTextBlock to measure the text height
                RichTextBlock tempRichTextBlock = new RichTextBlock();
                tempRichTextBlock.Width = richEditBox.Width;
                tempRichTextBlock.FontSize = richEditBox.FontSize;
                tempRichTextBlock.FontFamily = richEditBox.FontFamily;

                // Set the text content of the temporary RichTextBlock to the RichEditBox text content
                tempRichTextBlock.Blocks.Clear();
                tempRichTextBlock.Blocks.Add(new Paragraph
                {
                    Inlines =
            {
                new Run
                {
                    Text = RichEditBoxConverter.GetText(richEditBox),
                    FontStyle = richEditBox.FontStyle,
                    FontWeight = richEditBox.FontWeight,
                    Foreground = richEditBox.Foreground
                }
            }
                });

                // Measure the desired height of the temporary RichTextBlock
                tempRichTextBlock.Measure(new Windows.Foundation.Size(richEditBox.Width, double.PositiveInfinity));
                textHeight = tempRichTextBlock.DesiredSize.Height;
            }

            return textHeight;
        }

        private async void SAsB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            await SaveFile(false, false, false, false);
            Zippy.Source = new Uri("ms-appx:///ZippySave.mp4");

        }

        private async void SB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            if (TXTFile != null) await SaveFile(true, false, false, false);
            else await SaveFile(false, false, false, false);
            Zippy.Source = new Uri("ms-appx:///ZippySave.mp4");

        }

        #endregion Buttons

        #region Actions

        private void AutoSaveSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var LS = ApplicationData.Current.LocalSettings;
            if (AutoSaveSwitch != null)
            {
                _ = ApplicationView.GetForCurrentView().TitleBar;
                if (AutoSaveSwitch.IsOn == true)
                {
                    LS.Values["Autosave"] = "On";
                    TriggerAutosave();
                }
                if (AutoSaveSwitch.IsOn == false)
                {
                    LS.Values["Autosave"] = "Off";
                    TriggerAutosave();
                }
            }
            TriggerAutosave();
        }

        public void TriggerAutosave()
        {
            if (DTSave.IsEnabled != true) DTSave.Start();
        }

        private async void DTSave_Tick(object sender, object e)
        {
            if (AutoSaveSwitch.IsOn == true && TXTFile != null)
            {
                try
                {
                    if (TXTFile != null)
                    {
                        if (AutoSaveSwitch.IsOn == true)
                        {
                            await AutosaveWritingToFile();
                        }
                        else
                        {
                            SaveIndicatorGrid.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        SaveIndicatorGrid.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception EX)
                {
                    new ToastContentBuilder()
                    .SetToastScenario(ToastScenario.Reminder)
                    .AddText($"Ivirius Text Editor has crashed. These are the details:")
                    .AddText($"Debugger Output:")
                    .AddText($"Message:")
                    .AddText($"{EX.Message}")
                    .AddText($"Data:")
                    .AddText($"{EX.Data}")
                    .AddButton(new ToastButton()
                        .SetDismissActivation().SetContent("Close"))
                    .AddButton(new ToastButton()
                    .SetProtocolActivation(new Uri("https://ivirius.webnode.page/contact/")).SetContent("Send bug report"))
                    .Show();
                }
            }
            else
            {
                DTSave.Stop();
            }
        }

        public async Task AutosaveWritingToFile()
        {
            try
            {
                SaveIndicatorGrid.Visibility = Visibility.Collapsed;
                FileSaveBox.Close();
                var RAS = await TXTFile.OpenAsync(FileAccessMode.ReadWrite);
                REB.Document.SaveToStream(TextGetOptions.FormatRtf, RAS);
                try
                {
                    RTB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAS);
                }
                catch (AccessViolationException)
                {
                    ActionErrorMessage.Text = "The file you are trying to edit cannot be checked for saving";
                    ActionErrorBox.Open();
                }
                RAS.Dispose();
                var Stats = await CachedFileManager.CompleteUpdatesAsync(TXTFile);
                if (Stats == FileUpdateStatus.Complete)
                {
                    //Confirm file saving without close
                    CheckForSaving();
                }
                if (Stats != FileUpdateStatus.Complete)
                {
                    //File failed to save message
                    FileNotSavedInfoBar.Title = "File couldn't be saved";
                    FileNotSavedInfoBar.IsOpen = true;
                    CheckForSaving();
                }
            }
            catch
            {
                return;
            }
        }

        public void CheckForSaving()
        {
            string REBText = RichEditBoxConverter.GetText(REB);
            string RTBText = RichEditBoxConverter.GetText(RTB);

            if (REBText == RTBText && REBText == RichEditBoxConverter.GetText(EmptyRTB))
            {
                WorkSaved.Visibility = Visibility.Visible;
                WorkNotSaved.Visibility = Visibility.Collapsed;
                if (TXTFile != null)
                {
                    FileNameTextBlock.Text = TXTFile.DisplayName;
                }
                else
                {
                    FileNameTextBlock.Text = "Untitled";
                }
            }
            else if (REBText == RTBText && TXTFile != null)
            {
                WorkSaved.Visibility = Visibility.Visible;
                WorkNotSaved.Visibility = Visibility.Collapsed;
                if (TXTFile != null)
                {
                    FileNameTextBlock.Text = TXTFile.DisplayName;
                }
                else
                {
                    FileNameTextBlock.Text = "Untitled";
                }
            }
            else
            {
                WorkSaved.Visibility = Visibility.Collapsed;
                WorkNotSaved.Visibility = Visibility.Visible;
                if (TXTFile != null)
                {
                    FileNameTextBlock.Text = TXTFile.DisplayName;
                }
                else
                {
                    FileNameTextBlock.Text = "Untitled";
                }
            }
            TriggerAutosave();
        }

        public async Task SaveFile(bool Background, bool Close, bool Erase, bool NoFile)
        {
            var FSP = new FileSavePicker();
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);

            //----------Save Background----------

            if (Background == true && Close == false && Erase == false && NoFile == false)
            {
                //Check if file is null
                if (TXTFile == null)
                {
                    //If yes, show a dialog for saving
                    FileSaveBox.Close();
                    await SaveFile(false, false, false, false);
                }
                else if (TXTFile != null)
                {
                    //If no, save in the background
                    try
                    {
                        FileSaveBox.Close();
                        //Get a RandomAccessStream of the file
                        var RAS = await TXTFile.OpenAsync(FileAccessMode.ReadWrite);
                        try
                        {
                            //Save the RichEditBox contents to the stream
                            REB.Document.SaveToStream(TextGetOptions.FormatRtf, RAS);
                            //Open the file in a secondary stream
                            var RBS = await TXTFile.OpenAsync(FileAccessMode.Read);
                            //Open the file in the RichEditBox used for checking file saving
                            RTB.Document.LoadFromStream(TextSetOptions.FormatRtf, RBS);
                            //Complete file updates
                            var Stats = await CachedFileManager.CompleteUpdatesAsync(TXTFile);
                            if (Stats == FileUpdateStatus.Complete)
                            {
                                //Confirm file saving without close
                                new ToastContentBuilder()
                                .SetToastScenario(ToastScenario.Reminder)
                                .AddText($"Your file has been succesfully saved at {TXTFile.Path}")
                                .AddButton(new ToastButton()
                                    .SetDismissActivation().SetContent("Close"))
                                .Show();
                                CheckForSaving();
                            }
                            else
                            {
                                //File failed to save message
                                FileNotSavedInfoBar.Title = "File couldn't be saved";
                                FileNotSavedInfoBar.IsOpen = true;
                                CheckForSaving();
                            }
                            //Dispose the check stream
                            RBS.Dispose();
                        }
                        finally
                        {
                            //Dispose the main stream
                            RAS.Dispose();
                        }
                    }
                    catch
                    {
                        //Handle exceptions
                        FileNotSavedInfoBar.Title = "The file is currently in use either by the autosave option or another app";
                        FileNotSavedInfoBar.IsOpen = true;
                        CheckForSaving();
                    }
                }
            }


            //----------Save----------
            if (Background == false && Close == false && Erase == false && NoFile == false)
            {
                FileSaveBox.Close();
                //File dialog configuration
                FSP.FileTypeChoices.Add("Rich Ivirius Text", new List<string>() { ".ivrtext" });
                FSP.FileTypeChoices.Add("Universal Rich Text", new List<string>() { ".rtf" });
                FSP.FileTypeChoices.Add("Rich Text", new List<string>() { ".richtxtformat" });
                FSP.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                FSP.SuggestedFileName = "New Rich Text File";
                //Set file content
                TXTFile = await FSP.PickSaveFileAsync();
                if (TXTFile != null)
                {
                    var RAS = await TXTFile.OpenAsync(FileAccessMode.ReadWrite);
                    try
                    {
                        try
                        {
                            FileSaveBox.Close();
                            REB.Document.SaveToStream(TextGetOptions.FormatRtf, RAS);
                            var RBS = await TXTFile.OpenAsync(FileAccessMode.Read);
                            RTB.Document.LoadFromStream(TextSetOptions.FormatRtf, RBS);
                            var Stats = await CachedFileManager.CompleteUpdatesAsync(TXTFile);
                            if (Stats == FileUpdateStatus.Complete)
                            {
                                //Confirm file saving without close
                                new ToastContentBuilder()
                                .SetToastScenario(ToastScenario.Reminder)
                                .AddText($"Your file has been succesfully saved at {TXTFile.Path}")
                                .AddButton(new ToastButton()
                                    .SetDismissActivation().SetContent("Close"))
                                .Show();
                                CheckForSaving();
                            }
                            else
                            {
                                //File failed to save message
                                FileNotSavedInfoBar.Title = "File couldn't be saved";
                                FileNotSavedInfoBar.IsOpen = true;
                                CheckForSaving();
                            }
                            RBS.Dispose();
                        }
                        finally
                        {
                            RAS.Dispose();
                        }
                    }
                    catch
                    {
                        FileNotSavedInfoBar.Title = "The file is currently in use either by the autosave option or another app";
                        FileNotSavedInfoBar.IsOpen = true;
                        CheckForSaving();
                    }
                }
            }

            //----------Save Background Close----------

            if (Background == true && Close == true && Erase == false && NoFile == false)
            {
                if (TXTFile == null)
                {
                    FileSaveBox.Close();
                    await SaveFile(false, true, false, false);
                }
                if (TXTFile.OpenReadAsync() != null)
                {
                    var RAS = await TXTFile.OpenAsync(FileAccessMode.ReadWrite);
                    try
                    {
                        try
                        {
                            FileSaveBox.Close();
                            REB.Document.SaveToStream(TextGetOptions.FormatRtf, RAS);
                            var RBS = await TXTFile.OpenAsync(FileAccessMode.Read);
                            RTB.Document.LoadFromStream(TextSetOptions.FormatRtf, RBS);
                            var Stats = await CachedFileManager.CompleteUpdatesAsync(TXTFile);
                            if (Stats == FileUpdateStatus.Complete)
                            {
                                //Confirm file saving and close
                                IsCloseRequestComplete = true;
                                new ToastContentBuilder()
                                    .SetToastScenario(ToastScenario.Reminder)
                                    .AddText($"Your file has been succesfully saved at {TXTFile.Path}")
                                    .AddButton(new ToastButton().SetDismissActivation().SetContent("Close"))
                                    .Show();
                                CheckForSaving();
                            }
                            else if (Stats != FileUpdateStatus.Complete)
                            {
                                //File failed to save message
                                FileNotSavedInfoBar.Title = "File couldn't be saved";
                                FileNotSavedInfoBar.IsOpen = true;
                                CheckForSaving();
                            }
                            RBS.Dispose();
                        }
                        finally
                        {
                            RAS.Dispose();
                        }
                    }
                    catch
                    {
                        FileNotSavedInfoBar.Title = "The file is currently in use either by the autosave option or another app";
                        FileNotSavedInfoBar.IsOpen = true;
                        CheckForSaving();
                    }
                }
            }

            //----------Save Close----------

            if (Background == false && Close == true && Erase == false && NoFile == false)
            {
                FileSaveBox.Close();
                FSP.FileTypeChoices.Add("Rich Ivirius Text", new List<string>() { ".ivrtext" });
                FSP.FileTypeChoices.Add("Universal Rich Text", new List<string>() { ".rtf" });
                FSP.FileTypeChoices.Add("Rich Text", new List<string>() { ".richtxtformat" });
                FSP.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                FSP.SuggestedFileName = "New Rich Text File";
                //Set file content
                TXTFile = await FSP.PickSaveFileAsync();
                if (TXTFile != null)
                {
                    try
                    {
                        var RAS = await TXTFile.OpenAsync(FileAccessMode.ReadWrite);
                        try
                        {
                            FileSaveBox.Close();
                            REB.Document.SaveToStream(TextGetOptions.FormatRtf, RAS);
                            var RBS = await TXTFile.OpenAsync(FileAccessMode.Read);
                            RTB.Document.LoadFromStream(TextSetOptions.FormatRtf, RBS);
                            var Stats = await CachedFileManager.CompleteUpdatesAsync(TXTFile);
                            if (Stats == FileUpdateStatus.Complete)
                            {
                                //Confirm file saving and close
                                IsCloseRequestComplete = true;
                                new ToastContentBuilder()
                                .SetToastScenario(ToastScenario.Reminder)
                                .AddText($"Your file has been succesfully saved at {TXTFile.Path}")
                                .AddButton(new ToastButton()
                                    .SetDismissActivation().SetContent("Close"))
                                .Show();
                                CheckForSaving();
                            }
                            else if (Stats != FileUpdateStatus.Complete)
                            {
                                //File failed to save message
                                FileNotSavedInfoBar.Title = "File couldn't be saved";
                                FileNotSavedInfoBar.IsOpen = true;
                                CheckForSaving();
                            }
                            RBS.Dispose();
                        }
                        finally
                        {
                            RAS.Dispose();
                        }
                    }
                    catch
                    {
                        FileNotSavedInfoBar.Title = "The file is currently in use either by the autosave option or another app";
                        FileNotSavedInfoBar.IsOpen = true;
                        CheckForSaving();
                    }
                }
            }

            //----------Save Background Erase----------

            if (Background == true && Close == false && Erase == true && NoFile == false)
            {
                if (TXTFile == null)
                {
                    FileSaveBox.Close();
                    await SaveFile(false, false, true, false);
                    CheckForSaving();
                }
                if (TXTFile != null)
                {
                    FileSaveBox.Close();
                    try
                    {
                        var RAS = await TXTFile.OpenAsync(FileAccessMode.ReadWrite);
                        try
                        {
                            REB.Document.SaveToStream(TextGetOptions.FormatRtf, RAS);
                            var RBS = await TXTFile.OpenAsync(FileAccessMode.Read);
                            RTB.Document.LoadFromStream(TextSetOptions.FormatRtf, RBS);

                            RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                            var ValueRTB = RichEditBoxConverter.GetText(RTB);

                            REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                            REB.Document.SetText(TextSetOptions.FormatRtf, "");
                            var ValueREB = RichEditBoxConverter.GetText(REB);

                            RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                            RTB.Document.SetText(TextSetOptions.FormatRtf, "");

                            var Stats = await CachedFileManager.CompleteUpdatesAsync(TXTFile);
                            TXTFile = null;
                            if (Stats == FileUpdateStatus.Complete)
                            {
                                //Confirm file saving without close
                                new ToastContentBuilder()
                                .SetToastScenario(ToastScenario.Reminder)
                                .AddText($"Your file has been succesfully saved at {TXTFile.Path}")
                                .AddButton(new ToastButton()
                                    .SetDismissActivation().SetContent("Close"))
                                .Show();
                                CheckForSaving();
                            }
                            if (Stats != FileUpdateStatus.Complete)
                            {
                                //File failed to save message
                                FileNotSavedInfoBar.Title = "File couldn't be saved";
                                FileNotSavedInfoBar.IsOpen = true;
                                CheckForSaving();
                            }
                            RBS.Dispose();
                        }
                        finally
                        {
                            RAS.Dispose();
                        }
                    }
                    catch
                    {
                        FileNotSavedInfoBar.Title = "The file is currently in use either by the autosave option or another app";
                        FileNotSavedInfoBar.IsOpen = true;
                        CheckForSaving();
                    }
                }
            }

            //----------Save Erase----------

            if (Background == false && Close == false && Erase == true && NoFile == false)
            {
                FileSaveBox.Close();
                //File dialog configuration
                FSP.FileTypeChoices.Add("Rich Ivirius Text", new List<string>() { ".ivrtext" });
                FSP.FileTypeChoices.Add("Universal Rich Text", new List<string>() { ".rtf" });
                FSP.FileTypeChoices.Add("Rich Text", new List<string>() { ".richtxtformat" });
                FSP.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                FSP.SuggestedFileName = "New Rich Text File";
                //Set file content
                TXTFile = await FSP.PickSaveFileAsync();
                if (TXTFile != null)
                {
                    try
                    {
                        var RAS = await TXTFile.OpenAsync(FileAccessMode.ReadWrite);
                        try
                        {
                            FileSaveBox.Close();
                            REB.Document.SaveToStream(TextGetOptions.FormatRtf, RAS);
                            var RBS = await TXTFile.OpenAsync(FileAccessMode.Read);
                            RTB.Document.LoadFromStream(TextSetOptions.FormatRtf, RBS);

                            RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                            var ValueRTB = RichEditBoxConverter.GetText(RTB);

                            REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                            REB.Document.SetText(TextSetOptions.FormatRtf, "");
                            var ValueREB = RichEditBoxConverter.GetText(REB);

                            RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                            RTB.Document.SetText(TextSetOptions.FormatRtf, "");

                            var Stats = await CachedFileManager.CompleteUpdatesAsync(TXTFile);
                            TXTFile = null;
                            if (Stats == FileUpdateStatus.Complete)
                            {
                                //Confirm file saving without close
                                new ToastContentBuilder()
                                .SetToastScenario(ToastScenario.Reminder)
                                .AddText($"Your file has been succesfully saved at {TXTFile.Path}")
                                .AddButton(new ToastButton()
                                    .SetDismissActivation().SetContent("Close"))
                                .Show();
                                CheckForSaving();
                            }
                            if (Stats != FileUpdateStatus.Complete)
                            {
                                //File failed to save message
                                FileNotSavedInfoBar.Title = "File couldn't be saved";
                                FileNotSavedInfoBar.IsOpen = true;
                                CheckForSaving();
                            }
                            RBS.Dispose();
                        }
                        finally
                        {
                            RAS.Dispose();
                        }
                    }
                    catch
                    {
                        FileNotSavedInfoBar.Title = "The file is currently in use either by the autosave option or another app";
                        FileNotSavedInfoBar.IsOpen = true;
                        CheckForSaving();
                    }
                }
            }
        }

        public async Task Open()
        {
            //File dialog configuration
            var FOP = new FileOpenPicker();
            FOP.FileTypeFilter.Add(".ivrtext");
            FOP.FileTypeFilter.Add(".rtf");
            FOP.FileTypeFilter.Add(".richtxtformat");
            FOP.FileTypeFilter.Add(".ivrnote");
            FOP.FileTypeFilter.Add(".rwhi");
            FOP.SuggestedStartLocation = PickerLocationId.Desktop;

            //Set file content
            TXTFile = await FOP.PickSingleFileAsync();
            if (!(TXTFile == null))
            {
                try
                {
                    //Set RichEditBox content
                    var RAS = await TXTFile.OpenAsync(FileAccessMode.ReadWrite);
                    try
                    {
                        REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAS);
                        RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                        RTB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAS);
                    }
                    finally
                    {
                        RAS.Dispose();
                    }
                    HomePage.Visibility = Visibility.Collapsed;
                    CheckForSaving();
                    _ = REB.Focus(FocusState.Programmatic);
                }
                catch
                {
                    ActionErrorMessage.Text = "This file is currently in use or has been recently closed. Any changes you make in the current document must be saved separately. Try again later";
                    ActionErrorBox.Open();
                    CheckForSaving();
                    return;
                }
            }
        }

        #endregion Actions

        #region External

        public void RequestClose()
        {
            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
            if (Value == SecValue)
            {
                IsCloseRequestComplete = true;
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, true, false, false);
                    else await SaveFile(false, true, false, false);
                    HomePage.Visibility = Visibility.Collapsed;
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    IsCloseRequestComplete = true;
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
            return;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs EvArgs)
        {
            //Catch file
            base.OnNavigatedTo(EvArgs);
            var Args = EvArgs.Parameter as IActivatedEventArgs;
            var FArgs = Args as FileActivatedEventArgs;
            try
            {
                if (Args != null && Args.Kind == ActivationKind.File)
                {
                    //Write file properties
                    TXTFile = FArgs.Files[0] as StorageFile;
                    var Str = await TXTFile.OpenReadAsync();
                    REB.Document.LoadFromStream(TextSetOptions.FormatRtf, Str);
                    //Read file
                    Str.Dispose();
                    HomePage.Visibility = Visibility.Collapsed;
                    CheckForSaving();
                }
                else
                {
                    CheckForSaving();
                    return;
                }
            }
            catch
            {
                try
                {
                    FileNotSavedInfoBar.Title = "This file is currently in use or has been recently closed. Try again later";
                    FileNotSavedInfoBar.IsOpen = true;
                    CheckForSaving();
                    return;
                }
                catch { return; }
            }
        }

        #endregion External

        #endregion File

        #region Text Editing

        #region Clipboard

        private void MenuFlyoutItem_Click_4(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Paste(0);
        }

        private void MenuFlyoutItem_Click_5(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Paste(1);
        }

        private void CTB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Cut();
            REB.ContextFlyout.Hide();
        }

        private void CB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Copy();
            REB.ContextFlyout.Hide();
        }

        private void PB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Paste(0);
            REB.ContextFlyout.Hide();
        }

        private void SAB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            _ = REB.Focus(FocusState.Pointer);
            var Text = RichEditBoxConverter.GetText(REB);
            REB.Document.Selection.SetRange(0, Text.Length);
            REB.ContextFlyout.Hide();
        }

        private void MenuFlyoutItem_Click_9(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Cut();
            REB.ContextFlyout.Hide();
        }

        private void MenuFlyoutItem_Click_10(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Copy();
            REB.ContextFlyout.Hide();
        }

        private void MenuFlyoutItem_Click_11(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Paste(0);
            REB.ContextFlyout.Hide();
        }

        private void MenuFlyoutItem_Click_12(object Sender, RoutedEventArgs EvArgs)
        {
            _ = REB.Focus(FocusState.Pointer);
            var Text = RichEditBoxConverter.GetText(REB);
            REB.Document.Selection.SetRange(0, Text.Length);
            REB.ContextFlyout.Hide();
        }

        #endregion Clipboard

        #region Font & paragraph

        private void EraseFormatBTN_Click(object sender, RoutedEventArgs e)
        {
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";
                CF.Outline = FormatEffect.Off;
                CF.Size = (float)10.5;
                FSize.Value = 10.5;
                CF.Underline = UnderlineType.None;
                CF.Strikethrough = FormatEffect.Off;
                _ = ST.CharacterFormat.ForegroundColor;
                BackAccent.Foreground = new SolidColorBrush(Colors.Transparent);
                ST.CharacterFormat.BackgroundColor = Colors.Transparent;
                _ = ST.CharacterFormat.ForegroundColor;
                FontAccent.Foreground = new SolidColorBrush(Colors.Black);
                ST.CharacterFormat.ForegroundColor = Colors.Black;
                CF.Subscript = FormatEffect.Off;
                CF.Superscript = FormatEffect.Off;
            }
            CheckFormatting();
        }

        private void MenuFlyoutItem_Click_13(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure bold
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.CharacterFormat.Bold;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Bold = CF;
            }
            CheckFormatting();
        }

        private void MenuFlyoutItem_Click_14(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure italic
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.CharacterFormat.Italic;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Italic = CF;
            }
            CheckFormatting();
        }

        private void MenuFlyoutItem_Click_15(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure strikethrough
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.CharacterFormat.Strikethrough;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Strikethrough = CF;
            }
            CheckFormatting();
        }

        private void MenuFlyoutItem_Click_16(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure subscript
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.CharacterFormat.Subscript;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Subscript = CF;
            }
            CheckFormatting();
        }

        private void MenuFlyoutItem_Click_17(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure superscript
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.CharacterFormat.Superscript;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Superscript = CF;
            }
            CheckFormatting();
        }

        private void MenuFlyoutItem_Click_1(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure underline
            var MFItem = (MenuFlyoutItem)Sender;
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.ParagraphFormat.ListType;
                if (MFItem.Text == "None") CF = MarkerType.None;
                if (MFItem.Text == "Bullet") CF = MarkerType.Bullet;
                if (MFItem.Text == "Numbers") CF = MarkerType.CircledNumber;
                if (MFItem.Text == "Lowercase letters") CF = MarkerType.LowercaseEnglishLetter;
                if (MFItem.Text == "Uppercase letters") CF = MarkerType.UppercaseEnglishLetter;
                if (MFItem.Text == "Roman") CF = MarkerType.UppercaseRoman;
                ST.ParagraphFormat.ListType = CF;
                REB.ContextFlyout.Hide();
            }
        }

        private void BB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure bold
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat.Bold;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Bold = CF;
            }
            CheckFormatting();
        }

        private void IB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure italic
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat.Italic;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Italic = CF;
            }
            CheckFormatting();
        }

        private void STB_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure strikethrough
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat.Strikethrough;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Strikethrough = CF;
            }
            CheckFormatting();
        }

        private void MenuFlyoutItem_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure underline
            var MFItem = (MenuFlyoutItem)Sender;
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat.Underline;
                if (MFItem.Text == "None") CF = UnderlineType.None;
                if (MFItem.Text == "Single") CF = UnderlineType.Single;
                if (MFItem.Text == "Dash") CF = UnderlineType.Dash;
                if (MFItem.Text == "Dotted") CF = UnderlineType.Dotted;
                if (MFItem.Text == "Double") CF = UnderlineType.Double;
                if (MFItem.Text == "Thick") CF = UnderlineType.Thick;
                if (MFItem.Text == "Wave") CF = UnderlineType.Wave;
                ST.CharacterFormat.Underline = CF;
                REB.ContextFlyout.Hide();
            }
        }

        private void MenuFlyoutItem2_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure paragraph alignment
            var MFItem = (MenuFlyoutItem)Sender;
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.ParagraphFormat.Alignment;
                if (MFItem.Text == "Left") CF = ParagraphAlignment.Left;
                if (MFItem.Text == "Center") CF = ParagraphAlignment.Center;
                if (MFItem.Text == "Right") CF = ParagraphAlignment.Right;
                if (MFItem.Text == "Justify") CF = ParagraphAlignment.Justify;
                ST.ParagraphFormat.Alignment = CF;
                REB.ContextFlyout.Hide();
            }
        }

        private void ColorPicker_ColorChanged(object Sender, ColorChangedEventArgs EvArgs)
        {

        }

        private void BackPicker_ColorChanged(object Sender, ColorChangedEventArgs EvArgs)
        {
            //Configure font highlight
            if (!(REB == null))
            {
                var ST = REB.Document.Selection;
                if (!(ST == null))
                {
                    _ = ST.CharacterFormat;
                    var Br = new SolidColorBrush(BackPicker.Color);
                    var CF = BackPicker.Color;
                    if (BackAccent != null) BackAccent.Foreground = Br;
                    ST.CharacterFormat.BackgroundColor = CF;
                }
            }
        }

        private void FontBox_SelectionChanged(object Sender, SelectionChangedEventArgs EvArgs)
        {
            //Configure font family
            var ST = REB.Document.Selection;
            if (ST != null && (string)FontBox.SelectedItem != "" && FontBox.SelectedItem != null)
            {
                ST.CharacterFormat.Name = (string)FontBox.SelectedItem;
            }
            else return;
        }

        private void ForegroundButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure font color
            var BTN = Sender as Button;
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                _ = ST.CharacterFormat.ForegroundColor;
                var Br = BTN.Foreground;
                FontAccent.Foreground = Br;
                ST.CharacterFormat.ForegroundColor = (BTN.Foreground as SolidColorBrush).Color;
            }
        }

        private void NullForegroundButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure font color
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                _ = ST.CharacterFormat.ForegroundColor;
                FontAccent.Foreground = new SolidColorBrush(Colors.Black);
                ST.CharacterFormat.ForegroundColor = Colors.Black;
            }
        }

        private void HighlightButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure font color
            var BTN = Sender as Button;
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                _ = ST.CharacterFormat.ForegroundColor;
                var Br = BTN.Foreground;
                BackAccent.Foreground = Br;
                ST.CharacterFormat.BackgroundColor = (BTN.Foreground as SolidColorBrush).Color;
            }
        }

        private void NullHighlightButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure font color
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                _ = ST.CharacterFormat.ForegroundColor;
                BackAccent.Foreground = new SolidColorBrush(Colors.Transparent);
                ST.CharacterFormat.BackgroundColor = Colors.Transparent;
            }
        }

        private void SubscriptButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure subscript
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat.Subscript;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Subscript = CF;
            }
            CheckFormatting();
        }

        private void SuperscriptButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure superscript
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat.Superscript;
                switch (CF)
                {
                    case FormatEffect.Off:
                        CF = FormatEffect.On;
                        break;
                    default:
                        CF = FormatEffect.Off;
                        break;
                }
                ST.CharacterFormat.Superscript = CF;
            }
            CheckFormatting();
        }

        #endregion Font & paragraph

        #region Insert

        StorageFile IMGFile;

        private void Button_Click_11(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Text
                += DateTime.Now.Month.ToString()
                + "/" + DateTime.Now.Day.ToString()
                + "/" + DateTime.Now.Year.ToString()
                + " " + DateTime.Now.Hour.ToString()
                + ":" + DateTime.Now.Minute.ToString()
                + ":" + DateTime.Now.Second.ToString();
            DateTimeInsert.Flyout.Hide();
        }

        private void Button_Click_12(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Text
                += DateTime.Now.Day.ToString()
                + "." + DateTime.Now.Month.ToString()
                + "." + DateTime.Now.Year.ToString()
                + ", " + DateTime.Now.Hour.ToString()
                + ":" + DateTime.Now.Minute.ToString()
                + ":" + DateTime.Now.Second.ToString();
            DateTimeInsert.Flyout.Hide();
        }

        private void Button_Click_13(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Text
                += DateTime.Now.Day.ToString()
                + "." + DateTime.Now.Month.ToString()
                + ", " + DateTime.Now.Hour.ToString()
                + ":" + DateTime.Now.Minute.ToString()
                + ":" + DateTime.Now.Second.ToString()
                + ":" + DateTime.Now.Millisecond.ToString();
            DateTimeInsert.Flyout.Hide();
        }

        private void Button_Click_14(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Text
                += DateTime.Now.Month.ToString()
                + "/" + DateTime.Now.Day.ToString()
                + ", " + DateTime.Now.Hour.ToString()
                + ":" + DateTime.Now.Minute.ToString()
                + ":" + DateTime.Now.Second.ToString()
                + ":" + DateTime.Now.Millisecond.ToString();
            DateTimeInsert.Flyout.Hide();
        }

        private void Button_Click_15(object Sender, RoutedEventArgs EvArgs)
        {
            REB.Document.Selection.Text += DateTime.Now.ToString();
            DateTimeInsert.Flyout.Hide();
        }

        private void Button_Click_5(object Sender, RoutedEventArgs EvArgs)
        {
            if (string.IsNullOrEmpty(HLBox.Text)) return;
            if (string.IsNullOrEmpty(REB.Document.Selection.Text)) return;
            REB.Document.Selection.Link = "\"" + HLBox.Text + "\"";
            LinkInsert.Flyout.Hide();
        }

        private void Button_Click_6(object Sender, RoutedEventArgs EvArgs)
        {
            if (string.IsNullOrEmpty(REB.Document.Selection.Link)) return; REB.Document.Selection.Link = "";
        }

        private void Button_Click_7(object Sender, RoutedEventArgs EvArgs)
        {
            LinkInsert.Flyout.Hide();
        }

        private async void Image_Insert_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //File dialog configuration
            var FOP = new FileOpenPicker();
            FOP.FileTypeFilter.Add(".png");
            FOP.FileTypeFilter.Add(".jpg");
            FOP.FileTypeFilter.Add(".jpeg");
            FOP.FileTypeFilter.Add(".bmp");
            FOP.SuggestedStartLocation = PickerLocationId.Desktop;
            IMGFile = await FOP.PickSingleFileAsync();
            //Set file content
            if (!(IMGFile == null))
            {
                try
                {
                    //Insert image
                    var RAC = await IMGFile.OpenAsync(FileAccessMode.Read);
                    var BMP = new BitmapImage(new Uri(IMGFile.Path));
                    REB.Document.Selection.InsertImage(BMP.PixelWidth, BMP.PixelHeight, 0, VerticalCharacterAlignment.Baseline, IMGFile.DisplayName, RAC);
                }
                catch (Exception Ex)
                {
                    FatalExceptionMessage.Text = $"{Ex.HResult} ===== {Ex.Message}";
                    FatalExceptionBox.Open();
                }
            }
        }

        private void LinkInsert_Click(object Sender, RoutedEventArgs EvArgs) => LinkInsert.Flyout.ShowAt(LinkInsert);

        #endregion Insert

        #region Doc

        HandwritingView HWV;

        #region TextToSpeech

        MediaElement ME;

        SpeechSynthesizer Synth;

        private async void ReadButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configuring the sppech synthesizer
            ME = new MediaElement();
            Synth = new SpeechSynthesizer();
            var Str = await Synth.SynthesizeTextToStreamAsync(REB.Document.Selection.Text.ToString());
            ME.SetSource(Str, Str.ContentType);
            ME.Play();
            ME.MediaEnded += ME_MediaEnded;
            KeepReadButton.IsEnabled = true;
            PauseReadButton.IsEnabled = true;
            StopReadButton.IsEnabled = true;
        }

        private void ME_MediaEnded(object Sender, RoutedEventArgs EvArgs)
        {
            //Disabling all the action buttons
            KeepReadButton.IsEnabled = false;
            PauseReadButton.IsEnabled = false;
            StopReadButton.IsEnabled = false;
        }

        private void StopReadButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Stops the reading process
            ME.Stop();
            KeepReadButton.IsEnabled = false;
            PauseReadButton.IsEnabled = false;
            StopReadButton.IsEnabled = false;
        }

        private void PauseReadButton_Click(object Sender, RoutedEventArgs EvArgs) => ME.Pause();

        private void KeepReadButton_Click(object Sender, RoutedEventArgs EvArgs) => ME.Play();

        #endregion TextToSpeech

        private void REB_SelectionChanged(object Sender, RoutedEventArgs EvArgs)
        {
            //Variables
            var ST = REB.Document.Selection;
            var CF = ST.CharacterFormat;

            //Colors
            if (BackPicker != null) BackPicker.Color = CF.BackgroundColor;
            if (ColPicker != null) ColPicker.Color = CF.ForegroundColor;
            if (CF.BackgroundColor == Colors.White || CF.BackgroundColor == Colors.Transparent)
            {
                BackAccent.Foreground = new SolidColorBrush(Colors.Transparent);
            }

            //Autosave
            if (AutoSaveSwitch.IsOn == true)
            {
                TriggerAutosave();
            }

            //Font
            if (!(FSize == null))
            {
                if (ST.Length > 0 || ST.Length < 0) FSize.Value = double.NaN;
                else FSize.Value = CF.Size;
            }
            if (FontBox != null)
            {
                if (ST.Length > 0 || ST.Length < 0) FontBox.SelectedItem = null;
                else FontBox.SelectedItem = CF.Name.ToString();
            }
            CheckFormatting();

            //Paragraph
            if (ST.Length > 0 || ST.Length < 0)
            {
                LeftInd.Background = new SolidColorBrush(Colors.Gray);
                RightInd.Background = new SolidColorBrush(Colors.Gray);
                LeftInd.Foreground = new SolidColorBrush(Colors.Gray);
                RightInd.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                LeftInd.ClearValue(BackgroundProperty);
                RightInd.ClearValue(BackgroundProperty);
                LeftInd.ClearValue(ForegroundProperty);
                RightInd.ClearValue(ForegroundProperty);
                CheckIndent();
            }
            CheckAlignment();

            //Text-to-speech
            if (!(ReadButton == null))
            {
                if (REB.Document.Selection.Text == "") ReadButton.IsEnabled = false;
                else ReadButton.IsEnabled = true;
            }

            //Selected words
            if (ST.Length > 0 || ST.Length < 0)
            {
                SelWordGrid.Visibility = Visibility.Visible;
                REB.Document.Selection.GetText(TextGetOptions.None, out var seltext);
                var selwordcount = seltext.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
                SelWordCount.Text = $"Selected words: {selwordcount}";
            }
            else
            {
                SelWordGrid.Visibility = Visibility.Collapsed;
            }

            REB.Document.GetText(TextGetOptions.None, out var text);
            var wordcount = text.Split(new char[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            WordCount.Text = $"Word count: {wordcount}";

            //Disable & enable buttons
            ButtonCheck();
        }

        private void ButtonCheck()
        {
            if (REB.Document.CanCopy() == true)
            {
                CB.IsEnabled = true;
                CBBar.IsEnabled = true;
            }
            else
            {
                CB.IsEnabled = false;
                CBBar.IsEnabled = false;
            }

            if (REB.Document.CanPaste() == true)
            {
                MenuPB.IsEnabled = true;
                PB.IsEnabled = true;
                PBDDB.IsEnabled = true;
                PBBar.IsEnabled = true;
            }
            else
            {
                MenuPB.IsEnabled = false;
                PB.IsEnabled = false;
                PBDDB.IsEnabled = false;
                PBBar.IsEnabled = false;
            }

            if (REB.Document.CanUndo() == true) Undo.IsEnabled = true;
            else Undo.IsEnabled = false;

            if (REB.Document.CanRedo() == true) Redo.IsEnabled = true;
            else Redo.IsEnabled = false;
        }

        private void SizeBox_SelectionChanged(object Sender, SelectionChangedEventArgs EvArgs)
        {
            //Set A4 size
            if ((string)SizeBox.SelectedItem == "A4")
            {
                REB.Width = 744;
                REB.Height = 1052.4;
                PP.MediaSize = Windows.Graphics.Printing.PrintMediaSize.IsoA4;
            }

            //Set Letter size
            if ((string)SizeBox.SelectedItem == "Letter")
            {
                REB.Width = 765;
                REB.Height = 990;
                PP.MediaSize = Windows.Graphics.Printing.PrintMediaSize.NorthAmericaLetter;
            }

            //Set Tabloid size
            if ((string)SizeBox.SelectedItem == "Tabloid")
            {
                REB.Width = 990;
                REB.Height = 1530;
                PP.MediaSize = Windows.Graphics.Printing.PrintMediaSize.NorthAmericaTabloid;
            }
        }

        [Obsolete]
        private void ZoomSlider_ValueChanged(object Sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs EvArgs)
        {
            //Changes the zoom amount
            SCR.ZoomToFactor((float)ZoomSlider.Value);
            var Val = SCR.ZoomFactor.ToString(".00");
            ZoomText.Text = $"Zoom: {Val.Replace('.', '­')}%";
        }

        private void LeftIndent_ValueChanged(object Sender, EventArgs EvArgs)
        {

        }

        private void RightIndent_ValueChanged(object Sender, EventArgs EvArgs)
        {

        }

        private void LeftInd_ValueChanged(object Sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs EvArgs)
        {
            SetParagraphIndents((float)LeftInd.Value, (float)RightInd.Value);
        }

        private void RightInd_ValueChanged(object Sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs EvArgs)
        {
            SetParagraphIndents((float)LeftInd.Value, (float)RightInd.Value);
        }

        private void SetParagraphIndents(float leftIndent, float rightIndent, bool applyToSelectionOnly = true)
        {
            // Get the ITextDocument interface for the RichEditBox's document
            ITextDocument document = REB.Document;

            // Get the current selection's start and end positions
            int start = document.Selection.StartPosition;
            int end = document.Selection.EndPosition;

            // If applyToSelectionOnly is true, check if there's any selected text in the RichEditBox
            if (applyToSelectionOnly && start == end)
            {
                return;
            }

            // Get the ITextRange interface for the selection or the entire document
            ITextRange textRange;
            if (applyToSelectionOnly)
            {
                textRange = document.Selection;
            }
            else
            {
                textRange = document.GetRange(0, RichEditBoxConverter.GetText(REB).Length);
            }

            // Get the ITextParagraphFormat interface for the text range
            ITextParagraphFormat paragraphFormat = textRange.ParagraphFormat;

            // Set the left and right indents for the current selection's paragraph(s)
            paragraphFormat.SetIndents(0, leftIndent, rightIndent);

            // Apply the new paragraph format to the current selection or the entire document
            textRange.ParagraphFormat = paragraphFormat;
        }

        private void CheckIndent()
        {
            //LeftIndent.Value = REB.Document.Selection.ParagraphFormat.LeftIndent;
            LeftInd.Value = REB.Document.Selection.ParagraphFormat.LeftIndent;

            //RightIndent.Value = REB.Document.Selection.ParagraphFormat.RightIndent;
            RightInd.Value = REB.Document.Selection.ParagraphFormat.RightIndent;
        }

        private void ZoomIn_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Zooms in
            ZoomSlider.Value += 0.1;
        }

        private void ZoomOut_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Zooms out
            ZoomSlider.Value -= 0.1;
        }

        private void HandButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Configure the handwriting dialog
            REB.IsHandwritingViewEnabled = true;
            HWV = REB.HandwritingView;
            switch (HWV.IsOpen)
            {
                case false:
                    _ = HWV.TryOpen();
                    break;
                case true:
                    _ = HWV.TryClose();
                    break;
            }
        }

        #endregion Doc

        #region Templates

        private void Template1_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Normal
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = (float)10.5;
                FSize.Value = 10.5;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template2_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Title
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                var PF = ST.ParagraphFormat;
                PF.Alignment = ParagraphAlignment.Center;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 28;
                FSize.Value = 28;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template3_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Title 2
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                var PF = ST.ParagraphFormat;
                PF.Alignment = ParagraphAlignment.Center;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 22;
                FSize.Value = 22;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template4_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Important
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.On;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.On;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 16;
                FSize.Value = 16;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template5_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Header
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 14;
                FSize.Value = 14;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template6_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Medium
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 18;
                FSize.Value = 18;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template7_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Subtitle
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 20;
                FSize.Value = 20;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template8_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Strong
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.On;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 18;
                FSize.Value = 18;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template9_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Content
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 16;
                FSize.Value = 16;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template10_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Finished
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.On;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 14;
                FSize.Value = 14;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template11_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Unfinished
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.On;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.Off;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 14;
                FSize.Value = 14;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        private void Template12_Click(object Sender, RoutedEventArgs EvArgs)
        {
            //Strong header
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                var CF = ST.CharacterFormat;
                CF.Bold = FormatEffect.Off;
                CF.FontStretch = FontStretch.Undefined;
                CF.Italic = FormatEffect.On;
                CF.Name = "Segoe UI";
                FontBox.SelectedItem = "Segoe UI";

                CF.Outline = FormatEffect.Off;
                CF.Size = 18;
                CF.ForegroundColor = Colors.DimGray;
                FSize.Value = 18;
                CF.Underline = UnderlineType.None;
                ST.CharacterFormat = CF;
                TempFlyout.Hide();
            }
        }

        #endregion Templates

        #region Find and Replace

        private void RepAllBTN_Click(object Sender, RoutedEventArgs EvArgs)
        {
            FindFlyout.Hide();
            if (ReplaceBox.Text == FindTextBox.Text)
            {
                ActionErrorBox.Open();
                ActionErrorMessage.Text = "Usage of identical characters for Find and Replace is not allowed";
            }
            else if (ReplaceBox.Text.ToLower() == FindTextBox.Text.ToLower() && CaseSensBox.IsChecked == true && FullWordsBox.IsChecked == true)
            {
                ActionErrorBox.Open();
                ActionErrorMessage.Text = "Usage of too similar characters for Find and Replace. Please uncheck the \"Match words\" box to proceed";
            }
            else if (ReplaceBox.Text.ToLower() == FindTextBox.Text.ToLower() && CaseSensBox.IsChecked != true)
            {
                ActionErrorBox.Open();
                ActionErrorMessage.Text = "Usage of too similar characters for Find and Replace. Please check the \"Case Sensitive\" box to proceed";
            }
            else
            {
                Replace(REB, FindTextBox.Text, ReplaceBox.Text, true);
            }
        }

        public void Replace(RichEditBox Sender, string TextToFind, string TextToReplace, bool ReplaceAll)
        {
            FindFlyout.Hide();
            if (ReplaceAll == true)
            {
                var Value = RichEditBoxConverter.GetText(Sender);
                if (!(string.IsNullOrWhiteSpace(Value) && string.IsNullOrWhiteSpace(TextToFind) && string.IsNullOrWhiteSpace(TextToReplace)))
                {
                    Sender.Document.Selection.SetRange(0, RichEditBoxConverter.GetText(Sender).Length);
                    if (CaseSensBox.IsChecked == true)
                    {
                        _ = Sender.Document.Selection.FindText(TextToFind, RichEditBoxConverter.GetText(Sender).Length, FindOptions.Case);
                        if (Sender.Document.Selection.Length == 1)
                        {
                            Sender.Document.Selection.SetText(TextSetOptions.FormatRtf, ReplaceBox.Text);
                            _ = Sender.Focus(FocusState.Pointer);
                            Replace(Sender, TextToFind, TextToReplace, true);
                        }
                    }
                    if (FullWordsBox.IsChecked == true)
                    {
                        _ = Sender.Document.Selection.FindText(TextToFind, RichEditBoxConverter.GetText(Sender).Length, FindOptions.Word);
                        if (Sender.Document.Selection.Length == 1)
                        {
                            Sender.Document.Selection.SetText(TextSetOptions.FormatRtf, ReplaceBox.Text);
                            _ = Sender.Focus(FocusState.Pointer);
                            Replace(Sender, TextToFind, TextToReplace, true);
                        }
                    }
                    if (!CaseSensBox.IsChecked == true && !FullWordsBox.IsChecked == true)
                    {
                        _ = Sender.Document.Selection.FindText(TextToFind, RichEditBoxConverter.GetText(Sender).Length, FindOptions.None);
                        if (Sender.Document.Selection.Length == 1)
                        {
                            Sender.Document.Selection.SetText(TextSetOptions.FormatRtf, ReplaceBox.Text);
                            _ = Sender.Focus(FocusState.Pointer);
                            Replace(Sender, TextToFind, TextToReplace, true);
                        }
                    }
                    _ = Sender.Focus(FocusState.Pointer);
                }
            }
            else
            {
                Sender.Document.Selection.SetRange(0, RichEditBoxConverter.GetText(Sender).Length);
                if (CaseSensBox.IsChecked == true)
                {
                    _ = Sender.Document.Selection.FindText(TextToFind, RichEditBoxConverter.GetText(Sender).Length, FindOptions.Case);
                    if (Sender.Document.Selection.Length == 1)
                    {
                        Sender.Document.Selection.SetText(TextSetOptions.FormatRtf, ReplaceBox.Text);
                        _ = Sender.Focus(FocusState.Pointer);
                    }
                }
                if (FullWordsBox.IsChecked == true)
                {
                    _ = Sender.Document.Selection.FindText(TextToFind, RichEditBoxConverter.GetText(Sender).Length, FindOptions.Word);
                    if (Sender.Document.Selection.Length == 1)
                    {
                        Sender.Document.Selection.SetText(TextSetOptions.FormatRtf, ReplaceBox.Text);
                        _ = Sender.Focus(FocusState.Pointer);
                    }
                }
                if (!CaseSensBox.IsChecked == true && !FullWordsBox.IsChecked == true)
                {
                    _ = Sender.Document.Selection.FindText(TextToFind, RichEditBoxConverter.GetText(Sender).Length, FindOptions.None);
                    if (Sender.Document.Selection.Length == 1)
                    {
                        Sender.Document.Selection.SetText(TextSetOptions.FormatRtf, ReplaceBox.Text);
                        _ = Sender.Focus(FocusState.Pointer);
                    }
                }
                _ = Sender.Focus(FocusState.Pointer);
            }
        }

        private void FindBTN_Click(object Sender, RoutedEventArgs EvArgs)
        {
            FindFlyout.Hide();
            REB.Document.Selection.SetRange(0, RichEditBoxConverter.GetText(REB).Length);
            if (CaseSensBox.IsChecked == true)
            {
                _ = REB.Document.Selection.FindText(FindTextBox.Text, RichEditBoxConverter.GetText(REB).Length, FindOptions.Case);
                _ = REB.Focus(FocusState.Pointer);
            }
            if (FullWordsBox.IsChecked == true)
            {
                _ = REB.Document.Selection.FindText(FindTextBox.Text, RichEditBoxConverter.GetText(REB).Length, FindOptions.Word);
                _ = REB.Focus(FocusState.Pointer);
            }
            if (!CaseSensBox.IsChecked == true && !FullWordsBox.IsChecked == true)
            {
                _ = REB.Document.Selection.FindText(FindTextBox.Text, RichEditBoxConverter.GetText(REB).Length, FindOptions.None);
                _ = REB.Focus(FocusState.Pointer);
            }
        }

        private void RepBTN_Click(object Sender, RoutedEventArgs EvArgs)
        {
            FindFlyout.Hide();
            Replace(REB, FindTextBox.Text, ReplaceBox.Text, false);
        }

        private void CancelFindRepBTN_Click(object Sender, RoutedEventArgs EvArgs)
        {
            _ = REB.Focus(FocusState.Pointer);
            FindFlyout.Hide();
        }

        #endregion Find and Replace

        #endregion Text Editing

        #region Settings

        string RestartArgs;

        private void ThemeBox_SelectionChanged(object Sender, SelectionChangedEventArgs EvArgs)
        {

        }

        private async void SSButton_Click_1(object Sender, RoutedEventArgs EvArgs)
        {
            RestartArgs = "e";
            var LS = ApplicationData.Current.LocalSettings;
            LS.Values["SetupFinish"] = "No";
            LS.Values["Theme"] = null;
            LS.Values["AccentBorder"] = "Off";
            LS.Values["AccentBackground"] = "Off";
            LS.Values["Changelog"] = "On";
            LS.Values["HomePage"] = "Off";
            LS.Values["Ruler"] = "On";
            LS.Values["StatusBar"] = "On";
            LS.Values["EXP"] = "Off";
            LS.Values["News"] = "On";
            LS.Values["Tips"] = "Off";
            LS.Values["Password"] = "args:passwordNullOrEmpty";
            LS.Values["Remember_me"] = "false";
            LS.Values["Username"] = "";
            _ = await CoreApplication.RequestRestartAsync(RestartArgs);
        }

        private async void SettingsSaveButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            var LS = ApplicationData.Current.LocalSettings;
            if (ThemeBox != null)
            {
                _ = ApplicationView.GetForCurrentView().TitleBar;
                if ((string)ThemeBox.SelectedItem == "Light")
                {
                    LS.Values["Theme"] = "Light";
                }
                if ((string)ThemeBox.SelectedItem == "Dark")
                {
                    LS.Values["Theme"] = "Dark";
                }
                if ((string)ThemeBox.SelectedItem == "Acrylic")
                {
                    LS.Values["Theme"] = "Nostalgic Windows";
                }
                if ((string)ThemeBox.SelectedItem == "Acrylic Glass")
                {
                    LS.Values["Theme"] = "Acrylic";
                }
                if ((string)ThemeBox.SelectedItem == "Old")
                {
                    LS.Values["Theme"] = "Old";
                }
                if ((string)ThemeBox.SelectedItem == "Full Dark")
                {
                    LS.Values["Theme"] = "Full Dark";
                }
                if ((string)ThemeBox.SelectedItem == "Slate Green")
                {
                    LS.Values["Theme"] = "Slate Green";
                }
                if ((string)ThemeBox.SelectedItem == "Transparent")
                {
                    LS.Values["Theme"] = "Transparent";
                }
            }
            if (AccentBorderToggle != null)
            {
                if (AccentBorderToggle.IsOn == true)
                {
                    LS.Values["AccentBorder"] = "On";
                }
                else
                {
                    LS.Values["AccentBorder"] = "Off";
                }
            }
            if (ChangelogToggle != null)
            {
                if (ChangelogToggle.IsOn == true)
                {
                    LS.Values["Changelog"] = "On";
                }
                else
                {
                    LS.Values["Changelog"] = "Off";
                }
            }
            if (StatusBarToggle != null)
            {
                if (StatusBarToggle.IsOn == true)
                {
                    LS.Values["StatusBar"] = "On";
                }
                else
                {
                    LS.Values["StatusBar"] = "Off";
                }
            }
            else
            {
                LS.Values["StatusBar"] = "On";
            }
            if (CutToggle != null)
            {
                if (CutToggle.IsOn == true)
                {
                    LS.Values["Cut"] = "On";
                }
                else
                {
                    LS.Values["Cut"] = "Off";
                }
            }
            else
            {
                LS.Values["Cut"] = "On";
            }
            if (CopyToggle != null)
            {
                if (CopyToggle.IsOn == true)
                {
                    LS.Values["Copy"] = "On";
                }
                else
                {
                    LS.Values["Copy"] = "Off";
                }
            }
            else
            {
                LS.Values["Copy"] = "On";
            }
            if (PasteToggle != null)
            {
                if (PasteToggle.IsOn == true)
                {
                    LS.Values["Paste"] = "On";
                }
                else
                {
                    LS.Values["Paste"] = "Off";
                }
            }
            else
            {
                LS.Values["Paste"] = "On";
            }
            if (DeleteToggle != null)
            {
                if (DeleteToggle.IsOn == true)
                {
                    LS.Values["Delete"] = "On";
                }
                else
                {
                    LS.Values["Delete"] = "Off";
                }
            }
            else
            {
                LS.Values["Delete"] = "Off";
            }
            if (NewToggle != null)
            {
                if (NewToggle.IsOn == true)
                {
                    LS.Values["New"] = "On";
                }
                else
                {
                    LS.Values["New"] = "Off";
                }
            }
            else
            {
                LS.Values["New"] = "Off";
            }
            if (OpenToggle != null)
            {
                if (OpenToggle.IsOn == true)
                {
                    LS.Values["Open"] = "On";
                }
                else
                {
                    LS.Values["Open"] = "Off";
                }
            }
            else
            {
                LS.Values["Open"] = "Off";
            }
            if (PrintToggle != null)
            {
                if (PrintToggle.IsOn == true)
                {
                    LS.Values["Print"] = "On";
                }
                else
                {
                    LS.Values["Print"] = "Off";
                }
            }
            else
            {
                LS.Values["Print"] = "Off";
            }
            if (HomeToggle != null)
            {
                if (HomeToggle.IsOn == true)
                {
                    LS.Values["HomePage"] = "On";
                }
                if (HomeToggle.IsOn == false)
                {
                    LS.Values["HomePage"] = "Off";
                }
            }
            else
            {
                LS.Values["HomePage"] = "Off";
            }
            if (RulerToggle != null)
            {
                if (RulerToggle.IsOn == true)
                {
                    LS.Values["Ruler"] = "On";
                }
                if (RulerToggle.IsOn == false)
                {
                    LS.Values["Ruler"] = "Off";
                }
            }
            else
            {
                LS.Values["Ruler"] = "On";
            }
            if (ToolbarBackgroundToggle != null)
            {
                if (ToolbarBackgroundToggle.IsOn == true)
                {
                    LS.Values["Toolbar"] = "On";
                }
                if (ToolbarBackgroundToggle.IsOn == false)
                {
                    LS.Values["Toolbar"] = "Off";
                }
            }
            else
            {
                LS.Values["Toolbar"] = "On";
            }
            if (AccentBackgroundToggle != null)
            {
                if (AccentBackgroundToggle.IsOn == true)
                {
                    LS.Values["AccentBackground"] = "On";
                }
                if (AccentBackgroundToggle.IsOn == false)
                {
                    LS.Values["AccentBackground"] = "Off";
                }
            }
            else
            {
                LS.Values["AccentBackground"] = "Off";
            }

            RestartArgs = "e";
            _ = await CoreApplication.RequestRestartAsync(RestartArgs);
        }

        private async void MenuFlyoutItem_Click_19(object Sender, RoutedEventArgs EvArgs)
        {
            RestartArgs = "e";
            var LS = ApplicationData.Current.LocalSettings;
            LS.Values["SetupFinish"] = "No";
            LS.Values["Theme"] = null;
            LS.Values["AccentBorder"] = "Off";
            LS.Values["AccentBackground"] = "Off";
            LS.Values["Changelog"] = "On";
            LS.Values["HomePage"] = "Off";
            LS.Values["Ruler"] = "On";
            LS.Values["StatusBar"] = "On";
            LS.Values["EXP"] = "Off";
            LS.Values["News"] = "On";
            LS.Values["Tips"] = "Off";
            LS.Values["Password"] = "args:passwordNullOrEmpty";
            LS.Values["Remember_me"] = "false";
            LS.Values["Username"] = "";
            _ = await CoreApplication.RequestRestartAsync(RestartArgs);
        }

        #endregion Settings

        #region Developer

        public void OpenConsole()
        {
            try
            {
                ConsoleMSGBox.Open();
            }
            catch
            {

            }
        }

        private void ConsoleBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) TriggerConsoleEvent();
        }

        public async void TriggerConsoleEvent()
        {
            if (ConsoleBox.Text == "new")
            {
                var Value = RichEditBoxConverter.GetText(REB);
                var SecValue = RichEditBoxConverter.GetText(RTB);
                var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
                if (Value == SecValue)
                {
                    RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                    var ValueRTB = RichEditBoxConverter.GetText(RTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, "");
                    var ValueREB = RichEditBoxConverter.GetText(REB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                    HomePage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //Remember the user to save the file
                    FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                    try { FileSaveBox.Open(); } catch { }
                    async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                    {
                        if (TXTFile != null) await SaveFile(true, false, true, false);
                        else await SaveFile(false, false, true, false);
                        HomePage.Visibility = Visibility.Collapsed;
                        FileSaveBox.Close();
                        FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                        FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                        FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                    }
                    void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                    {
                        RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                        var ValueRTB = RichEditBoxConverter.GetText(RTB);
                        REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                        REB.Document.SetText(TextSetOptions.FormatRtf, "");
                        var ValueREB = RichEditBoxConverter.GetText(REB);
                        RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                        RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                        HomePage.Visibility = Visibility.Collapsed;
                        FileSaveBox.Close();
                        FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                        FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                        FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                    }
                    void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                    {
                        FileSaveBox.Close();
                        FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                        FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                        FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                    }
                }
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "shutdown")
            {
                _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "shutdown /r")
            {
                RestartArgs = "e";
                _ = await CoreApplication.RequestRestartAsync(RestartArgs);
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "shutdown /clear")
            {
                RestartArgs = "e";
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["SetupFinish"] = "No";
                LS.Values["Theme"] = null;
                LS.Values["AccentBorder"] = "Off";
                LS.Values["AccentBackground"] = "Off";
                LS.Values["Changelog"] = "On";
                LS.Values["HomePage"] = "Off";
                LS.Values["Ruler"] = "On";
                LS.Values["StatusBar"] = "On";
                LS.Values["EXP"] = "Off";
                LS.Values["News"] = "On";
                LS.Values["Tips"] = "Off";
                LS.Values["Password"] = "args:passwordNullOrEmpty";
                LS.Values["Remember_me"] = "false";
                LS.Values["Username"] = "";
                _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "shutdown /r /clear")
            {
                RestartArgs = "e";
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["SetupFinish"] = "No";
                LS.Values["Theme"] = null;
                LS.Values["AccentBorder"] = "Off";
                LS.Values["AccentBackground"] = "Off";
                LS.Values["Changelog"] = "On";
                LS.Values["HomePage"] = "Off";
                LS.Values["Ruler"] = "On";
                LS.Values["StatusBar"] = "On";
                LS.Values["EXP"] = "Off";
                LS.Values["News"] = "On";
                LS.Values["Tips"] = "Off";
                LS.Values["Password"] = "args:passwordNullOrEmpty";
                LS.Values["Remember_me"] = "false";
                LS.Values["Username"] = "";
                _ = await CoreApplication.RequestRestartAsync(RestartArgs);
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "settings")
            {
                SettingsBox.Open();
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "settings /r")
            {
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["SetupFinish"] = "No";
                LS.Values["Theme"] = null;
                LS.Values["AccentBorder"] = "Off";
                LS.Values["AccentBackground"] = "Off";
                LS.Values["Changelog"] = "On";
                LS.Values["HomePage"] = "Off";
                LS.Values["Ruler"] = "On";
                LS.Values["StatusBar"] = "On";
                LS.Values["EXP"] = "Off";
                LS.Values["News"] = "On";
                LS.Values["Tips"] = "Off";
                LS.Values["Password"] = "args:passwordNullOrEmpty";
                LS.Values["Remember_me"] = "false";
                LS.Values["Username"] = "";
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "settings /get")
            {
                var LS = ApplicationData.Current.LocalSettings;
                OutputBox.Text
                    += $"AccentBorder :: '{LS.Values["AccentBorder"]}' \n"
                    + $"AccentBackground :: '{LS.Values["AccentBackground"]}' \n"
                    + $"SetupFinish :: '{LS.Values["SetupFinish"]}' \n"
                    + $"Theme :: '{LS.Values["Theme"]}' \n"
                    + $"Changelog :: '{LS.Values["Changelog"]}' \n"
                    + $"HomePage :: '{LS.Values["HomePage"]}' \n"
                    + $"Ruler :: '{LS.Values["Ruler"]}' \n"
                    + $"StatusBar :: '{LS.Values["StatusBar"]}' \n"
                    + $"EXP :: '{LS.Values["EXP"]}' \n"
                    + $"News :: '{LS.Values["News"]}' \n"
                    + $"Tips :: '{LS.Values["Tips"]}' \n"
                    + $"Password :: '{LS.Values["Password"]}' \n"
                    + $"Remember_me :: '{LS.Values["Remember_me"]}' \n"
                    + $"Username :: '{LS.Values["Username"]}' \n"
                    + "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "save")
            {
                if (TXTFile != null) await SaveFile(true, false, false, false);
                else await SaveFile(false, false, false, false);
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "save /all")
            {
                await SaveFile(false, false, false, false);
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "open")
            {
                await Open();
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "open /h")
            {
                _ = await Launcher.LaunchUriAsync(new Uri("ivirius-ivrhub://"));
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "homepage")
            {
                HomePage.Visibility = Visibility.Visible;
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "print")
            {
                //Configure printing dialog
                SCR.Content = null;
                var PH = new PrintHelper(Container);
                PH.AddFrameworkElementToPrint(REB);
                PH.OnPrintFailed += PrintHelper_OnPrintFailed;
                PH.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;
                PH.OnPrintCanceled += PH_OnPrintCanceled;
                await PH.ShowPrintUIAsync("New Rich Text File", PP);
                void PrintHelper_OnPrintSucceeded()
                {
                    //Try to print document
                    PH.Dispose();
                    ActionCompleteMessage.Text = "Printing succeded";
                    ActionCompleteBox.Open();
                    SCR.Content = REB;
                }
                void PrintHelper_OnPrintFailed()
                {
                    //Printing failed event
                    PH.Dispose();
                    ActionErrorMessage.Text = "Printing failed";
                    ActionErrorBox.Open();
                    SCR.Content = REB;
                }
                void PH_OnPrintCanceled()
                {
                    //Cancel printing
                    SCR.Content = REB;
                }
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "help /w")
            {
                _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page"));
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "setup /r")
            {
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["SetupFinish"] = "No";
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "setup /f")
            {
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["SetupFinish"] = "Yes";
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "file /clear")
            {
                TXTFile = null;
                CheckForSaving();
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "file /del /clear")
            {
                if (TXTFile != null)
                {
                    _ = TXTFile.DeleteAsync();
                    TXTFile = null;
                    CheckForSaving();
                    OutputBox.Text += "> Command executed succesfully \n";
                }
                else
                {
                    OutputBox.Text += "> Command executed succesfully, but failed operation \n";
                }
            }
            if (ConsoleBox.Text == "logout")
            {
                RestartArgs = "";
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["Remember_me"] = "false";
                _ = await CoreApplication.RequestRestartAsync(RestartArgs);
            }
            if (ConsoleBox.Text == "loginui /kill /in")
            {
                RestartArgs = "";
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["Remember_me"] = "true";
                _ = await CoreApplication.RequestRestartAsync(RestartArgs);
            }
            if (ConsoleBox.Text == "loginui /kill /off")
            {
                RestartArgs = "";
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["Remember_me"] = "false";
                _ = await CoreApplication.RequestRestartAsync(RestartArgs);
            }
            if (ConsoleBox.Text == "console /boot /t")
            {
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["ConsoleBoot"] = "On";
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "console /boot /f")
            {
                var LS = ApplicationData.Current.LocalSettings;
                LS.Values["ConsoleBoot"] = "Off";
                OutputBox.Text += "> Command executed succesfully \n";
            }
            if (ConsoleBox.Text == "output /clear")
            {
                OutputBox.Text = "> Output \n";
            }

            //Last command
            if (ConsoleBox.Text == "help")
            {
                OutputBox.Text += "List of commands: \n"
                    + "> new - new document \n"
                    + "> shutdown - close app \n"
                    + "> shutdown /r - restart app \n"
                    + "> shutdown /clear - close app and reset settings \n"
                    + "> shutdown /r /clear - restart app and reset settings \n"
                    + "> settings  - open settings \n"
                    + "> settings /r - reset all settings \n"
                    + "> settings /get - gives the values of all settigs \n"
                    + "> save - save \n"
                    + "> save /all - save as \n"
                    + "> open - open file \n"
                    + "> open /h - open hub \n"
                    + "> homepage - open home page \n"
                    + "> print - print \n"
                    + "> help - get list of all commands \n"
                    + "> help /w - get help on the web \n"
                    + "> setup /r - reset setup \n"
                    + "> setup /f - finish setup \n"
                    + "> file /clear - clear file info \n"
                    + "> file /del /clear - delete file and clear info \n"
                    + "> logout - logs out the user \n"
                    + "> loginui /kill /in - kills app and logs in the user \n"
                    + "> loginui /kill /off - kills app and logs out the user \n"
                    + "> console /boot /t - always show console on boot \n"
                    + "> console /boot /f - never show console on boot \n"
                    + "> output /clear - clear the output \n";
            }
            ConsoleBox.Text = "";
        }

        private void ConsoleBootDT_Tick(object Sender, object Args)
        {
            OpenConsole();
            DT.Stop();
        }

        #endregion Developer

        #region Menu Actions

        private async void MenuFlyoutItem_Click_25(object sender, RoutedEventArgs e)
        {
            RestartArgs = "ivirius_text_editor.UserRequestedRestart";
            _ = await CoreApplication.RequestRestartAsync(RestartArgs);
        }

        private async void MenuFlyoutItem_Click_22(object sender, RoutedEventArgs e)
        {
            var X = (Window.Current.Content as Frame).Content as MainPage;
            await X.AddExternalTabAsync();
        }

        private async void MenuFlyoutItem_Click_20(object Sender, RoutedEventArgs EvArgs)
        {
            var LS = ApplicationData.Current.LocalSettings;
            if (LS.Values["Password"] == null || (string)LS.Values["Password"] == "" || (string)LS.Values["Password"] == "args:passwordNullOrEmpty" || LS != null)
            {
                ActionErrorMessage.Text = "The current user doesn't have a password and can't be logged out";
                ActionErrorBox.Open();
            }
            if (LS.Values["Password"] != null || (string)LS.Values["Password"] != "" || LS != null)
            {
                RestartArgs = "";
                LS.Values["Remember_me"] = "false";
                _ = await CoreApplication.RequestRestartAsync(RestartArgs);
            }
            else
            {
                ActionErrorMessage.Text = "The current user doesn't have a password and can't be logged out";
                ActionErrorBox.Open();
            }
        }

        private void MenuFlyoutItem_Click_18(object Sender, RoutedEventArgs EvArgs)
        {
            ConsoleMSGBox.Open();
        }

        private async void MenuFlyoutItem_Click_7(object Sender, RoutedEventArgs EvArgs)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/news/"));
        }

        private async void MenuFlyoutItem_Click_8(object Sender, RoutedEventArgs EvArgs)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://www.youtube.com/channel/UC-wq6vlXEW3FBj2jMNVMOkg"));
        }

        private void MenuFlyoutItem_Click_3(object Sender, RoutedEventArgs EvArgs)
        {

        }

        [Obsolete]
        private void MenuFlyoutItem_Click_2(object Sender, RoutedEventArgs EvArgs) => HomePage.Visibility = Visibility.Visible;

        #endregion Menu Actions

        #region Other Actions

        private void TextCmdBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 1400)
            {
                TempViewer.Visibility = Visibility.Collapsed;
                TempDDB.Visibility = Visibility.Visible;
            }
            if (e.NewSize.Width >= 1400)
            {
                TempViewer.Visibility = Visibility.Visible;
                TempDDB.Visibility = Visibility.Collapsed;
            }
        }

        private async void Button_Click_17(object sender, RoutedEventArgs e)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/ivirius-text-editor2/"));
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            WelcomeZippyFlyout.IsOpen = true;
        }

        private void Undo_Click(object Sender, RoutedEventArgs EvArgs) => REB.Document.Undo();

        private void Redo_Click(object Sender, RoutedEventArgs EvArgs) => REB.Document.Redo();

        private void SettingsButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            Settings.Visibility = Visibility.Visible;
        }

        private async void ChangelogButton_Click(object Sender, RoutedEventArgs EvArgs)
        {
            ChangelogBox.Open();
        }

        private async void Button_Click(object Sender, RoutedEventArgs EvArgs) => await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page"));

        private async void Button_Click_1(object sender, RoutedEventArgs EvArgs) => await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/contact/"));

        private async void Button_Click_2(object Sender, RoutedEventArgs EvArgs) => await Launcher.LaunchUriAsync(new Uri("https://www.termsfeed.com/live/b7d195d5-dcc3-4a7e-9340-3f1ba360a662"));

        private async void Button_Click_3(object Sender, RoutedEventArgs EvArgs) => await Launcher.LaunchUriAsync(new Uri("ivirius-ivrhub://"));

        private async void Button_Click_16(object Sender, RoutedEventArgs EvArgs)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/ivirius-text-editor-2-0-is-out/"));
        }

        private async void SNFYesToFull_Click(object Sender, RoutedEventArgs EvArgs)
        {
            await SaveFile(false, false, false, false);
        }

        private async void Button_Click_9(object Sender, RoutedEventArgs EvArgs)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/new-setup-for-ivirius-text-editor/"));
        }

        private async void Button_Click_10(object Sender, RoutedEventArgs EvArgs)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/tabs-are-coming-for-ivirius-text-editor/"));
        }

        private void REB_TextChanged(object Sender, RoutedEventArgs EvArgs)
        {
            CheckForSaving();
            CheckIndent();
        }

        private async void SNFYes_Click(object Sender, RoutedEventArgs EvArgs)
        {
            await SaveFile(false, false, false, false);
        }

        private async void SNFYes_Click_Close(object Sender, RoutedEventArgs EvArgs)
        {
            await SaveFile(false, true, false, false);
        }

        private async void SNFYes_Click_Erase(object Sender, RoutedEventArgs EvArgs)
        {
            await SaveFile(false, false, true, false);
        }

        private void SNFCancel_Click(object Sender, RoutedEventArgs EvArgs)
        {

        }

        private void AboutItem_Click(object Sender, RoutedEventArgs EvArgs)
        {
            AboutBox.Open();
        }

        private void Button_Click_8(object Sender, RoutedEventArgs EvArgs)
        {

        }

        private void Page_KeyDown(object Sender, KeyRoutedEventArgs EvArgs)
        {
            //Scroll viewer movement
            if (EvArgs.Key == VirtualKey.Shift)
            {
                SCR.IsScrollInertiaEnabled = false;
                SCR.VerticalScrollMode = ScrollMode.Disabled;
                TextCmdBar.IsScrollInertiaEnabled = false;
                TextCmdBar.VerticalScrollMode = ScrollMode.Disabled;
            }
        }

        private void Page_KeyUp(object Sender, KeyRoutedEventArgs EvArgs)
        {
            //Scroll viewer movement
            if (EvArgs.Key == VirtualKey.Shift)
            {
                SCR.IsScrollInertiaEnabled = true;
                SCR.VerticalScrollMode = ScrollMode.Enabled;
                TextCmdBar.IsScrollInertiaEnabled = true;
                TextCmdBar.VerticalScrollMode = ScrollMode.Enabled;
            }
        }

        #endregion Other Actions

        #region Home Page

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            TemplatesBox.Open();
        }

        private void HideHome_Click(object Sender, RoutedEventArgs EvArgs) => HomePage.Visibility = Visibility.Collapsed;

        #region Templates

        private async void Button_Click_21(object sender, RoutedEventArgs e)
        {
            Zippy.Source = new Uri("ms-appx:///ZippyFolder.mp4");

            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
            if (Value == SecValue)
            {
                RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                var ValueRTB = RichEditBoxConverter.GetText(RTB);

                REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                REB.Document.SetText(TextSetOptions.FormatRtf, "");
                var ValueREB = RichEditBoxConverter.GetText(REB);

                RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                HomePage.Visibility = Visibility.Collapsed;
                TXTFile = null;
                var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///EssayTemp.rtf"));
                var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                TempTXT = null;
                TemplatesBox.Close();
                CheckForSaving();
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, false, true, false);
                    else if (TXTFile == null) await SaveFile(false, false, true, false);
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                async void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                    var ValueRTB = RichEditBoxConverter.GetText(RTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, "");
                    var ValueREB = RichEditBoxConverter.GetText(REB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                    HomePage.Visibility = Visibility.Collapsed;
                    TXTFile = null;
                    var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///EssayTemp.rtf"));
                    var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                    REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                    TempTXT = null;
                    TemplatesBox.Close();
                    CheckForSaving();
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
        }

        private async void Button_Click_22(object sender, RoutedEventArgs e)
        {
            Zippy.Source = new Uri("ms-appx:///ZippyFolder.mp4");

            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
            if (Value == SecValue)
            {
                RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                var ValueRTB = RichEditBoxConverter.GetText(RTB);

                REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                REB.Document.SetText(TextSetOptions.FormatRtf, "");
                var ValueREB = RichEditBoxConverter.GetText(REB);

                RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                HomePage.Visibility = Visibility.Collapsed;
                TXTFile = null;
                var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ResumeTemp.rtf"));
                var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                TempTXT = null;
                TemplatesBox.Close();
                CheckForSaving();
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, false, true, false);
                    else if (TXTFile == null) await SaveFile(false, false, true, false);
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                async void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                    var ValueRTB = RichEditBoxConverter.GetText(RTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, "");
                    var ValueREB = RichEditBoxConverter.GetText(REB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                    HomePage.Visibility = Visibility.Collapsed;
                    TXTFile = null;
                    var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ResumeTemp.rtf"));
                    var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                    REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                    TempTXT = null;
                    TemplatesBox.Close();
                    CheckForSaving();
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
        }

        private async void Button_Click_23(object sender, RoutedEventArgs e)
        {
            Zippy.Source = new Uri("ms-appx:///ZippyFolder.mp4");

            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
            if (Value == SecValue)
            {
                RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                var ValueRTB = RichEditBoxConverter.GetText(RTB);

                REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                REB.Document.SetText(TextSetOptions.FormatRtf, "");
                var ValueREB = RichEditBoxConverter.GetText(REB);

                RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                HomePage.Visibility = Visibility.Collapsed;
                TXTFile = null;
                var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CreditsTemp.rtf"));
                var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                TempTXT = null;
                TemplatesBox.Close();
                CheckForSaving();
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, false, true, false);
                    else if (TXTFile == null) await SaveFile(false, false, true, false);
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                async void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                    var ValueRTB = RichEditBoxConverter.GetText(RTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, "");
                    var ValueREB = RichEditBoxConverter.GetText(REB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                    HomePage.Visibility = Visibility.Collapsed;
                    TXTFile = null;
                    var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CreditsTemp.rtf"));
                    var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                    REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                    TempTXT = null;
                    TemplatesBox.Close();
                    CheckForSaving();
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
        }

        private async void Button_Click_24(object sender, RoutedEventArgs e)
        {
            Zippy.Source = new Uri("ms-appx:///ZippyFolder.mp4");

            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
            if (Value == SecValue)
            {
                RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                var ValueRTB = RichEditBoxConverter.GetText(RTB);

                REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                REB.Document.SetText(TextSetOptions.FormatRtf, "");
                var ValueREB = RichEditBoxConverter.GetText(REB);

                RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                HomePage.Visibility = Visibility.Collapsed;
                TXTFile = null;
                var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ImageEssayTemp.rtf"));
                var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                TempTXT = null;
                TemplatesBox.Close();
                CheckForSaving();
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, false, true, false);
                    else if (TXTFile == null) await SaveFile(false, false, true, false);
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                async void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                    var ValueRTB = RichEditBoxConverter.GetText(RTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, "");
                    var ValueREB = RichEditBoxConverter.GetText(REB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                    HomePage.Visibility = Visibility.Collapsed;
                    TXTFile = null;
                    var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ImageEssayTemp.rtf"));
                    var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                    REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                    TempTXT = null;
                    TemplatesBox.Close();
                    CheckForSaving();
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
        }

        private async void Button_Click_25(object sender, RoutedEventArgs e)
        {
            Zippy.Source = new Uri("ms-appx:///ZippyFolder.mp4");

            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
            if (Value == SecValue)
            {
                RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                var ValueRTB = RichEditBoxConverter.GetText(RTB);

                REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                REB.Document.SetText(TextSetOptions.FormatRtf, "");
                var ValueREB = RichEditBoxConverter.GetText(REB);

                RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                HomePage.Visibility = Visibility.Collapsed;
                TXTFile = null;
                var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ImageGalleryTemp.rtf"));
                var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                TempTXT = null;
                TemplatesBox.Close();
                CheckForSaving();
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, false, true, false);
                    else if (TXTFile == null) await SaveFile(false, false, true, false);
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                async void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                    var ValueRTB = RichEditBoxConverter.GetText(RTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, "");
                    var ValueREB = RichEditBoxConverter.GetText(REB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                    HomePage.Visibility = Visibility.Collapsed;
                    TXTFile = null;
                    var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ImageGalleryTemp.rtf"));
                    var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                    REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                    TempTXT = null;
                    TemplatesBox.Close();
                    CheckForSaving();
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
        }

        private async void Button_Click_26(object sender, RoutedEventArgs e)
        {
            Zippy.Source = new Uri("ms-appx:///ZippyFolder.mp4");

            var Value = RichEditBoxConverter.GetText(REB);
            var SecValue = RichEditBoxConverter.GetText(RTB);
            var EmptyValue = RichEditBoxConverter.GetText(EmptyRTB);
            if (Value == SecValue)
            {
                RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                var ValueRTB = RichEditBoxConverter.GetText(RTB);

                REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                REB.Document.SetText(TextSetOptions.FormatRtf, "");
                var ValueREB = RichEditBoxConverter.GetText(REB);

                RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                HomePage.Visibility = Visibility.Collapsed;
                TXTFile = null;
                var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///NewspaperTemp.rtf"));
                var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                TempTXT = null;
                TemplatesBox.Close();
                CheckForSaving();
            }
            else
            {
                //Remember the user to save the file
                FileSaveBox.FirstButtonClick += ED2_PrimaryButtonClick;
                FileSaveBox.SecondButtonClick += ED2_SecondaryButtonClick;
                FileSaveBox.CancelButtonClick += ED2_CloseButtonClick;
                try { FileSaveBox.Open(); } catch { }
                async void ED2_PrimaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    if (TXTFile != null) await SaveFile(true, false, true, false);
                    else if (TXTFile == null) await SaveFile(false, false, true, false);
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                async void ED2_SecondaryButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    RTB.Document.SetText(TextSetOptions.FormatRtf, EmptyValue);
                    var ValueRTB = RichEditBoxConverter.GetText(RTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, ValueRTB);
                    REB.Document.SetText(TextSetOptions.FormatRtf, "");
                    var ValueREB = RichEditBoxConverter.GetText(REB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, ValueREB);
                    RTB.Document.SetText(TextSetOptions.FormatRtf, "");
                    HomePage.Visibility = Visibility.Collapsed;
                    TXTFile = null;
                    var TempTXT = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///NewspaperTemp.rtf"));
                    var RAStream = await TempTXT.OpenAsync(FileAccessMode.Read);
                    REB.Document.LoadFromStream(TextSetOptions.FormatRtf, RAStream);
                    TempTXT = null;
                    TemplatesBox.Close();
                    CheckForSaving();
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
                void ED2_CloseButtonClick(object SenderSec, RoutedEventArgs DialogEvArgs)
                {
                    FileSaveBox.Close();
                    FileSaveBox.FirstButtonClick -= ED2_PrimaryButtonClick;
                    FileSaveBox.SecondButtonClick -= ED2_SecondaryButtonClick;
                    FileSaveBox.CancelButtonClick -= ED2_CloseButtonClick;
                }
            }
        }

        #endregion Templates

        #endregion Home Page

        #region View modes

        private void MenuFlyoutItem_Click_111(object sender, RoutedEventArgs e)
        {
            TopMid.Visibility = Visibility.Visible;
            SP.Visibility = Visibility.Visible;
            RulerGrid.Visibility = Visibility.Visible;
            Bottom.Visibility = Visibility.Visible;
            ToolbarShowButton.Visibility = Visibility.Collapsed;
        }

        private void MenuFlyoutItem_Click_112(object sender, RoutedEventArgs e)
        {
            TopMid.Visibility = Visibility.Collapsed;
            SP.Visibility = Visibility.Visible;
            RulerGrid.Visibility = Visibility.Visible;
            Bottom.Visibility = Visibility.Visible;
            ToolbarShowButton.Visibility = Visibility.Collapsed;
        }

        private void MenuFlyoutItem_Click_113(object sender, RoutedEventArgs e)
        {
            TopMid.Visibility = Visibility.Collapsed;
            SP.Visibility = Visibility.Collapsed;
            RulerGrid.Visibility = Visibility.Collapsed;
            Bottom.Visibility = Visibility.Collapsed;
            ToolbarShowButton.Visibility = Visibility.Visible;
        }

        #endregion View modes

        #region Zippy

        public void LoadZippySetting()
        {
            ApplicationDataContainer LS = ApplicationData.Current.LocalSettings;
            if (LS.Values["ZippyEnabled"] != null)
            {
                if ((bool)LS.Values["ZippyEnabled"] == true)
                {
                    ZippyToggle.IsChecked = true;
                    ZippyGrid.Visibility = Visibility.Visible;
                }
                else if ((bool)LS.Values["ZippyEnabled"] == false)
                {
                    ZippyToggle.IsChecked = false;
                    ZippyGrid.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                LS.Values["ZippyEnabled"] = true;
                ZippyToggle.IsChecked = true;
                ZippyGrid.Visibility = Visibility.Visible;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer LS = ApplicationData.Current.LocalSettings;
            LS.Values["ZippyEnabled"] = true;
            if (ZippyGrid != null)
            {
                ZippyGrid.Visibility = Visibility.Visible;
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer LS = ApplicationData.Current.LocalSettings;
            LS.Values["ZippyEnabled"] = false;
            if (ZippyGrid != null)
            {
                ZippyGrid.Visibility = Visibility.Collapsed;
            }
        }

        private readonly List<string> Facts = new List<string>()
        {
            "This app was published in October 2021. It's been a long way since then!",
            "You can do all sorts of fun stuff using the console from the developer mode of this app!",
            "Both aero themes use a combination of aero effects and acrylic materials to make the noisy blur and the glass texture possible at the same time on your version of Windows!",
            "This app didn't always look this good... And it had no full dark theme!",
            "I might look familiar to you. I'm definetly not Clippy's paper...",
            "Even though it's obvious that many companies do not respect the user's privacy, your name that you have introduced in the app's setup cannot be accesed by Ivirius. Privacy matters!",
            "This app runs in a sandbox environment, which means that everything you do in it will have limited interactions with your sistem. It's called an UWP (Universal Windows Platform) app!",
            "This app looks and runs better on Windows 11. It's not a problem if you're running Windows 10 on this computer, but trust me, the graphics are so much better on 11!",
            "I am not an AI. I'm as simple as I can get. Everything I do is limited to what you see here.",
            "I love sitting here, drinking some coffee and watching you work. You see this couch? It was a real hassle to assemble! Unlike Clippy, I can actually be a bit useful! Don't worry, I promise not to remind you that you're writing a letter!",
            "Did you know there's one easter egg hidden in here? Try to press TAB and when you see a little circle selected above the workspace, press Enter. Please don't tell my developers! They might patch it soon...",
            "I can take different shapes, depending on what you're doing in the app. Try printing this document, and then look at me!",
            "This place is way more fun than any other text editor! It almost feel like an adventure game sometimes. Try it for yourself and discover some little secrets hiding here and there!",
            "Can you believe I was first supposed to be a re-imagined version of Clippy? It's much more fun and creative to be a document though!",
            "Watch me for a couple of minutes and try to notice the perfect loop. I bet you can't do it on your first try!",
            "Do you know which feature of this app I like the most? The autosave! Try it for yourself!",
            "Did you know I'm more famous than Clippy? Try playing rock, PAPER, scissors!",
            "Did you know my grandparents weren't made out of trees?",
            "Windows 7 marked the era of greatness for Microsoft. Honestly, how many remnants of it can you count in Windows 10 and 11? More than 100 for sure!",
            "Have you ever tried peeling an old Intel sticker? It's got a little easter egg on it!"
        };

        private readonly List<string> Tips = new List<string>()
        {
            "Use Ctrl + TAB to insert a TAB in your document",
            "Before closing a tab, check if the file is saved by looking at the indicator... unless you trust the autosave function!",
            "Use the F2 key to open Settings",
            "The quick access toolbar you see at the top is actually very useful. Check it out!",
            "Please don't make me sad. If you do, then I might not give you access to some of my features!",

            //ONLY FOR FREE
            "This app has some experimental features built-in. Most of them do not require the toggle, but those who do are actually considered unstable, so watch out for those!",
            "Did you know that there's a paid version of this app? Click the button above me to check it out. Don't worry, this free version has its advantges too!",

            "Use the templates for quick text formatting. They're useful when you're in a hurry!",
            "If you can't find an action inside the app, tap me to use the search function!",

            //ONLY FOR +
            //"You can use this app for any type of text file! Just make sure to save it the correct format at the end. Also, you might want to disable the autosave feature when working with plain text files!",
            //"There's no experimental features here, and neither untested ones. If you want to experiment with some, go to the free version!",

            "If the app crashes, contact the developers! It's very important to let them know if you have an issue. Don't worry, the waitlist is usually empty, so your e-mail will be read very soon!",
            "Don't forget about the ruler and status bar! They are very useful for the quickest of actions!",
        };

        private readonly List<string> Jokes = new List<string>()
        {
            "It looks like you're trying to be funny. Would you like help?",
            "It looks like you're deleting some files. Let me format the drive for you, just to speed things up!",
            "If somebody has ever called you annoying, then they haven't met me yet!",
            "Yes, I do have a search function, but it's not Google, it acts more like Bing!",
            "Wow, you have written so much! Don't worry, I won't tell anybody you copied it all from Wikipedia!",
            "Sometimes I watch you sleep",
            "Why is my name Zippy, honestly? Well, my developers asked Bing for a name!",
            "You want some dark humor? Put on the dark theme, turn off my lamp and give me a flashlight... Or a torch, if you're british!",
            "This looks fun! Would be a shame if someone... crashed the computer!",
            "Why are you typing so slow? Did you eat all of my swedish meatballs again?",
            "If a paper cuts your finger, then I cut your sanity!",
            "Do you want the best out of your PC? Use Windows XP! It also comes with my brother, Clippy!",
            "Have you ever wondered why Clippy is so chaotic? He was made by Microsoft when Steve Ballmer was still in charge!",
            "Don't look at me like that! It's not my fault you've run out of paper!",
            "Let's play rock, paper, scissors! Maybe i will take my revenge on The Rock!",
            "Do you know what plant is inside my pot? It's the one from the Settings that tells you to stop emitting carbon!",
            "Have you ever tried using your computer without Windows Explorer? Trust me, it's totally normal!",
            "Go to Performance Monitor, click on the \"Restore Window\" button under the title bar, and prepare to scream of confusion!"
        };

        private readonly List<string> TellSomething = new List<string>()
        {
            "It looks like you're writing something. Would you like a hug?",
            "You seem to be working hard in here. How about taking a break and playing some Minesweeper with me? Believe me, I'm competing in the Minesweeper championship!",
            "I see you like using Ivirius Text Editor. Did you forget about WordPad?",
            "Are you an advanced user of Ivirius Text Editor? When I'm bored, I like to play with the console. It's like a little command prompt!",
            "Don't look at me like I'm an old man. Clippy may have been born in 1995, but I now have the knowledge of modern things, unlike Clippy!",
            "When I'm bored, I like to invite Clippy in here and play with him on my classic Pac-Man arcade!",
            "I love sitting here, drinking some coffee. I'm feeling a bit sleepy. Could you turn off the lamp for me please?",
            "Sometimes I wonder from what type of tree I come",
            "Could you please wet the plant for me?",
            "What are you afraid of? I'm afraid of scissors and water!",
            "Give me a treat please!",
            "Weee! This is so fun!",
            "I'm going nuts!"
        };

        private void MenuFlyoutItem_Click_23(object sender, RoutedEventArgs e)
        {
            try
            {
                var rnd = new Random();
                var r = rnd.Next(Facts.Count);
                ZippyMessageFlyout.Title = "💡 Fun fact!";
                ZippyMessageFlyout.Subtitle = Facts[r];
                ZippyMessageFlyout.IsOpen = true;
            }
            catch
            {
                return;
            }
        }

        private void Zippy_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var LS = ApplicationData.Current.LocalSettings;
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                var p = e.GetCurrentPoint((UIElement)sender);
                if (p.Properties.IsLeftButtonPressed)
                {
                    if (15 > (int)LS.Values["Happiness"] && (int)LS.Values["Happiness"] >= 0)
                    {
                        try
                        {
                            ZippyMessageFlyout.Title = "😢";
                            ZippyMessageFlyout.Subtitle = "You have made me sad. No search for you!";
                            ZippyMessageFlyout.IsOpen = true;
                        }
                        catch
                        {
                            return;
                        }
                    }
                    else
                    {
                        SearchBlock.Flyout.ShowAt(REB);
                    }
                }
                else if (p.Properties.IsRightButtonPressed)
                {
                    MoreOptionsBlock.Flyout.ShowAt(Zippy);
                }
            }
        }

        private void MenuFlyoutItem_Click_24(object sender, RoutedEventArgs e)
        {
            try
            {
                var rnd = new Random();
                var r = rnd.Next(Tips.Count);
                ZippyMessageFlyout.Title = "💡 Tip!";
                ZippyMessageFlyout.Subtitle = Tips[r];
                ZippyMessageFlyout.IsOpen = true;
            }
            catch
            {
                return;
            }
        }

        private void MenuFlyoutItem_Click_26(object sender, RoutedEventArgs e)
        {
            try
            {
                var rnd = new Random();
                var r = rnd.Next(Jokes.Count);
                ZippyMessageFlyout.Title = "🤣";
                ZippyMessageFlyout.Subtitle = Jokes[r];
                ZippyMessageFlyout.IsOpen = true;
            }
            catch
            {
                return;
            }
        }

        private void MenuFlyoutItem_Click_27(object sender, RoutedEventArgs e)
        {
            try
            {
                var rnd = new Random();
                var r = rnd.Next(TellSomething.Count);
                ZippyMessageFlyout.Title = "🗒";
                ZippyMessageFlyout.Subtitle = TellSomething[r];
                ZippyMessageFlyout.IsOpen = true;
                if (ZippyMessageFlyout.Subtitle == "Weee! This is so fun!" || ZippyMessageFlyout.Subtitle == "I'm going nuts!")
                {
                    Zippy.Source = new Uri("ms-appx:///ZippyNuts.mp4");
                }
            }
            catch
            {
                return;
            }
        }

        public void CheckZippyHappiness()
        {
            var LS = ApplicationData.Current.LocalSettings;
            if (LS.Values["Happiness"] != null)
            {
                if ((int)LS.Values["Happiness"] > 200)
                {
                    FactItem.Visibility = Visibility.Visible;
                    TellSomethingItem.Visibility = Visibility.Visible;
                    JokeItem.Visibility = Visibility.Visible;
                    BRedFull.Visibility = Visibility.Collapsed;
                    BRed.Visibility = Visibility.Collapsed;
                    BYellow.Visibility = Visibility.Collapsed;
                    BGreen.Visibility = Visibility.Visible;
                    Zippy.Visibility = Visibility.Visible;
                    LS.Values["Happiness"] = 200;
                }
                if (200 >= (int)LS.Values["Happiness"] && (int)LS.Values["Happiness"] >= 120)
                {
                    FactItem.Visibility = Visibility.Visible;
                    TellSomethingItem.Visibility = Visibility.Visible;
                    JokeItem.Visibility = Visibility.Visible;
                    BRedFull.Visibility = Visibility.Collapsed;
                    BRed.Visibility = Visibility.Collapsed;
                    BYellow.Visibility = Visibility.Collapsed;
                    BGreen.Visibility = Visibility.Visible;
                    Zippy.Visibility = Visibility.Visible;
                }
                if (120 > (int)LS.Values["Happiness"] && (int)LS.Values["Happiness"] >= 50)
                {
                    FactItem.Visibility = Visibility.Visible;
                    TellSomethingItem.Visibility = Visibility.Visible;
                    JokeItem.Visibility = Visibility.Collapsed;
                    BRedFull.Visibility = Visibility.Collapsed;
                    BRed.Visibility = Visibility.Collapsed;
                    BYellow.Visibility = Visibility.Visible;
                    BGreen.Visibility = Visibility.Collapsed;
                    Zippy.Visibility = Visibility.Visible;
                }
                if (50 > (int)LS.Values["Happiness"] && (int)LS.Values["Happiness"] >= 15)
                {
                    FactItem.Visibility = Visibility.Visible;
                    TellSomethingItem.Visibility = Visibility.Collapsed;
                    JokeItem.Visibility = Visibility.Collapsed;
                    BRedFull.Visibility = Visibility.Collapsed;
                    BRed.Visibility = Visibility.Visible;
                    BYellow.Visibility = Visibility.Collapsed;
                    BGreen.Visibility = Visibility.Collapsed;
                    Zippy.Visibility = Visibility.Visible;
                }
                if (15 > (int)LS.Values["Happiness"] && (int)LS.Values["Happiness"] >= 0)
                {
                    FactItem.Visibility = Visibility.Collapsed;
                    TellSomethingItem.Visibility = Visibility.Collapsed;
                    JokeItem.Visibility = Visibility.Collapsed;
                    BRedFull.Visibility = Visibility.Visible;
                    BRed.Visibility = Visibility.Collapsed;
                    BYellow.Visibility = Visibility.Collapsed;
                    BGreen.Visibility = Visibility.Collapsed;
                    Zippy.Visibility = Visibility.Collapsed;
                }
                if ((int)LS.Values["Happiness"] < 0)
                {
                    FactItem.Visibility = Visibility.Collapsed;
                    TellSomethingItem.Visibility = Visibility.Collapsed;
                    JokeItem.Visibility = Visibility.Collapsed;
                    BRedFull.Visibility = Visibility.Visible;
                    BRed.Visibility = Visibility.Collapsed;
                    BYellow.Visibility = Visibility.Collapsed;
                    BGreen.Visibility = Visibility.Collapsed;
                    Zippy.Visibility = Visibility.Collapsed;
                    LS.Values["Happiness"] = 0;
                }
                try
                {
                    HappinessText.Text = $"Happiness: {LS.Values["Happiness"]} / 200";
                    HappinessBar.Value = Convert.ToDouble(LS.Values["Happiness"]);
                }
                catch
                {
                    HappinessText.Text = "Happiness cannot be currently shown due to an unhandled exception";
                    HappinessBar.Value = 0;
                }
            }
        }

        private void MenuFlyoutItem_Click_28(object sender, RoutedEventArgs e)
        {
            try
            {
                var LS = ApplicationData.Current.LocalSettings;
                var I = (int)LS.Values["Happiness"];
                I += 5;
                LS.Values["Happiness"] = I;
                Zippy.Source = new Uri("ms-appx:///ZippyEatsACookie.mp4");

                CheckZippyHappiness();
            }
            catch { }
        }

        private void MenuFlyoutItem_Click_29(object sender, RoutedEventArgs e)
        {
            try
            {
                var LS = ApplicationData.Current.LocalSettings;
                var I = (int)LS.Values["Happiness"];
                I -= 5;
                LS.Values["Happiness"] = I;
                Zippy.Source = new Uri("ms-appx:///ZippyAngry.mp4");

                CheckZippyHappiness();
            }
            catch { }
        }

        private void MenuFlyoutItem_Click_30(object sender, RoutedEventArgs e)
        {
            try
            {
                var LS = ApplicationData.Current.LocalSettings;
                var I = (int)LS.Values["Happiness"];
                I += 1;
                LS.Values["Happiness"] = I;
                CheckZippyHappiness();
            }
            catch { }
        }

        private void Zippy_MediaEnded(object sender, RoutedEventArgs e)
        {
            Zippy.Source = ZippySource.Source;
        }

        #endregion Zippy

        #region Search

        private List<string> SearchOptions = new List<string>()
        {
            "Cut",
            "Copy",
            "Paste",
            "Select All",
            "Bold",
            "Italic",
            "Underline",
            "Strikethrough",
            "Subscript",
            "Superscript",
            "Font color",
            "Highlight",
            "List",
            "Erase formatting",
            "Font family",
            "Font size",
            "Paragraph alignment",
            "Left indent",
            "Right indent",
            "View modes",
            "Tablet writing",
            "Undo",
            "Redo",
            "Find and replace"
        };

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var option in SearchOptions)
                {
                    var found = splitText.All((key) =>
                    {
                        return option.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(option);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }

        }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if ((string)args.SelectedItem == "Cut")
            {
                _ = CTB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Copy")
            {
                _ = CB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Paste")
            {
                _ = PB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Select All")
            {
                _ = SAB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Bold")
            {
                _ = BB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Italic")
            {
                _ = IB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Underline")
            {
                _ = UB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Strikethrough")
            {
                _ = STB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Subscript")
            {
                _ = SubscriptButton.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Superscript")
            {
                _ = SuperscriptButton.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Font color")
            {
                _ = CPDDB.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Highlight")
            {
                _ = Back.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "List")
            {
                _ = ListBox.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Erase formatting")
            {
                _ = EraseFormatBTN.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Font family")
            {
                _ = FontBox.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Font size")
            {
                _ = FSize.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Paragraph alignment")
            {
                _ = LeftAl.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Left indent")
            {
                _ = LeftIndent.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Right indent")
            {
                _ = RightIndent.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "View modes")
            {
                _ = ToolbarOptionsButton.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Tablet writing")
            {
                _ = HandButton.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Undo")
            {
                _ = Undo.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Redo")
            {
                _ = Redo.Focus(FocusState.Keyboard);
            }
            if ((string)args.SelectedItem == "Find and replace")
            {
                _ = FindButton.Focus(FocusState.Keyboard);
            }
        }

        #endregion Search

        #region Spell check

        private void SCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SCheckOn();
        }

        public void SCheckOn()
        {
            var LS = ApplicationData.Current.LocalSettings;
            LS.Values["SCheck"] = "On";
            REB.IsSpellCheckEnabled = true;
            var T = RichEditBoxConverter.GetText(EmptyRTB);
            var Y = RichEditBoxConverter.GetText(REB);
            if (T != Y)
            {
                var Options = TextSetOptions.FormatRtf;
                REB.Document.GetText(TextGetOptions.FormatRtf, out _);
                REB.Document.SetText(Options, Y);
                string value = RichEditBoxConverter.GetText(REB);
                var lastNewLine = value.LastIndexOf("\\par", StringComparison.Ordinal);
                value = value.Remove(lastNewLine, "\\par".Length);
                REB.Document.SetText(Options, string.Empty);
                var options = TextSetOptions.FormatRtf | TextSetOptions.ApplyRtfDocumentDefaults;
                REB.Document.SetText(options, value);
            }
            else
            {

            }
        }

        public void SCheckOff()
        {
            var LS = ApplicationData.Current.LocalSettings;
            LS.Values["SCheck"] = "Off";
            REB.IsSpellCheckEnabled = false;
            var T = RichEditBoxConverter.GetText(EmptyRTB);
            var Y = RichEditBoxConverter.GetText(REB);
            if (T != Y)
            {
                var Options = TextSetOptions.FormatRtf;
                REB.Document.GetText(TextGetOptions.FormatRtf, out _);
                REB.Document.SetText(Options, Y);
                string value = RichEditBoxConverter.GetText(REB);
                var lastNewLine = value.LastIndexOf("\\par", StringComparison.Ordinal);
                value = value.Remove(lastNewLine, "\\par".Length);
                REB.Document.SetText(Options, string.Empty); 
                var options = TextSetOptions.FormatRtf | TextSetOptions.ApplyRtfDocumentDefaults;
                REB.Document.SetText(options, value);
            }
            else
            {

            }
        }

        private void SCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SCheckOff();
        }

        #endregion Spell check

        private void MenuFlyoutFontSize_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuFlyoutItem).Text == "8") REB.Document.Selection.CharacterFormat.Size = 8;
            if ((sender as MenuFlyoutItem).Text == "9") REB.Document.Selection.CharacterFormat.Size = 9;
            if ((sender as MenuFlyoutItem).Text == "10") REB.Document.Selection.CharacterFormat.Size = 10;
            if ((sender as MenuFlyoutItem).Text == "10.5" || (sender as MenuFlyoutItem).Text == "Default") REB.Document.Selection.CharacterFormat.Size = (float)10.5;
            if ((sender as MenuFlyoutItem).Text == "11") REB.Document.Selection.CharacterFormat.Size = 11;
            if ((sender as MenuFlyoutItem).Text == "12") REB.Document.Selection.CharacterFormat.Size = 12;
            if ((sender as MenuFlyoutItem).Text == "14") REB.Document.Selection.CharacterFormat.Size = 14;
            if ((sender as MenuFlyoutItem).Text == "16") REB.Document.Selection.CharacterFormat.Size = 16;
            if ((sender as MenuFlyoutItem).Text == "18") REB.Document.Selection.CharacterFormat.Size = 18;
            if ((sender as MenuFlyoutItem).Text == "20") REB.Document.Selection.CharacterFormat.Size = 20;
            if ((sender as MenuFlyoutItem).Text == "22") REB.Document.Selection.CharacterFormat.Size = 22;
            if ((sender as MenuFlyoutItem).Text == "24") REB.Document.Selection.CharacterFormat.Size = 24;
            if ((sender as MenuFlyoutItem).Text == "26") REB.Document.Selection.CharacterFormat.Size = 26;
            if ((sender as MenuFlyoutItem).Text == "28") REB.Document.Selection.CharacterFormat.Size = 28;
            if ((sender as MenuFlyoutItem).Text == "36") REB.Document.Selection.CharacterFormat.Size = 36;
            if ((sender as MenuFlyoutItem).Text == "48") REB.Document.Selection.CharacterFormat.Size = 48;
            if ((sender as MenuFlyoutItem).Text == "72") REB.Document.Selection.CharacterFormat.Size = 72;
        }

        private void LeftAl_Click(object sender, RoutedEventArgs e)
        {
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.ParagraphFormat.Alignment;
                if (CF != ParagraphAlignment.Left) CF = ParagraphAlignment.Left;
                else CF = ParagraphAlignment.Justify;
                ST.ParagraphFormat.Alignment = CF;
            }
            CheckAlignment();
        }

        public void CheckAlignment()
        {
            Style S = new Style(typeof(Button));
            S.BasedOn = Application.Current.Resources["AccentButtonStyle"] as Style;
            if (REB.Document.Selection.ParagraphFormat.Alignment == ParagraphAlignment.Left)
            {
                LeftAl.Style = S;
                RightAl.Style = null;
                CenterAl.Style = null;
                JustifyAl.Style = null;
            }
            if (REB.Document.Selection.ParagraphFormat.Alignment == ParagraphAlignment.Center)
            {
                LeftAl.Style = null;
                RightAl.Style = null;
                CenterAl.Style = S;
                JustifyAl.Style = null;
            }
            if (REB.Document.Selection.ParagraphFormat.Alignment == ParagraphAlignment.Right)
            {
                LeftAl.Style = null;
                RightAl.Style = S;
                CenterAl.Style = null;
                JustifyAl.Style = null;
            }
            if (REB.Document.Selection.ParagraphFormat.Alignment == ParagraphAlignment.Justify)
            {
                LeftAl.Style = null;
                RightAl.Style = null;
                CenterAl.Style = null;
                JustifyAl.Style = S;
            }
            if (REB.Document.Selection.ParagraphFormat.Alignment == ParagraphAlignment.Undefined)
            {
                LeftAl.Style = null;
                RightAl.Style = null;
                CenterAl.Style = null;
                JustifyAl.Style = null;
            }
        }

        private void CenterAl_Click(object sender, RoutedEventArgs e)
        {
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.ParagraphFormat.Alignment;
                if (CF != ParagraphAlignment.Center) CF = ParagraphAlignment.Center;
                else CF = ParagraphAlignment.Left;
                ST.ParagraphFormat.Alignment = CF;
            }
            CheckAlignment();
        }

        private void RightAl_Click(object sender, RoutedEventArgs e)
        {
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.ParagraphFormat.Alignment;
                if (CF != ParagraphAlignment.Right) CF = ParagraphAlignment.Right;
                else CF = ParagraphAlignment.Left;
                ST.ParagraphFormat.Alignment = CF;
            }
            CheckAlignment();
        }

        private void JustifyAl_Click(object sender, RoutedEventArgs e)
        {
            var ST = REB.Document.Selection;
            if (ST != null)
            {
                var CF = ST.ParagraphFormat.Alignment;
                if (CF != ParagraphAlignment.Justify) CF = ParagraphAlignment.Justify;
                else CF = ParagraphAlignment.Left;
                ST.ParagraphFormat.Alignment = CF;
            }
            CheckAlignment();
        }

        public void CheckFormatting()
        {
            Style S = new Style(typeof(Button));
            S.BasedOn = Application.Current.Resources["AccentButtonStyle"] as Style;
            if (REB.Document.Selection.CharacterFormat.Bold == FormatEffect.On)
            {
                BB.Style = S;
            }
            if (REB.Document.Selection.CharacterFormat.Bold != FormatEffect.On)
            {
                BB.Style = null;
            }
            if (REB.Document.Selection.CharacterFormat.Italic == FormatEffect.On)
            {
                IB.Style = S;
            }
            if (REB.Document.Selection.CharacterFormat.Italic != FormatEffect.On)
            {
                IB.Style = null;
            }
            if (REB.Document.Selection.CharacterFormat.Strikethrough == FormatEffect.On)
            {
                STB.Style = S;
            }
            if (REB.Document.Selection.CharacterFormat.Strikethrough != FormatEffect.On)
            {
                STB.Style = null;
            }
            if (REB.Document.Selection.CharacterFormat.Subscript == FormatEffect.On)
            {
                SubscriptButton.Style = S;
            }
            if (REB.Document.Selection.CharacterFormat.Subscript != FormatEffect.On)
            {
                SubscriptButton.Style = null;
            }
            if (REB.Document.Selection.CharacterFormat.Superscript == FormatEffect.On)
            {
                SuperscriptButton.Style = S;
            }
            if (REB.Document.Selection.CharacterFormat.Superscript != FormatEffect.On)
            {
                SuperscriptButton.Style = null;
            }
        }

        private void IndReset_Click(object sender, RoutedEventArgs e)
        {
            LeftIndent.Value = 0;
            RightIndent.Value = 0;
        }

        private void NumberBox_ValueChanged(Microsoft.UI.Xaml.Controls.NumberBox sender, Microsoft.UI.Xaml.Controls.NumberBoxValueChangedEventArgs args)
        {
            //Configure font size
            if (!(REB == null))
            {
                var ST = REB.Document.Selection;
                if (!(ST == null))
                {
                    _ = ST.CharacterFormat.Size;
                    if (FSize != null && FSize.Value != double.NaN && FSize.Value != 0)
                    {
                        try
                        {
                            var CF = (float)FSize.Value;
                            ST.CharacterFormat.Size = CF;
                        }
                        catch { }
                    }
                    else return;
                }
            }
        }

        private void SCR_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ZoomSlider.Value = SCR.ZoomFactor;
        }

        private void ZoomReset_Click(object sender, RoutedEventArgs e)
        {
            ZoomSlider.Value = 1;
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://learn.microsoft.com/en-us/windows/apps/winui/winui2/"));
        }

        private async void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://learn.microsoft.com/en-us/windows/communitytoolkit/"));
        }

        private async void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://learn.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide"));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            ActionErrorBox.Open();
        }

        private void Button_Click_27(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_28(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_29(object sender, RoutedEventArgs e)
        {
            OutputBox.Text = "> Output \n";
        }

        private void Button_Click_30(object sender, RoutedEventArgs e)
        {
            ConsoleMSGBox.Close();
        }

        private void Button_Click_31(object sender, RoutedEventArgs e)
        {
            TriggerConsoleEvent();
        }

        private async void HyperlinkButton_Click_3(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/Humanizr/Humanizer/blob/main/LICENSE"));
        }

        private void MenuFlyoutItem_Click_6(object sender, RoutedEventArgs e)
        {
            REB.Document.Selection.GetText(TextGetOptions.None, out string Text);
            var HumanizedText = Text.Transform(To.SentenceCase);
            var Options = TextSetOptions.FormatRtf | TextSetOptions.ApplyRtfDocumentDefaults;
            REB.Document.Selection.SetText(Options, HumanizedText);
        }

        private void MenuFlyoutItem_Click_21(object sender, RoutedEventArgs e)
        {
            REB.Document.Selection.GetText(TextGetOptions.None, out string Text);
            var HumanizedText = Text.Transform(To.TitleCase);
            var Options = TextSetOptions.FormatRtf | TextSetOptions.ApplyRtfDocumentDefaults;
            REB.Document.Selection.SetText(Options, HumanizedText);
        }

        private void MenuFlyoutItem_Click_31(object sender, RoutedEventArgs e)
        {
            REB.Document.Selection.GetText(TextGetOptions.None, out string Text);
            var HumanizedText = Text.Humanize();
            var Options = TextSetOptions.FormatRtf | TextSetOptions.ApplyRtfDocumentDefaults;
            REB.Document.Selection.SetText(Options, HumanizedText);
        }

        private void Button_Click_32(object sender, RoutedEventArgs e)
        {
            FontColorBox.Open();
            CPDDB.Flyout.Hide();
        }

        private void ColorPicker_ColorChanged(Microsoft.UI.Xaml.Controls.ColorPicker sender, Microsoft.UI.Xaml.Controls.ColorChangedEventArgs args)
        {
            //Configure font color
            var ST = REB.Document.Selection;
            if (!(ST == null))
            {
                _ = ST.CharacterFormat.ForegroundColor;
                var Br = new SolidColorBrush(ColPicker.Color);
                var CF = ColPicker.Color;
                FontAccent.Foreground = Br;
                ST.CharacterFormat.ForegroundColor = CF;
            }
        }

        private async void Button_Click_400(object sender, RoutedEventArgs e)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://github.com/jpbandroid/Text-Editor/tree/master"));
        }

        private async void Button_Click_401(object sender, RoutedEventArgs e)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/ivirius-text-editor2/"));
        }

        private async void ChangelogButton_Click2(object sender, RoutedEventArgs e)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/ivirius-text-editor/"));
        }

        private void MenuFlyoutItem_Click_150(object sender, RoutedEventArgs e)
        {
            ZippyMessageFlyout.Title = "⌚ Time";
                ZippyMessageFlyout.Subtitle = "The time right now is: " + DateTime.Now.ToString();
                ZippyMessageFlyout.IsOpen = true;
        }

        private void MenuFlyoutItem_Click_151(object sender, RoutedEventArgs e)
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            ZippyMessageFlyout.Title = "👋 Hello";
                ZippyMessageFlyout.Subtitle = "Hello, " + userName;
                ZippyMessageFlyout.IsOpen = true;
        }

        private void HideSettings_Click(object Sender, RoutedEventArgs EvArgs) => Settings.Visibility = Visibility.Collapsed;

    }
}