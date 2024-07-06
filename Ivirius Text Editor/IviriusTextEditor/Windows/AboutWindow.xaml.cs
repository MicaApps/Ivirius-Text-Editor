using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;
using Ivirius_Text_Editor;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Ivirius_Text_Editor.IviriusTextEditor.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            this.InitializeComponent();
            this.SetWindowSize(400, 520);
            this.SetIsResizable(false);
            this.SetIsMaximizable(false);
            this.SetIsMinimizable(false);
            this.AppWindow.SetIcon("Assets/icon.ico");
            this.AppWindow.Title = $"About {App.Current.AppName}";
            SystemBackdrop = new MicaBackdrop()
            {
                Kind = MicaKind.BaseAlt
            };
            version.Text = $"Version {App.Current.AppVersion} - Full release";
            AppNameTextBlock.Text = $"{App.Current.AppName}";
            CompileDate.Text = "Compilation date " + GetBuildDate(Assembly.GetExecutingAssembly());
        }

        private void HyperlinkButton_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_30(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private static DateTime GetBuildDate(Assembly assembly)
        {
            //var attribute = assembly.GetCustomAttribute<BuildDateAttribute>();
            //return attribute != null ? attribute.DateTime : default(DateTime);
            return DateTime.Now;
        }
    }
}
