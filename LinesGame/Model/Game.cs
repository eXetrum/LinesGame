using LinesGame.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Media;
using System.Windows.Threading;

namespace LinesGame.Model
{
    public class Game : INotifyPropertyChanged
    {
        // Опишем делегат вызываемый по завершению игры
        public delegate void OnGameOver();
        // Создаем событие 
        public event OnGameOver OnOver;

        private const long gameMaxTime = 5 * 60;    // Максимальное время при режиме игры на время в секундах
        private const int gameMaxMoves = 100;       // Максимальное количество ходов при режиме игры на количество ходов
        // Игровой таймер
        private DispatcherTimer timer;
        // Осталось ходов/времени (игра на время / игра на количество ходов)
        private long remain;
        // Перечисление статусов игры
        public enum GameStatus
        {
            PrepareOK,      // Игра готова к запуску
            Running,        // Игра создана и запущена
            Paused,         // Игра приостановлена
            End             // Игра завершена
        }
        // Игровое поле
        private Field field;
        // Очки, время игры, ходы и т.д.
        private GameScore gamescore;
        // Текущий статус игры
        private GameStatus gamestatus;
        // Список линий полученных за один ход (серия линий)
        private List<Line> moveCombo = new List<Line>();
        // Конструктор
        public Game() {
            // Создаем объект хранящий и собирающий статистику
            gamescore = new Model.GameScore(Settings.Default.GameType, Settings.Default.Field);
            // Создаем игровой таймер
            timer = new DispatcherTimer();
            // Интервал в секунду
            timer.Interval = TimeSpan.FromSeconds(1);
            // Задаем обработчик при достижении очередного интервала
            timer.Tick += timer_Tick;
            // Если игра на время
            if (gamescore.GameType == Utils.GameType.LimitedTime)
            {
                // Задаем количество игровых секунд
                remain = gameMaxTime;
            }
            else // Игра на количество ходов
            {
                remain = gameMaxMoves;
            }
            // На основе данных из настроек создаем поле соотв. размера
            switch (Settings.Default.Field)
            {
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
            // Готовим поле к старту
            PrepareGame();
        }
        // Обработчик тиков таймера
        void timer_Tick(object sender, EventArgs e)
        {
            // Увеличиваем счетчик прошедших секунд
            ElapsedTime = TimeSpan.FromSeconds(ElapsedTime.Seconds + 1);
            // Отнимаем от оставшегося времени если игра на время
            if (_GameScore.GameType == Utils.GameType.LimitedTime)
            {
                --Remain;
            }
            // Отладочное сообщение в консоль
            Console.WriteLine("elapsed=" + ElapsedTime + ", remain=" + remain);
            // Достигли конца счетчика
            if (remain == 0)                    
            {
                StopGame(); // Останавливаем игру  
            }
        }
        // Осталось ходов/времени
        public long Remain
        {
            get { return remain; }
            set
            {
                remain = value;
                OnPropertyChanged("Remain");
                OnPropertyChanged("Remained");

            }
        }
        // Оставшееся время/количество ходов
        public string Remained
        {
            get
            {
                if (_GameScore.GameType == Utils.GameType.LimitedTime)
                {
                    return TimeSpan.FromSeconds(remain).ToString(@"hh\:mm\:ss");
                }
                else
                {
                    return remain.ToString();
                }
            }
            set { }
        }
        // Прошло времени
        public TimeSpan ElapsedTime
        {
            get { return TimeSpan.FromSeconds(_GameScore.ElapsedSeconds); }
            set
            {
                _GameScore.ElapsedSeconds = value.Seconds;
                OnPropertyChanged("ElapsedTime");
            }
        }
        // Доступ к Полю
        public Field Field
        {
            get { return field; }
            set 
            { 
                field = value;
                OnPropertyChanged("Field");
            }
        }
        // Доступ к статистике
        public GameScore _GameScore
        {
            get { return gamescore; }
            set 
            {
                gamescore = value;
                OnPropertyChanged("_GameScore");
            }
        }
        // Доступ к текущему статусу игры
        public GameStatus _GameStatus
        {
            get
            {
                return gamestatus;
            }
            set
            {
                gamestatus = value;
                OnPropertyChanged("_GameStatus");
            }
        }

        public void ClearCombo()
        {
            moveCombo.Clear();
        }
        // Метод подготовки игры к старту но еще не старт
        public void PrepareGame()
        {
            moveCombo = new List<Line>();
            // Готовим поле к началу
            do
            {
                PrepareField();
            } while (!HasMoves()); // Допустимые ходы должны быть
            // Статус игры - готова к запуску
            gamestatus = GameStatus.PrepareOK;
        }
        // Метод запуска игры
        public void StartGame()
        {
            // Обнуляем очки
            gamescore.Reset();
            // Запускаем таймер
            timer.Start();
            // Меняем статус игры на "Запущена"
            _GameStatus = GameStatus.Running;
        }
        // Метод остановки игры
        public void StopGame()
        {
            Console.WriteLine("Game Over");
            if (_GameStatus == GameStatus.End) return;
            _GameStatus = GameStatus.End;
            // Останавливаем таймер
            timer.Stop();
            // Если обработчик события задан            
            if (OnOver != null)
            {
                Console.WriteLine("Game OnOver event raised");
                // Вызываем
                OnOver();                
            }
        }
        // Поставить игру на паузу
        public void PauseGame()
        {
            // Останавливаем таймер
            timer.Stop();
            // Меняем статус игры
            _GameStatus = GameStatus.Paused;
        }
        // Снять с паузы и продолжить
        public void ResumeGame()
        {
            // Запускаем таймер
            timer.Start();
            // Меняем статус игры на
            _GameStatus = GameStatus.Running;
        }
        // Проверяем поле на "не упавшие" шары (ситуация в которой пустые клетки под шарами а не на верху поля)
        public bool GravityRequire()
        {
            for (int j = 0; j < field.Columns; ++j)
            {
                int empty = -1;
                int ball = -1;
                for (int i = 0; i < field.Rows; ++i)
                {
                    // Если нашли пустую клетку - запоминаем индекс клетки
                    if (empty == -1 && field[i * field.Columns + j].CellColor.Equals(Colors.Transparent)) empty = i;
                    // Если нашли клетку с шаром - запоминаем индекс клетки
                    if (ball == -1 && !field[i * field.Columns + j].CellColor.Equals(Colors.Transparent)) ball = i;
                    // Если индексы шара и пустой клетки уже определены - дальше продолжать не имеет смысла
                    if (empty != -1 && ball != -1) break;
                }
                //Console.WriteLine("Column=" + j + ", empty=" + empty + ", ball=" + ball);
                // Если встретили при спуске по строке сначала шар а затем пустоую 
                // клетку - нужно включить "гравитацию" чтобы шары упали вниз
                if (ball < empty) return true;
            }
            // В противном случае гравитация не нужна
            return false;
        }
        // Проверка необходимости заполнения пустых клеток (ситуация при которой на поле могут образоваться пустые клетки)
        public bool FillEmptyRequire()
        {
            for (int j = 0; j < field.Columns; ++j)
            {
                for (int i = 0; i < field.Rows; ++i)
                {
                    // Клетка не пустая (на клетке шар) - двигаемся к след ячейке
                    if (!field[i * field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;
                    // Иначе нужно заполнить пустоты
                    return true;
                }
            }
            return false;
        }
        // Проверка поля на образование новых комбинаций линий после очередного хода (возможно была применена гравитация и образовались новые линии)
        public void CheckField() 
        {
            moveCombo.Clear();
            // Получаем список линий
            List<Line> lines = GetLines();
            // Если список пуст
            bool fieldComplete = lines.Count == 0;
            // Если список не пуст
            while (!fieldComplete)
            {               
                // Убираем все линии с поля
                foreach (var p in lines)
                {
                    if (!moveCombo.Contains(p)) moveCombo.Add(p);
                    // Выводим в консоль отладочную запись
                    Console.WriteLine(p);
                    // Убираем линию
                    RemoveLine(p);
                    // Если задано испольование звуков
                    if(Settings.Default.Sound == Utils.SoundSettings.Enabled) 
                    {
                        // Проигрываем звук исчезновения линии
                        try
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Sound\disappear.wav");
                            player.Play();
                        }
                        catch (Exception ex) { Console.WriteLine("Exception while sound play: " + ex.Message); }
                    }
                }
                //if (Score.MaxCombo < curcombat) Score.MaxCombo = curcombat;
                if (_GameScore.MaxCombo < moveCombo.Count) _GameScore.MaxCombo = moveCombo.Count;
                // Получаем комбинации линий снова
                lines = GetLines();
                // Если комбинаций нет - подготовка окончена
                fieldComplete = lines.Count == 0;
            }
            // Если нет ходов - игра окончена
            if (!HasMoves())
            {
                Console.WriteLine("NO MORE MOVES, GAME OVER");
                StopGame();
            }
        }
        // Стартовая подготовка поля
        protected void PrepareField()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < field.Rows; ++i)
            {
                for (int j = 0; j < field.Columns; ++j)
                {
                    // Выставляем на поле очередной шар случайного цвета (количество цветов берем из настроек)
                    field[i * field.Columns + j].CellColor = Utils.BallColors[rnd.Next(Settings.Default.ColorCount)];
                }
            }
            // Получаем линии на поле (если при старте есть линии - убераем их до тех пор пока поле не будет содержать линий со старта)
            List<Line> lines = GetLines();
            bool fieldComplete = lines.Count == 0;
            while (!fieldComplete)
            {
                foreach (var p in lines)
                {
                    Console.WriteLine(p);
                    //RemoveLine(p);
                    // Обнуляем цвета на соответствующих линии клетках
                    foreach (var pp in p.GetLine())
                    {
                        field[pp.X * field.Columns + pp.Y].CellColor = Colors.Transparent;
                    }
                }
                // Опускаем все шары вниз (пустые клетки перемещаем вверх)
                Console.WriteLine("Fall down");
                BallsGravityDown();                
                // Заполняем пустые клетки новыми шарами
                Console.WriteLine("Fill empty");
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
                    field[i * field.Columns + j].CellColor = Utils.BallColors[rnd.Next(Settings.Default.ColorCount)];
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
        void RemoveLine(Line path)
        {
            if (_GameStatus == GameStatus.End) return;
            // На основе длинны линии рассчитываем прибавку к очкам
            switch(path.GetLine().Count) 
            {
                case 3:
                    gamescore.Score += 300;
                    break;
                case 4:
                    gamescore.Score += 600;
                    break;
                case 5:
                    gamescore.Score += 1000;
                    break;
                default: // Для длин линий более 5 формула такая
                    gamescore.Score += path.GetLine().Count * 250;
                    break;
            }
            // Обнуляем цвета на соответствующих линии клетках
            foreach(var p in path.GetLine()) 
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
                    Line horizontal = new Line(color);
                    horizontal.Add(new Point(i, j));
                    // Если справа есть еще шары и цвет совпадает с текущим - пробуем построить путь
                    while (j + 1 < _field.Columns && color.Equals(_field[i * _field.Columns + (j + 1)].CellColor))
                    {
                        ++j;
                        horizontal.Add(new Point(i, j));
                    }
                    if (horizontal.GetLine().Count >= 3) { return true; }
                }
            }
            // Vertical lines
            for (int j = 0; j < _field.Columns; ++j)
            {
                for (int i = 0; i < _field.Rows; ++i)
                {
                    if (_field[i * _field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;
                    Color color = _field[i * _field.Columns + j].CellColor;
                    Line vertical = new Line(color);
                    vertical.Add(new Point(i, j));
                    // Если снизу есть еще шары и цвет совпадает с текущим - пробуем построить путь
                    while (i + 1 < _field.Rows && color.Equals(_field[(i + 1) * _field.Columns + j].CellColor))
                    {
                        ++i;
                        vertical.Add(new Point(i, j));
                    }
                    if (vertical.GetLine().Count >= 3) { return true; }
                }
            }

            return false;
        }
        // Получить список линий на поле из шаров одинаковых цветов
        protected List<Line> GetLines()
        {
            List<Line> lines = new List<Line>();

            for (int i = 0; i < field.Rows; ++i)
            {
                for (int j = 0; j < field.Columns; ++j)
                {
                    // Если нет шара в ячейке - не строим путь а переходим к след. ячейке
                    if (field[i * field.Columns + j].CellColor.Equals(Colors.Transparent)) continue;

                    Color color = field[i * field.Columns + j].CellColor;
                    Line horizontal = new Line(color);
                    horizontal.Add(new Point(i, j));
                    // Если справа есть еще шары и цвет совпадает с текущим - пробуем построить путь
                    while (j + 1 < field.Columns && color.Equals(field[i * field.Columns + (j + 1)].CellColor))
                    {
                        ++j;
                        horizontal.Add(new Point(i, j));
                    }
                    if (horizontal.GetLine().Count >= 3)
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
                    Line vertical = new Line(color);
                    vertical.Add(new Point(i, j));
                    // Если снизу есть еще шары и цвет совпадает с текущим - пробуем построить путь
                    while (i + 1 < field.Rows && color.Equals(field[(i + 1) * field.Columns + j].CellColor))
                    {
                        ++i;
                        vertical.Add(new Point(i, j));
                    }
                    if (vertical.GetLine().Count >= 3)
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
        // Класс точка. Используем для хранения координат каждого шара внутри линии
        protected class Point {
            private int x, y;

            public Point(int x, int y) { this.x = x; this.y = y; }
            public Point(Point other) { this.x = other.x; this.y = other.y; }
            // Доступ к координатам
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
            // Перегрузим вывод в строку (Используем для отладки)
            public override string ToString()
            {
                string s = "{" + X + ", " + Y + "}";
                return s;
            }
            // Сравнение двух объектов типа Point
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
        // Линия из шаров
        protected class Line
        {
            // Список координат шаров линии
            List<Point> line;
            // Цвет линии
            Color color;
            //// Конструкторы ////
            public Line(Color color) 
            {
                this.color = color;
                line = new List<Point>();
            }
            public Line(Line other)
            {
                color = other.color;
                line = new List<Point>();
                line.AddRange(other.line);
            }
            // Добавить координаты очередного шара
            public void Add(Point p) 
            {
                line.Add(new Point(p));
            }
            // Удалить последний добавленый
            public void RemoveLast() 
            {
                line.RemoveAt(line.Count - 1);
            }

            public List<Point> GetLine() { return line; }

            public override int GetHashCode()
            {
                int hash = 1;
                foreach (var p in line)
                {
                    hash ^= p.GetHashCode();
                }
                return hash;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Line objAsPath = obj as Line;
                if (objAsPath == null) return false;
                else return Equals(objAsPath);
            }

            public bool Equals(Line p)
            {
                if (p == null) return false;
                if (!color.Equals(p.color)) return false;

                foreach (var pp in line)
                {
                    if (!p.line.Contains(pp)) return false;
                }
                return true;
            }

            public override string ToString()
            {
                string s = string.Empty;
                foreach (var p in line)
                {
                    s += p;
                }
                return s;
            }
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
