using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Ivirius.UI.Windowing
{
    public sealed class ChildAppWindowContainer : Grid
    {
        public ChildAppWindowContainer()
        {
            foreach (UIElement child in Children)
            {
                if (!(child is DialogWindow || child is ChildAppWindow))
                {
                    throw new ArgumentException("Only ChildAppWindow and DialogWindow elements are allowed as content in the ChildAppWindowHost.");
                }
            }
        }
    }
}
