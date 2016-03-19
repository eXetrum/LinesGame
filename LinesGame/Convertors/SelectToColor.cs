using LinesGame.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LinesGame.Convertors
{
    public class SelectToColor : ConvertorBase<SelectToColor>
    {
        public SelectToColor() { }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //DateTime date = (DateTime)value;
            //MessageBox.Show("Coverter: value=" + value.ToString() + ", parametr=" + parameter);
            //Color c;

            //Field field = (Field)value;
            //int index = int.Parse((string)parameter);

            //if (field[index].Selected)
            //{
            //    MessageBox.Show("true");
            //    return new SolidColorBrush(Colors.Red);
            //}
            //else
            //{
            //    MessageBox.Show("false");
            //    return new SolidColorBrush(Colors.Black);
            //}



            if (value is bool)
            {
                bool selected = (bool)value;
                if (selected)
                {
                    //MessageBox.Show("true");
                    return new SolidColorBrush(Colors.Silver);//Colors.SaddleBrown);
                }
                else
                {
                    //MessageBox.Show("false");
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
            return value;// date.ToShortDateString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
