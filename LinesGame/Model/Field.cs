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
        private int rows;
        private int columns;
        private ObservableCollection<Cell> field;

        public Field(int rows, int columns)
        {
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
            this.rows = thatField.rows;
            this.columns = thatField.columns;
            field = new ObservableCollection<Cell>();
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    field.Add(new Cell()
                    {
                        CellColor = thatField[i * columns + j].CellColor,
                        //ContainBall = thatField[i * columns + j].ContainBall,
                        Selected = thatField[i * columns + j].Selected,
                        ID = thatField[i * columns + j].ID
                    });
                }
            }

        }

        public Cell this[int index]
        {
            get
            {
                return field[index];
            }
            set
            {
                field[index] = value;
                //if (PropertyChanged != null)
                //{
                //    MessageBox.Show("cell changed");
                //    PropertyChanged(this, new PropertyChangedEventArgs(Binding.IndexerName));
                //}
                //OnPropertyChanged("Cell[" + i + ", " + j + "]");
            }
        }

        public bool SwapCells(int leftIndex, int rightIndex)
        {
            int i1 = leftIndex / columns;
            int j1 = leftIndex - i1 * columns;

            int i2 = rightIndex / columns;
            int j2 = rightIndex - i2 * columns;

            if (Math.Abs(i1 - i2) > 1 || Math.Abs(j1 - j2) > 1 || (Math.Abs(i1 - i2) == 1 && Math.Abs(j1 - j2) == 1)) return false;
            // Если цвет шаров одинаков - нет смысла менять местами
            if (field[leftIndex].CellColor == field[rightIndex].CellColor)
            {
                return false;
            }

            //bool ContainBallTemp = field[leftIndex].ContainBall;
            //bool SelectedTemp = field[leftIndex].Selected;
            Color CellColorTemp = field[leftIndex].CellColor;

            //field[leftIndex].ContainBall = field[rightIndex].ContainBall;
            //field[leftIndex].Selected = field[rightIndex].Selected;
            field[leftIndex].CellColor = field[rightIndex].CellColor;

            //field[rightIndex].ContainBall = ContainBallTemp;
            //field[rightIndex].Selected = SelectedTemp;
            field[rightIndex].CellColor = CellColorTemp;

            field[leftIndex].Selected = false;
            field[rightIndex].Selected = false;


            return true;
        }

        public void ToggleCell(int i, int j)
        {
            field[i * columns + j].Toggle();
            //OnPropertyChanged("toggle Cell[" + i + ", " + j + "]");
        }

        public int Rows
        {
            get { return rows; }
        }

        public int Columns
        {
            get { return columns; }
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
