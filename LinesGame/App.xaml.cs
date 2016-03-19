using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LinesGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var startupWin = new View.StartupView
            {
                //DataContext = new LinesGame.ViewModel.GameFieldViewModel(10, 10)
            };
            startupWin.Show();
        }
    }
}
