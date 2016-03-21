using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;

namespace LinesGame.Model
{
    public static class Utils
    {
        [Serializable]
        public enum FieldType
        {   
            Field10x10,     // Поле 10х10 
            Field20x20,     // Поле 20х20 
            Field17x19,     // Поле 17х19
        }

        [Serializable]
        public enum SoundSettings
        {
            Enabled,        // Звук вкл
            Disabled        // Звук выкл
        }

        [Serializable]
        public enum GameType
        {
            LimitedTime,    // Режим ограниченного времени
            LimitedMoves    // Режим ограниченного количества ходов
        }
        // Возможные цвета шаров
        public static Color[] BallColors = new Color[] 
        {
            Colors.Red,
            Colors.Green,
            Colors.Orange,
            Colors.SteelBlue,
            Colors.Navy,
            Colors.BlueViolet,
            Colors.Brown            
        };
    }
}
