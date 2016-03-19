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
            DataContext = new GameSetupViewModel();
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            GameSetupViewModel gameSetupViewModel = DataContext as GameSetupViewModel;

            GameField10x10View gameView = new GameField10x10View(gameSetupViewModel);

            gameView.Show();

            Owner.Close();
            Close();            
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();

            Close();
        }
    }
}
