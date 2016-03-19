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

namespace LinesGame.Model
{
    public static class Utils
    {
        [Serializable]
        public enum FieldType
        {
            Field10x10,
            Field20x20,
            Field17x19,
        }

        [Serializable]
        public enum SoundSettings
        {
            Enabled,
            Disabled
        }

        [Serializable]
        public enum GameType
        {
            LimitedTime,
            LimitedMoves
        }

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
