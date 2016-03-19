using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LinesGame.Model
{
    public class Cell : INotifyPropertyChanged
    {
        private Color cellcolor;
        private bool selected;

        public int ID { get; set; }

        public Cell()
        {
            CellColor = Colors.Transparent;
            Selected = false; 
        }

        public Color CellColor
        {
            get { return cellcolor; }
            set
            {
                cellcolor = value;
                OnPropertyChanged("CellColor");
            }
        }

        public bool Selected 
        { 
            get { return selected; }
            set 
            {
                selected = value;
                OnPropertyChanged("Selected");
            } 
        }
        // Сменить фокус (был фокус - снимаем, не было фокуса - выставляем фокус)
        public void Toggle()
        {
            Selected = !Selected;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    

}
