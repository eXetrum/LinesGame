using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LinesGame.Model;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace LinesGame.Convertors
{
    public class ContainBallToColor : ConvertorBase<ContainBallToColor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is Cell)
            //{
            //    Cell cell = (Cell)value;
            //    if (cell.ContainBall)
            //    {
            //        return new SolidColorBrush(cell.CellColor);
            //    }
            //    else
            //    {
            //        return new SolidColorBrush(Colors.Transparent);
            //    }
            //}
            //Color c;
            Color color = (Color)value;
            if (value is Color)
            {
                //Color color = (Color)value;
                SolidColorBrush brush = new SolidColorBrush(color);



                var start = new GradientStop();
                start.Offset = 0;
                start.Color = Colors.Azure;

                var stop = new GradientStop();
                stop.Offset = 1;
                stop.Color = color;

                var result = new RadialGradientBrush();
                result.GradientOrigin = new Point(0.20, 0.5);
                result.Center = new Point(0.25, 0.5);
                result.RadiusX = 0.75;
                result.RadiusY = 0.5;
                result.GradientStops = new GradientStopCollection();
                result.GradientStops.Add(start);
                result.GradientStops.Add(stop);

                //return brush;
                return result;
            }
            //return new SolidColorBrush(color);// value;
            return value;
            // date.ToShortDateString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
