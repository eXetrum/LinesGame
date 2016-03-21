using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Xml.Serialization;

namespace LinesGame.Model
{
    [Serializable]
    public class GameScore : INotifyPropertyChanged
    {
        // Дата игры
        private DateTime gamedate;
        // Тип игры (на время или количество ходов)
        private Utils.GameType gametype;
        // Тип поля
        private Utils.FieldType fieldtype;
        // Прошло времени
        private long elapsedseconds;        
        // Сделано ходов
        private int elapsedmoves;        
        // Максимальная серия линий полученная за ход
        private int maxcombo;
        // Очки
        private long score;
        
        // Конструктор по умолчанию
        public GameScore()
        {
            gametype = Utils.GameType.LimitedTime;
            fieldtype = Utils.FieldType.Field10x10;
            Reset();
        }
        // Конструктор с параметрами
        public GameScore(Utils.GameType gametype, Utils.FieldType fieldtype)
        {
            // Запоминаем тип игры
            this.gametype = gametype;
            // Тип поля
            this.fieldtype = fieldtype;
            // Сброс всех счетчиков
            Reset();
        }

        public DateTime GameDate
        {
            get
            {
                return gamedate;
            }
            set
            {
                gamedate = value;
            }
        }

        public Utils.GameType GameType
        {
            get
            {
                return gametype;
            }
            set
            {
                gametype = value;
            }
        }

        public Utils.FieldType FieldType
        {
            get { return fieldtype; }
            set
            {
                fieldtype = value;
                OnPropertyChanged("FieldType");
            }
        }

        public long ElapsedSeconds
        {
            get { return elapsedseconds; }
            set
            {
                elapsedseconds = value;
                OnPropertyChanged("ElapsedSeconds");
            }
        }

        public int ElapsedMoves
        {
            get { return elapsedmoves; }
            set 
            {
                elapsedmoves = value;                
                OnPropertyChanged("ElapsedMoves");
            }
        }

        public int MaxCombo
        {
            get
            {
                return maxcombo;
            }
            set
            {
                maxcombo = value;
                OnPropertyChanged("MaxCombo");
            }
        }

        public long Score
        {
            get { return score; }
            set 
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }

        public void Reset()
        {
            // Запоминаем дату игры
            gamedate = new DateTime(DateTime.Now.Ticks);
            // Обнуляем все счетчики
            ElapsedSeconds = 0;
            ElapsedMoves = 0;
            MaxCombo = 0;
            Score = 0;            
        }
        // Сериализация объекта данных игры в xml файл
        public static void SaveScore(GameScore serializableObject)
        {
            string fileName = "Records/" + serializableObject.GameDate.ToString("dd-MM-yyyy HH-mm-ss") + ".xml";
            // Создаем директорию Records если таковой еще нет
            (new FileInfo(fileName)).Directory.Create();
            // Проверим чтобы объект был передан
            if (serializableObject == null) { return; }
            // Проубем записать в файл
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(GameScore));
                    serializer.Serialize(stream, serializableObject);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveScore exception: " + ex.Message);
            }
        }
        // Разбор xml файла и построение объекта GameScore
        public static GameScore LoadScore(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(GameScore); }

            GameScore objectOut = null;

            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(GameScore));
                    objectOut = (GameScore) serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadScore exception: " + ex.Message);
            }

            return objectOut;
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
