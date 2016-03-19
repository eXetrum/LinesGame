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
    /// Interaction logic for StartupView.xaml
    /// </summary>
    public partial class StartupView : Window
    {
        public StartupView()
        {
            InitializeComponent();
        }

        private void newGameBtn_Click(object sender, RoutedEventArgs e)
        {
            GameSetupView gameSetupView = new GameSetupView();
            gameSetupView.Owner = this;
            gameSetupView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            //gameSetupView.DataContext = new GameSetupViewModel();
            gameSetupView.ShowDialog();
            //GameField10x10 field = new GameField10x10();
            //field.Owner = this;
            //field.Show();

        }

        private void loadGameBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("not implemented");
        }

        private void saveGameBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("not implemented");
        }

        private void settingsGameBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingsView settingsView = new SettingsView();
            settingsView.Owner = this;
            settingsView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            settingsView.ShowDialog();
        }

        private void rulesGameBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("not implemented");
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
