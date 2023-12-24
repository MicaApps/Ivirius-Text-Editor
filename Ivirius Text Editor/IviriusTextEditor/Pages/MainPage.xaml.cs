using IviriusTextEditor.Core.Helpers;
using MicaForUWP.Media;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AcrylicBrush = Windows.UI.Xaml.Media.AcrylicBrush;

namespace IviriusTextEditor.Pages
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer DT = new();

        public MainPage()
        {
            InitializeComponent();

            //Variables
            var CoreTB = CoreApplication.GetCurrentView().TitleBar;
            var AppTB = ApplicationView.GetForCurrentView().TitleBar;
            var LocalSettings = ApplicationData.Current.LocalSettings;

            //Title bar
            CoreTB.ExtendViewIntoTitleBar = true;
            CoreTB.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            AppTB.ButtonBackgroundColor = Colors.Transparent;
            AppTB.ButtonInactiveBackgroundColor = Colors.Transparent;
            AppTB.ButtonInactiveForegroundColor = Color.FromArgb(255, 125, 125, 125);
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += MainPage_CloseRequested;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Windows.Foundation.Size(800, 400));
            Window.Current.SetTitleBar(CustomDragRegion);

            //Others
            try
            {
                DT.Interval = new TimeSpan(0, 0, (int)0.1);
                DT.Tick += DT_Tick;
                DT.Start();
            }
            catch { }
            TabbedView.TabItems.Clear();

            //Settings

            //Accent border settings
            if (SettingsHelper.GetSettingString("AccentBorder") == null)
            {
                SettingsHelper.SetSetting("AccentBorder", "Off");
                AccentBorder.Visibility = Visibility.Collapsed;
            }
            if (SettingsHelper.GetSettingString("AccentBorder") == "On")
            {
                AccentBorder.Visibility = Visibility.Visible;
            }
            if (SettingsHelper.GetSettingString("AccentBorder") == "Off")
            {
                AccentBorder.Visibility = Visibility.Collapsed;
            }

            //Theme settings
            if (SettingsHelper.GetSettingString("Theme") == null)
            {
                SettingsHelper.SetSetting("Theme", "Mica Light");
            }

            //Disabled themes
            if (SettingsHelper.GetSettingString("Theme") == "Custom")
            {
                SettingsHelper.SetSetting("Theme", "Mica Light");
            }
            if (SettingsHelper.GetSettingString("Theme") == "Dark")
            {
                SettingsHelper.SetSetting("Theme", "Mica Dark");
            }

            //Mica light theme
            if (SettingsHelper.GetSettingString("Theme") == "Mica Light")
            {
                //Text colors
                Application.Current.Resources["AppTextBrush"] = Application.Current.Resources["TextFillColorPrimaryBrush"];

                //TitleBar buttons
                AppTB.ButtonForegroundColor = Colors.Black;
                AppTB.ButtonHoverBackgroundColor = Color.FromArgb(25, 0, 0, 0);
                AppTB.ButtonPressedBackgroundColor = Color.FromArgb(65, 0, 0, 0);
                AppTB.ButtonHoverForegroundColor = Colors.Black;
                AppTB.ButtonPressedForegroundColor = Colors.Black;

                //TitleBar color
                Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 0.8F,
                    TintOpacity = 0F,
                    BackgroundSource = BackgroundSource.WallpaperBackdrop,
                    Opacity = 1,
                    TintColor = Color.FromArgb(255, 230, 230, 230),
                    FallbackColor = Color.FromArgb(255, 230, 230, 230)
                };
                MainPageComponent.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];

                //Tab color
                Application.Current.Resources["AppBackgroundBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 0.9F,
                    TintOpacity = 0F,
                    BackgroundSource = BackgroundSource.WallpaperBackdrop,
                    Opacity = 1,
                    TintColor = Color.FromArgb(255, 255, 255, 255),
                    FallbackColor = Color.FromArgb(255, 255, 255, 255)
                };

                Application.Current.Resources["TabViewItemHeaderBackgroundSelected"] = Application.Current.Resources["AppBackgroundBrush"];

                //Final settings
                RequestedTheme = ElementTheme.Light;
            }

            //Light theme
            if (SettingsHelper.GetSettingString("Theme") == "Light")
            {
                //Text colors
                Application.Current.Resources["AppTextBrush"] = Application.Current.Resources["TextFillColorPrimaryBrush"];

                //TitleBar buttons
                AppTB.ButtonForegroundColor = Colors.Black;
                AppTB.ButtonHoverBackgroundColor = Color.FromArgb(25, 0, 0, 0);
                AppTB.ButtonPressedBackgroundColor = Color.FromArgb(65, 0, 0, 0);
                AppTB.ButtonHoverForegroundColor = Colors.Black;
                AppTB.ButtonPressedForegroundColor = Colors.Black;

                //TitleBar color
                Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 0.8F,
                    TintOpacity = 0,
                    BackgroundSource = BackgroundSource.WallpaperBackdrop,
                    Opacity = 1,
                    TintColor = Color.FromArgb(255, 200, 200, 200),
                    FallbackColor = Colors.White
                };
                MainPageComponent.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];

                //Tab color
                Application.Current.Resources["AppBackgroundBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 0.7F,
                    TintOpacity = 0,
                    Opacity = 1,
                    TintColor = Colors.White,
                    FallbackColor = Colors.White
                };

                Application.Current.Resources["TabViewItemHeaderBackgroundSelected"] = Application.Current.Resources["AppBackgroundBrush"];

                //Final settings
                RequestedTheme = ElementTheme.Light;
            }

            //Mica dark theme
            if (SettingsHelper.GetSettingString("Theme") == "Mica Dark")
            {
                //Text colors
                Application.Current.Resources["AppTextBrush"] = Application.Current.Resources["TextFillColorPrimaryBrush"];

                //TitleBar buttons
                AppTB.ButtonForegroundColor = Colors.White;
                AppTB.ButtonHoverBackgroundColor = Color.FromArgb(25, 0, 0, 0);
                AppTB.ButtonPressedBackgroundColor = Color.FromArgb(65, 0, 0, 0);
                AppTB.ButtonHoverForegroundColor = Colors.White;
                AppTB.ButtonPressedForegroundColor = Colors.White;

                //TitleBar color
                Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 1F,
                    TintOpacity = 0.3F,
                    BackgroundSource = BackgroundSource.WallpaperBackdrop,
                    Opacity = 1,
                    TintColor = Color.FromArgb(255, 13, 13, 13),
                    FallbackColor = Color.FromArgb(255, 13, 13, 13)
                };
                MainPageComponent.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];

                //Tab color
                Application.Current.Resources["AppBackgroundBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 1F,
                    TintOpacity = 0.8F,
                    BackgroundSource = BackgroundSource.WallpaperBackdrop,
                    Opacity = 1,
                    TintColor = Color.FromArgb(255, 33, 33, 33),
                    FallbackColor = Color.FromArgb(255, 33, 33, 33)
                };

                Application.Current.Resources["TabViewItemHeaderBackgroundSelected"] = Application.Current.Resources["AppBackgroundBrush"];

                //Final settings
                RequestedTheme = ElementTheme.Dark;
            }

            //Full Dark theme
            if (SettingsHelper.GetSettingString("Theme") == "Full Dark")
            {
                //Text colors
                Application.Current.Resources["AppTextBrush"] = Application.Current.Resources["TextFillColorPrimaryBrush"];

                //TitleBar buttons
                AppTB.ButtonForegroundColor = Colors.White;
                AppTB.ButtonHoverForegroundColor = Colors.White;
                AppTB.ButtonPressedForegroundColor = Colors.White;
                AppTB.ButtonHoverBackgroundColor = Color.FromArgb(100, 79, 146, 255);
                AppTB.ButtonPressedBackgroundColor = Color.FromArgb(100, 81, 117, 176);

                //TitleBar color
                Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 0,
                    TintOpacity = 1,
                    BackgroundSource = BackgroundSource.WallpaperBackdrop,
                    Opacity = 1,
                    TintColor = (Color)Application.Current.Resources["SystemAccentColor"],
                    FallbackColor = (Color)Application.Current.Resources["SystemAccentColor"]
                };
                MainPageComponent.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];

                //Tab color
                Application.Current.Resources["AppBackgroundBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 0.2F,
                    TintOpacity = 0.8F,
                    Opacity = 1,
                    TintColor = Color.FromArgb(255, 50, 50, 50),
                    FallbackColor = Color.FromArgb(255, 50, 50, 50)
                };

                Application.Current.Resources["TabViewItemHeaderBackgroundSelected"] = Application.Current.Resources["AppBackgroundBrush"];

                //Final settings
                RequestedTheme = ElementTheme.Dark;
            }

            //Acrylic theme
            if (SettingsHelper.GetSettingString("Theme") == "Nostalgic Windows")
            {
                //Text colors
                Application.Current.Resources["AppTextBrush"] = Application.Current.Resources["TextFillColorPrimaryBrush"];

                //TitleBar buttons
                AppTB.ButtonForegroundColor = Colors.Black;
                AppTB.ButtonHoverBackgroundColor = Color.FromArgb(25, 0, 0, 0);
                AppTB.ButtonPressedBackgroundColor = Color.FromArgb(65, 0, 0, 0);
                AppTB.ButtonHoverForegroundColor = Colors.Black;
                AppTB.ButtonPressedForegroundColor = Colors.Black;

                //TitleBar color
                Application.Current.Resources["AppTitleBarBrush"] = new AcrylicBrush
                {
                    TintColor = Colors.GhostWhite,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    TintOpacity = 0.1,
                    FallbackColor = Colors.GhostWhite
                };
                MainPageComponent.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];

                //Tab color
                Application.Current.Resources["AppBackgroundBrush"] = new AcrylicBrush
                {
                    TintColor = Colors.GhostWhite,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    TintOpacity = 0.3,
                    FallbackColor = Colors.GhostWhite
                };

                Application.Current.Resources["TabViewItemHeaderBackgroundSelected"] = Application.Current.Resources["AppBackgroundBrush"];

                //Final settings
                RequestedTheme = ElementTheme.Light;
            }

            //Acrylig glass theme
            if (SettingsHelper.GetSettingString("Theme") == "Acrylic")
            {
                Application.Current.Resources["TextFillColorPrimaryBrush"] = new SolidColorBrush(Colors.Black);

                //Text colors
                Application.Current.Resources["AppTextBrush"] = Application.Current.Resources["TextFillColorPrimaryBrush"];

                //titleBar buttons
                AppTB.ButtonForegroundColor = Colors.White;
                AppTB.ButtonHoverForegroundColor = Colors.White;
                AppTB.ButtonPressedForegroundColor = Colors.White;
                AppTB.ButtonHoverBackgroundColor = Color.FromArgb(100, 79, 146, 255);
                AppTB.ButtonPressedBackgroundColor = Color.FromArgb(100, 81, 117, 176);

                //TitleBar color
                Application.Current.Resources["AppTitleBarBrush"] = new AcrylicBrush
                {
                    TintLuminosityOpacity = 0,
                    TintOpacity = 0,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    Opacity = 1,
                    TintColor = Colors.White
                };
                MainPageComponent.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];

                var GeneralBrush = new SolidColorBrush(Colors.Black);
                var GeneralBrush2 = new SolidColorBrush(Colors.White);

                //Text colors
                Application.Current.Resources["TabViewItemHeaderCloseButtonForegroundPointerOver"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderCloseButtonForegroundPressed"] = GeneralBrush2;
                Application.Current.Resources["TabViewButtonForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewButtonForegroundPointerOver"] = GeneralBrush2;
                Application.Current.Resources["TabViewButtonForegroundPressed"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderForegroundPointerOver"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderForegroundPressed"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemIconForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemIconForegroundPointerOver"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemIconForegroundPressed"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderCloseButtonForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderPointerOverCloseButtonForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderPressedCloseButtonForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemIconForegroundSelected"] = GeneralBrush;
                Application.Current.Resources["TabViewItemHeaderSelectedCloseButtonForeground"] = GeneralBrush;
                Application.Current.Resources["TabViewItemHeaderForegroundSelected"] = GeneralBrush;
                Application.Current.Resources["TabViewItemHeaderPressedCloseButtonBackground"] = Color.FromArgb(25, 50, 50, 50);
                Application.Current.Resources["TabViewItemHeaderBackgroundPressed"] = Color.FromArgb(25, 50, 50, 50);

                //Tab color
                Application.Current.Resources["AppBackgroundBrush"] = new AcrylicBrush
                {
                    TintColor = Colors.White,
                    FallbackColor = Colors.White,
                    TintOpacity = 0.6,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop
                };

                Application.Current.Resources["TabViewItemHeaderBackgroundSelected"] = Application.Current.Resources["AppBackgroundBrush"];

                //Final settings
                RequestedTheme = ElementTheme.Light;
                TabbedView.Margin = new Thickness(7, 0, 7, 7);
                AeroShine.Visibility = Visibility.Visible;
                AeroButtons.Visibility = Visibility.Visible;
                AeroCorners.Visibility = Visibility.Visible;
                AeroBlue.Visibility = Visibility.Visible;
            }

            //Luna theme
            if (SettingsHelper.GetSettingString("Theme") == "Luna")
            {
                //Text colors
                Application.Current.Resources["AppTextBrush"] = Application.Current.Resources["TextFillColorPrimaryBrush"];

                //TitleBar buttons
                AppTB.ButtonForegroundColor = Colors.White;
                AppTB.ButtonHoverForegroundColor = Colors.White;
                AppTB.ButtonPressedForegroundColor = Colors.White;
                AppTB.ButtonHoverBackgroundColor = Color.FromArgb(100, 79, 146, 255);
                AppTB.ButtonPressedBackgroundColor = Color.FromArgb(100, 81, 117, 176);

                //TitleBar color
                Application.Current.Resources["AppTitleBarBrush"] = new SolidColorBrush(Colors.White);
                MainPageComponent.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];

                var GeneralBrush = new SolidColorBrush(Colors.Black);
                var GeneralBrush2 = new SolidColorBrush(Colors.White);

                //Text colors
                Application.Current.Resources["TabViewItemHeaderCloseButtonForegroundPointerOver"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderCloseButtonForegroundPressed"] = GeneralBrush2;
                Application.Current.Resources["TabViewButtonForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewButtonForegroundPointerOver"] = GeneralBrush2;
                Application.Current.Resources["TabViewButtonForegroundPressed"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderForegroundPointerOver"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderForegroundPressed"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemIconForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemIconForegroundPointerOver"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemIconForegroundPressed"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderCloseButtonForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderPointerOverCloseButtonForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemHeaderPressedCloseButtonForeground"] = GeneralBrush2;
                Application.Current.Resources["TabViewItemIconForegroundSelected"] = GeneralBrush;
                Application.Current.Resources["TabViewItemHeaderSelectedCloseButtonForeground"] = GeneralBrush;
                Application.Current.Resources["TabViewItemHeaderForegroundSelected"] = GeneralBrush;
                Application.Current.Resources["TabViewItemHeaderPressedCloseButtonBackground"] = Color.FromArgb(25, 50, 50, 50);
                Application.Current.Resources["TabViewItemHeaderBackgroundPressed"] = Color.FromArgb(25, 50, 50, 50);

                //Tab color
                Application.Current.Resources["AppBackgroundBrush"] = new AcrylicBrush
                {
                    TintColor = Color.FromArgb(255, 255, 254, 217),
                    FallbackColor = Color.FromArgb(255, 255, 254, 217),
                    TintOpacity = 1,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop
                };

                Application.Current.Resources["TabViewItemHeaderBackgroundSelected"] = Application.Current.Resources["AppBackgroundBrush"];

                //Final settings
                TabbedView.Margin = new Thickness(7, 0, 7, 7);
                RequestedTheme = ElementTheme.Light;
                LunaTheme.Visibility = Visibility.Visible;
            }

            //Old theme
            if (SettingsHelper.GetSettingString("Theme") == "Old")
            {
                //Text colors
                Application.Current.Resources["AppTextBrush"] = new SolidColorBrush(Colors.Black);

                //TitleBar buttons
                AppTB.ButtonForegroundColor = Colors.Black;
                AppTB.ButtonHoverBackgroundColor = Color.FromArgb(25, 0, 0, 0);
                AppTB.ButtonPressedBackgroundColor = Color.FromArgb(65, 0, 0, 0);
                AppTB.ButtonHoverForegroundColor = Colors.Black;
                AppTB.ButtonPressedForegroundColor = Colors.Black;

                //TitleBar color
                Application.Current.Resources["AppTitleBarBrush"] = new AcrylicBrush
                {
                    TintColor = Colors.LightBlue,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    TintOpacity = 0.8,
                    FallbackColor = Colors.LightBlue
                };
                MainPageComponent.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];

                //Tab color
                Application.Current.Resources["AppBackgroundBrush"] = Application.Current.Resources["AppTitleBarBrush"];

                Application.Current.Resources["TabViewItemHeaderBackgroundSelected"] = Application.Current.Resources["AppBackgroundBrush"];

                //Final settings
                RequestedTheme = ElementTheme.Light;
                AeroBlue.Visibility = Visibility.Collapsed;
            }
        }

        private void DT_Tick(object sender, object e)
        {
            IList<object> TabItems = TabbedView.TabItems;
            foreach (TabViewItem TabItem in TabItems.Cast<TabViewItem>())
            {
                if ((TabItem.Content as Frame).Content is TabbedMainPage)
                {
                    if (((TabItem.Content as Frame).Content as TabbedMainPage) != null)
                    {
                        string FileName = ((TabItem.Content as Frame).Content as TabbedMainPage).FileNameTextBlock.Text;
                        TabItem.Header = FileName;
                    }
                }
            }
        }

        public async void Shutdown()
        {
            await ApplicationView.GetForCurrentView().TryConsolidateAsync();
        }

        private void MainPage_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            e.Handled = true;

            if (TabbedView.TabItems.Count > 1)
            {
                CloseWarningBox1.Open();
                CloseWarningBox1.Title = "Ivirius Text Editor Plus";
                CWFContent1.Text = "Are you sure you want to close?";
            }
            if (TabbedView.TabItems.Count == 1)
            {
                SysSender = TabbedView;
                var TabItem = (TabViewItem)SysSender.TabItems[0];
                if ((TabItem.Content as Frame).Content is TabbedMainPage)
                {
                    if (((TabItem.Content as Frame).Content as TabbedMainPage).isWorkSaved == false)
                    {
                        CloseWarningBox2.Open();
                        CloseWarningBox2.Title = "Ivirius Text Editor Plus";
                        CWFContent2.Text = "Do you want to save your file?";
                    }
                    else
                    {
                        TabbedView.TabItems.Clear();
                        Shutdown();
                    }
                }
                else Shutdown();
            }
        }

        public void RemoveCurrentTab(TabView Sender, TabViewTabCloseRequestedEventArgs EvArgs)
        {
            SysArgs = EvArgs;
            SysSender = Sender;

            if ((SysArgs.Tab.Content as Frame).Content is TabbedMainPage)
            {
                if (((SysArgs.Tab.Content as Frame).Content as TabbedMainPage).isWorkSaved == false)
                {
                    CloseWarningBox3.Open();
                    CloseWarningBox3.Title = "Ivirius Text Editor Plus";
                    CWFContent3.Text = "Do you want to save your file?";
                }
                else
                {
                    _ = SysSender.TabItems.Remove(SysArgs.Tab);
                    if (TabbedView.TabItems.Count == 0) Shutdown();
                }
            }
            else
            {
                _ = SysSender.TabItems.Remove(SysArgs.Tab);
                if (TabbedView.TabItems.Count == 0) Shutdown();
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
                try
                {
                    //Write file properties
                    var RF = new Frame();
                    _ = RF.Navigate(typeof(TabbedMainPage), Args);

                    var NewTabItem = new TabViewItem
                    {
                        IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document },
                        Content = RF,
                        Header = "New Tab"
                    };
                    TabbedView.TabItems.Add(NewTabItem);
                }
                catch
                {

                }
            }
        }

        public void BeginFullHelpButtonsForNewTab()
        {
            var RF = new Frame();
            _ = RF.Navigate(typeof(TabbedMainPage));

            var NewTabItem = new TabViewItem
            {
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document },
                Content = RF,
                Header = "New Tab"
            };

            TabbedView.TabItems.Add(NewTabItem);
            TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;

            var X = RF.Content as TabbedMainPage;
            X.BeginFullHelpButtons();
        }

        public void LaunchSettingsTab()
        {
            var RF = new Frame();
            _ = RF.Navigate(typeof(SettingsPage));

            var NewTabItem = new TabViewItem
            {
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Setting },
                Content = RF,
                Header = "Settings"
            };

            TabbedView.TabItems.Add(NewTabItem);
            TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;
        }

        public void BeginFullHelpForNewTab()
        {
            var RF = new Frame();
            _ = RF.Navigate(typeof(TabbedMainPage));

            var NewTabItem = new TabViewItem
            {
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document },
                Content = RF,
                Header = "New Tab"
            };

            TabbedView.TabItems.Add(NewTabItem);
            TabbedView.UpdateLayout();
            TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;

            var X = RF.Content as TabbedMainPage;
            X.BeginFullHelp();
        }

        public void BeginHandwritingForNewTab()
        {
            var RF = new Frame();
            _ = RF.Navigate(typeof(TabbedMainPage));

            var NewTabItem = new TabViewItem
            {
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document },
                Content = RF,
                Header = "New Tab"
            };

            TabbedView.TabItems.Add(NewTabItem);
            TabbedView.UpdateLayout();
            TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;

            var X = RF.Content as TabbedMainPage;
            X.OpenHW();
        }

        public void AddTabForFile(FileActivatedEventArgs EvArgs)
        {
            try
            {
                var RF = new Frame();
                _ = RF.Navigate(typeof(TabbedMainPage), EvArgs);

                var NewTabItem = new TabViewItem
                {
                    IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document },
                    Content = RF,
                    Header = "New Tab"
                };

                TabbedView.TabItems.Add(NewTabItem);
                TabbedView.UpdateLayout();
                TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;
            }
            catch
            {

            }
        }

        public async Task AddTabForFile(StorageFile File)
        {
            try
            {
                var RF = new Frame();
                _ = RF.Navigate(typeof(TabbedMainPage));

                var NewTabItem = new TabViewItem
                {
                    IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document },
                    Content = RF,
                    Header = "New Tab"
                };

                TabbedView.TabItems.Add(NewTabItem);
                TabbedView.UpdateLayout();
                TabbedView.SelectedIndex = TabbedView.TabItems.Count - 1;

                await (RF.Content as TabbedMainPage).ReadFile(File);
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
            var RF = new Frame();
            _ = RF.Navigate(typeof(TabbedMainPage));

            var NewTabItem = new TabViewItem
            {
                Header = "New Tab",
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document },
                Content = RF
            };

            TabbedView.UpdateLayout();
            TabbedView.SelectedIndex = TabbedView.TabItems.Count;

            return NewTabItem;
        }

        private void TabbedView_Loaded(object sender, RoutedEventArgs e)
        {
            var S = sender as TabView;
            if (S.TabItems.Count == 0) S.TabItems.Add(CreateNewTab());
        }

        TabView SysSender;

        TabViewTabCloseRequestedEventArgs SysArgs;

        private void TabbedView_TabCloseRequested(TabView Sender, TabViewTabCloseRequestedEventArgs EvArgs)
        {
            RemoveCurrentTab(Sender, EvArgs);
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            _ = await Launcher.LaunchUriAsync(new Uri("https://ivirius.webnode.page/experimental-options-terms-and-conditions/"));
        }

        private void TabbedView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabbedView.AllowDropTabs = true;
            _ = TabbedView.Focus(FocusState.Pointer);
            TabbedView.IsEnabled = true;
        }

        async void XClick(object sender2, RoutedEventArgs e2)
        {
            CWFBtn4.Visibility = Visibility.Collapsed;
            await (((TabbedView.SelectedItem as TabViewItem).Content as Frame).Content as TabbedMainPage).SaveFile(false, false, false, false);
            CloseWarningFlyoutHost.Flyout.Hide();
            CWFBtn1.Click -= XClick;
        }

        async void XClick2(object sender2, RoutedEventArgs e2)
        {
            CloseWarningFlyoutHost.Flyout.Hide();
            CWFBtn4.Visibility = Visibility.Collapsed;
            CWFBtn2.Click -= XClick2;
            _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
        }

        void XClick3(object sender2, RoutedEventArgs e2)
        {
            CloseWarningFlyoutHost.Flyout.Hide();
            CWFBtn4.Visibility = Visibility.Collapsed;
            CWFBtn3.Click -= XClick3;
        }

        async void XClick4(object sender2, RoutedEventArgs e2)
        {
            CloseWarningFlyoutHost.Flyout.Hide();
            CWFBtn4.Visibility = Visibility.Collapsed;
            CWFBtn4.Click -= XClick4;
            _ = await ApplicationView.GetForCurrentView().TryConsolidateAsync();
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            bool FS = ApplicationView.GetForCurrentView().IsFullScreenMode;
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

        private async void Button_Click_188(object sender, RoutedEventArgs e)
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

        private void MainPageComponent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window.Current.SetTitleBar(CustomDragRegion);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplicationDataContainer LS = ApplicationData.Current.LocalSettings;
            if ((string)LS.Values["SetupFinish"] != "Yes")
            {
                var TBView = ApplicationView.GetForCurrentView();
                _ = TBView.TryEnterFullScreenMode();
            }
            else { }
        }
    }
}
