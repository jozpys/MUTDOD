using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OdraIDE.QueryPlan
{
    public class LevelConverter : DependencyObject, IMultiValueConverter
    {

        public object Convert(
            object[] values, Type targetType,
            object parameter, CultureInfo culture)
        {
            Console.WriteLine("HUJ");
            Console.WriteLine(values[0]);
            Console.WriteLine(values[1]);
            double level;
            if (values[0] == DependencyProperty.UnsetValue)
            {
                level = 1;
            }
            else
            {
                level = (double)values[0];
            }
            double indent = (double)values[1];
            return indent * level;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
