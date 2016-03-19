using LinesGame.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinesGame.Convertors
{
    public class SoundEnumToBool : ConvertorBase<SoundEnumToBool>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("Convert sound enum ");
            if (value is Model.Utils.SoundSettings)
            {
                Model.Utils.SoundSettings sound = (Model.Utils.SoundSettings)value;
                return sound == Utils.SoundSettings.Enabled;
            }
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("ConvertBack sound enum ");
            if (value is bool)
            {
                return (value is bool && (bool)value) ? Utils.SoundSettings.Enabled : Utils.SoundSettings.Disabled;
            }
            return value;
        }
    }
}
