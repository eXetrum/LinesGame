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
            Window gameWindow = null;
            foreach (Window w in this.OwnedWindows)
            {
                if (w.Name.Equals("GameField"))
                {
                    gameWindow = w;
                    break;
                }
            }

            if (gameWindow == null)
            {
                GameSetupView gameSetupView = new GameSetupView();
                gameSetupView.Owner = this;
                gameSetupView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                gameSetupView.ShowDialog();
                return;
            }

            gameWindow.Show();
            this.Hide();
        }

        private void scoreGameBtn_Click(object sender, RoutedEventArgs e)
        {
            RecordsTableView table = new RecordsTableView();
            table.Owner = this;
            table.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            table.ShowDialog();
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
            GameRulesView gameRulesView = new GameRulesView();
            gameRulesView.Owner = this;
            gameRulesView.ShowDialog();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("keydown");
            if (e.Key != Key.Escape)
            {
                e.Handled = true;
                return;
            }
            Window gameWindow = null;
            foreach (Window w in this.OwnedWindows)
            {
                if (w.Name.Equals("GameField"))
                {
                    gameWindow = w;
                    break;
                }
            }

            if (gameWindow != null && e.Key == Key.Escape)
            {
                Console.WriteLine("GameFiled window found !");
                this.Hide();
                gameWindow.Show();
                gameWindow.Activate();
            }
            else
            {
                Console.WriteLine("GameFiled window not found");
            }            
            e.Handled = true;
        }
    }
}
