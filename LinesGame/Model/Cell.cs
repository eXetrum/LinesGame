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
        public int ID { get; set; }
        // Цвет ячейки
        private Color cellcolor;
        // Имеет выделение или нет
        private bool selected;
        // Конструктор по умолчанию
        public Cell()
        {
            // По умолчанию цвет клетки пустой (нет шара)
            CellColor = Colors.Transparent;
            // Выделения на клетке так же нет
            Selected = false; 
        }
        // Доступ к полю цвета
        public Color CellColor
        {
            get { return cellcolor; }
            set
            {
                cellcolor = value;
                // При изменении поля объекта нужно сообщить об этом используя INotifyPropertyChanged интерфейс
                OnPropertyChanged("CellColor");
            }
        }
        // Доступ к полю выделения
        public bool Selected 
        { 
            get { return selected; }
            set 
            {
                selected = value;
                // Сообщаем о изменении объекта
                OnPropertyChanged("Selected");
            } 
        }
        // Реализация интерфейса INotifyPropertyChanged 
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // Событие изменения состояня объекта
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
