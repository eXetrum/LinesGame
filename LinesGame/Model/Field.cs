using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LinesGame.Model
{
    public class Field : INotifyPropertyChanged
    {
        // Количество строк
        private int rows;
        // Количество столбцов
        private int columns;
        // Коллекция ячеек (описываем поле в виде списка)
        private ObservableCollection<Cell> field;
        // Конструктор объекта поле
        public Field(int rows, int columns)
        {
            // Запоминаем количество строк/столбцов
            this.rows = rows;
            this.columns = columns;
            // Ячейки складываем в коллекцию которая генерирует событие PropertyChanged при изменении объектов внутри себя
            field = new ObservableCollection<Cell>();
            // Заполняем поле новыми ячейками
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    field.Add( new Cell() { ID = i * columns + j } );
                }
            }
        }
        // Построить поле на основе другого поля
        public Field(Field thatField)
        {
            // Скопировали значения сторок/столбцов
            this.rows = thatField.rows;
            this.columns = thatField.columns;
            // Создали пустую коллекцию ячеек
            field = new ObservableCollection<Cell>();
            // Копируем ячейки
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    field.Add(new Cell()
                    {
                        CellColor = thatField[i * columns + j].CellColor,
                        Selected = thatField[i * columns + j].Selected,
                        ID = thatField[i * columns + j].ID
                    });
                }
            }

        }
        // Доступ к ячейке по индексу Field[]
        public Cell this[int index]
        {
            get
            {
                return field[index];
            }
            set
            {
                field[index] = value;
            }
        }
        // Обмен ячеек местами
        public bool SwapCells(int leftIndex, int rightIndex)
        {
            int i1 = leftIndex / columns;
            int j1 = leftIndex - i1 * columns;

            int i2 = rightIndex / columns;
            int j2 = rightIndex - i2 * columns;
            // Проверим чтобы переещение было возможно (только соседние сверху/снизу и слева/справа)
            if (Math.Abs(i1 - i2) > 1 || Math.Abs(j1 - j2) > 1 || (Math.Abs(i1 - i2) == 1 && Math.Abs(j1 - j2) == 1)) return false;
            // Если цвет шаров одинаков - нет смысла менять местами
            if (field[leftIndex].CellColor == field[rightIndex].CellColor)
            {
                return false;
            }
            // Меняем местами (нужно поменять местами только цвет ячейки)
            Color CellColorTemp = field[leftIndex].CellColor;
            field[leftIndex].CellColor = field[rightIndex].CellColor;
            field[rightIndex].CellColor = CellColorTemp;
            // Снимаем выделение с обеих ячеек
            field[leftIndex].Selected = false;
            field[rightIndex].Selected = false;
            // Обмен успешен
            return true;
        }
        // Доступ к полю количества строк
        public int Rows
        {
            get { return rows; }
        }
        // Доступ к полю количества столбцов
        public int Columns
        {
            get { return columns; }
        }
        // Реализация интерфейса INotifyPropertyChanged
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
