using System;
using System.Collections.Generic;
using System.Windows;

namespace OdraIDE.Core
{
    public interface ILayoutItem : IExtension
    {
        string Name { get; }
        string Title { get; }

        void OnGotFocus(object sender, RoutedEventArgs e);
        void OnLostFocus(object sender, RoutedEventArgs e);
    }
}
