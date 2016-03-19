using LinesGame.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinesGame.Convertors
{
    public class BoolToLabel : ConvertorBase<BoolToLabel>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("BOOL TO LABEL");
            if (value is bool)
            {
                bool runing = (bool)value;
                return runing ? "Пауза" : "Возобновить";
            }
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string label = (string)value;
                return label.Equals("Пауза");
            }
            return "unknow";
        }
    }
}
