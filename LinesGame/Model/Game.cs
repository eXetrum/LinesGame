using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace LinesGame.Model
{
    public class Game
    {
        private Field field;
        private FieldSetup fieldsetup;
        private GameScore score;
        private bool running;

        public Game(FieldSetup fieldsetup) {
            this.fieldsetup = fieldsetup;
            switch (fieldsetup.FieldType)
            {
                case Utils.FieldType.Field10x10:
                    field = new Field(10, 10);
                    break;
                case Utils.FieldType.Field17x19:
                    field = new Field(17, 19);
                    break;
                case Utils.FieldType.Field20x20:
                    field = new Field(20, 20);
                    break;
                default:
                    field = new Field(10, 10);
                    break;
            }

            score = new Model.GameScore();




            StartGame();
        }

        public Field Field
        {
            get { return field; }
            set { field = value; }
        }

        public FieldSetup FieldSetup
        {
            get { return fieldsetup; }
            set { fieldsetup = value; }
        }

        public GameScore Score
        {
            get { return score; }
            set { score = value; }
        }

        public bool Running
        {
            get
            {
                return running;
            }
            set
            {
                running = value;
            }
        }

        public void StartGame()
        {
            // Готовим поле к началу
            do
            {
                PrepareField();
            } while (!HasMoves()); // Допустимые ходы должны быть
            // Обнуляем очки
            score.Reset();
            Running = true;
        }

        public void StopGame()
        {
            MessageBox.Show("ALLAH AKBAR");
            Score.StopRecord();
            Running = false;
        }

        public void PauseGame()
        {
            Score.StopRecord();
            Running = false;
        }

        public void ResumeGame()
        {
            Score.ResumeRecord();
            Running = true;
        }


        public bool GravityRequire()
        {
            for (int j = 0; j < field.Columns; ++j)
            {
                int empty = -1;
                int ball = -1;
                for (int i = 0; i < field.Rows; ++i)
                {
                    if (empty == -1 && field[i * field.Columns + j].CellColor.Equals(Colors.Transparent))
                    {
                        empty = i;
                    }
                    if (ball == -1 && !field[i * field.Columns + j].CellColor.Equals(Colors.Transparent))
                    {
                        ball = i;
                    }
                    if (empty != -1 && ball != -1) break;
                }
                //Console.WriteLine("Column=" + j + ", empty=" + empty + ", ball=" + ball);
                // Если встретили при спуске по строке сначала шар а затем пустоую клетку - нужно включить гравитацию чтобы шары  упали вниз
                if (ball < empty) return true;
            }
            return false;
        }

        public bool FillEmptyRequire()
        {
            for (int j = 0; j < field.Columns; ++j)
            {
                for (int i = 0; i < field.Rows; ++i)
                {
                    if (!field[i * field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;
                    return true;
                }
            }
            return false;
        }

        public void ChekField() 
        {
            


            List<Path> lines = GetLines();
            bool fieldComplete = lines.Count == 0;
            while (!fieldComplete)
            {
                foreach (var p in lines)
                {
                    Console.WriteLine(p);
                    RemoveLine(p);
                    try
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\test\lines\test_splash.wav");
                        player.Play();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception while sound play: " + ex.Message);
                    }
                }
                // Опускаем все шары вниз (пустые клетки перемещаем вверх)
                //MessageBox.Show("Fall down");
                //BallsGravityDown();
                //Thread.Sleep(1000);
                // Заполняем пустые клетки новыми шарами
                //MessageBox.Show("Fill empty");
                //FillEmptyCells();
                //Thread.Sleep(1000);
                // Получаем комбинации линий
                lines = GetLines();
                // Если комбинаций нет - подготовка окончена
                fieldComplete = lines.Count == 0;
            }
            if (!HasMoves())
            {
                MessageBox.Show("GAME OVER");
                Score.StopRecord();
            }
        }

        protected void PrepareField()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < field.Rows; ++i)
            {
                for (int j = 0; j < field.Columns; ++j)
                {
                    field[i * field.Columns + j].CellColor = Utils.BallColors[rnd.Next(fieldsetup.BallCount)];
                    //field[i * field.Columns + j].ContainBall = true;
                }
            }

            List<Path> lines = GetLines();
            bool fieldComplete = lines.Count == 0;
            while (!fieldComplete)
            {
                foreach (var p in lines)
                {
                    Console.WriteLine(p);
                    //RemoveLine(p);                    
                }
                //MessageBox.Show("next");
                foreach (var p in lines)
                {
                    RemoveLine(p);                    
                }
                // Опускаем все шары вниз (пустые клетки перемещаем вверх)
                //MessageBox.Show("Fall down");
                BallsGravityDown();                
                // Заполняем пустые клетки новыми шарами
                //MessageBox.Show("Fill empty");
                FillEmptyCells();                
                // Получаем комбинации линий
                lines = GetLines();
                // Если комбинаций нет - подготовка окончена
                fieldComplete = lines.Count == 0;
            }
        }
        // Заполняем пустые клетки шарами
        public void FillEmptyCells()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < field.Rows; ++i)
            {
                for (int j = 0; j < field.Columns; ++j)
                {
                    // Если клетка не пустая - пропускаем и переходим к след.
                    if (!field[i * field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;
                    field[i * field.Columns + j].CellColor = Utils.BallColors[rnd.Next(fieldsetup.BallCount)];
                }
            }
        }
        // Шары падают вниз заполняя пусты клетки под собой
        public void BallsGravityDown()
        {
            // Сдвигаем все шары вниз чтоьбы пустые клетки остались на самом верху
            for (int j = 0; j < field.Columns; ++j) 
            {
                int lastEmpty = 0;
                for (int i = field.Rows - 1; i >= lastEmpty; --i)
                {
                    // found empty
                    if (field[i * field.Columns + j].CellColor.Equals(Colors.Transparent))
                    {
                        // moove up
                        for (int k = i - 1; k >= lastEmpty; --k)
                        {
                            field.SwapCells(k * field.Columns + j, (k + 1) * field.Columns + j);
                        }
                        ++lastEmpty;
                        i++;
                    }   
                }
            }
        }
        // Убрать линию из шаров с поля
        void RemoveLine(Path path)
        {
            switch(path.GetPath().Count) 
            {
                case 3:
                    score.Score += 300;
                    break;
                case 4:
                    score.Score += 600;
                    break;
                case 5:
                    score.Score += 1000;
                    break;
                default:
                    score.Score += path.GetPath().Count * 250;
                    break;
            }

            foreach(var p in path.GetPath()) 
            {
                field[p.X * field.Columns + p.Y].CellColor = Colors.Transparent;
            }
        }

        // Есть ли допустимые ходы
        public bool HasMoves()
        {
            // Создаем копию поля. Потому что оригинальный объект привязан к интерфейсу и любые изменения будут отображены, а нам этого не нужно
            Model.Field copyField = new Field(Field);
            // Пробегаем по всем ячейкам поля и пробуем сделать обмен между
            // ячейками так чтобы получилась линия, если при каком либо обмене на поле получается линия - значит
            // ходы существуют
            for (int i = 0; i < copyField.Rows; ++i)
            {
                for (int j = 0; j < copyField.Columns; ++j)
                {
                    /////////////////////////////////////////////////
                    // Обмен вниз
                    if (i + 1 < copyField.Rows)
                    {
                        copyField.SwapCells(i * copyField.Columns + j, (i + 1) * copyField.Columns + j);
                        // Проверяем возможно ли построить линии на новом поле
                        if (CanConstructLines(copyField)) return true;
                        // Если нельзя - отменяем последний обмен
                        copyField.SwapCells(i * copyField.Columns + j, (i + 1) * copyField.Columns + j);
                    }
                    /////////////////////////////////////////////////
                    // Обмен вправо
                    if (j + 1 < copyField.Columns)
                    {
                        copyField.SwapCells(i * copyField.Columns + j, i * copyField.Columns + (j + 1));
                        // Проверяем возможно ли построить линии на новом поле
                        if (CanConstructLines(copyField)) return true;
                        // Если нельзя - отменяем последний обмен
                        copyField.SwapCells(i * copyField.Columns + j, i * copyField.Columns + (j + 1));
                    }                    
                }
            }
            // Все обмены проверили и ничего не получилось - вывод ходов более нет
            return false;
        }
        // Проверка построения хотябы одной линии на поле
        protected bool CanConstructLines(Field _field)
        {
            for (int i = 0; i < _field.Rows; ++i)
            {
                for (int j = 0; j < _field.Columns; ++j)
                {
                    // Если нет шара в ячейке - не строим путь а переходим к след. ячейке
                    if (_field[i * _field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;
                    Color color = _field[i * _field.Columns + j].CellColor;
                    Path horizontal = new Path(color);
                    horizontal.Add(new Point(i, j));
                    // Если справа есть еще шары и цвет совпадает с текущим - пробуем построить путь
                    while (j + 1 < _field.Columns && color.Equals(_field[i * _field.Columns + (j + 1)].CellColor))
                    {
                        ++j;
                        horizontal.Add(new Point(i, j));
                    }
                    if (horizontal.GetPath().Count >= 3) { return true; }
                }
            }
            // Vertical lines
            for (int j = 0; j < _field.Columns; ++j)
            {
                for (int i = 0; i < _field.Rows; ++i)
                {
                    if (_field[i * _field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;
                    Color color = _field[i * _field.Columns + j].CellColor;
                    Path vertical = new Path(color);
                    vertical.Add(new Point(i, j));
                    // Если снизу есть еще шары и цвет совпадает с текущим - пробуем построить путь
                    while (i + 1 < _field.Rows && color.Equals(_field[(i + 1) * _field.Columns + j].CellColor))
                    {
                        ++i;
                        vertical.Add(new Point(i, j));
                    }
                    if (vertical.GetPath().Count >= 3) { return true; }
                }
            }

            return false;
        }

        public List<Path> GetLines()
        {
            List<Path> lines = new List<Path>();

            for (int i = 0; i < field.Rows; ++i)
            {
                for (int j = 0; j < field.Columns; ++j)
                {
                    // Если нет шара в ячейке - не строим путь а переходим к след. ячейке
                    if (field[i * field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;

                    Color color = field[i * field.Columns + j].CellColor;
                    Path horizontal = new Path(color);
                    horizontal.Add(new Point(i, j));
                    // Если справа есть еще шары и цвет совпадает с текущим - пробуем построить путь
                    while (j + 1 < field.Columns && color.Equals(field[i * field.Columns + (j + 1)].CellColor))
                    {
                        ++j;
                        horizontal.Add(new Point(i, j));
                    }
                    if (horizontal.GetPath().Count >= 3)
                    {
                        if (!lines.Contains(horizontal))
                        {
                            lines.Add(horizontal);
                        }                            
                    }
                }
            }
            // Vertical lines
            for (int j = 0; j < field.Columns; ++j)
            {
                for (int i = 0; i < field.Rows; ++i)
                {
                    if (field[i * field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;
                    Color color = field[i * field.Columns + j].CellColor;
                    Path vertical = new Path(color);
                    vertical.Add(new Point(i, j));
                    // Если снизу есть еще шары и цвет совпадает с текущим - пробуем построить путь
                    while (i + 1 < field.Rows && color.Equals(field[(i + 1) * field.Columns + j].CellColor))
                    {
                        ++i;
                        vertical.Add(new Point(i, j));
                    }
                    if (vertical.GetPath().Count >= 3)
                    {
                        if (!lines.Contains(vertical))
                        {
                            lines.Add(vertical);
                        }
                    }
                }
            }
            return lines;
        }

        public class Point {
            private int x, y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Point(Point other)
            {
                this.x = other.x;
                this.y = other.y;
            }

            public int X
            {
                get { return x; }
                set { x = value; }
            }
            
            public int Y
            {
                get { return y; }
                set { y = value; }
            }

            public override string ToString()
            {
                string s = "{" + X + ", " + Y + "}";
                return s;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Point objAsPoint = obj as Point;
                if (objAsPoint == null) return false;
                else return Equals(objAsPoint);
            }
            
            public override int GetHashCode()
            {
                return x ^ y;
            }
            
            public bool Equals(Point other)
            {
                if (other == null) return false;
                return this.x == other.x && this.y == other.y;
            }
        }

        public class Path
        {
            List<Point> path;
            Color color;
            
            public Path(Color color) 
            {
                this.color = color;
                path = new List<Point>();
            }

            public Path(Path other)
            {
                color = other.color;
                path = new List<Point>();
                path.AddRange(other.path);

            }

            public void Add(Point p) 
            {
                path.Add(new Point(p));
            }

            public void RemoveLast() 
            {
                path.RemoveAt(path.Count - 1);
            }

            public List<Point> GetPath() { return path; }

            public override int GetHashCode()
            {
                int hash = 1;
                foreach (var p in path)
                {
                    hash ^= p.GetHashCode();
                }
                return hash;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Path objAsPath = obj as Path;
                if (objAsPath == null) return false;
                else return Equals(objAsPath);
            }

            public bool Equals(Path p)
            {
                if (p == null) return false;
                if (!color.Equals(p.color)) return false;

                foreach (var pp in path)
                {
                    if (!p.path.Contains(pp)) return false;
                }
                return true;
            }
            public override string ToString()
            {
                string s = string.Empty;
                foreach (var p in path)
                {
                    s += p;
                }
                return s;
            }
        }
    }
    
}
