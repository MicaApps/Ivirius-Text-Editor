using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Ivirius_Text_Editor
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            RequestedTheme = ElementTheme.Light;
        }
    }
}