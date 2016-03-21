using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using LinesGame.ViewModel;
using LinesGame.Properties;

namespace LinesGame.View
{
    /// <summary>
    /// Interaction logic for GameSetupView.xaml
    /// </summary>
    public partial class GameSetupView : Window
    {
        public GameSetupView()
        {
            InitializeComponent();
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
            Window gameView = null;
            // Создаем окно отображения поля 
            switch (Settings.Default.Field)
            {
                case Model.Utils.FieldType.Field20x20:
                    gameView = new GameField20x20View();
                    break;
                case Model.Utils.FieldType.Field17x19:
                    gameView = new GameField17x19View();
                    break;
                default:
                    gameView = new GameField10x10View();
                    break;
            }
            gameView.Owner = this.Owner;
            // Показываем окно
            gameView.Show();

            object btn = Owner.FindName("newGameBtn");
            if (btn != null && btn as Button != null)
            {
                (btn as Button).Content = "Назад к игре";
            }

            Owner.Hide();
            Close();            
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            

            Close();
        }
    }
}
