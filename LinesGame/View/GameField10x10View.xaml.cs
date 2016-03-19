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

namespace LinesGame.View
{
    /// <summary>
    /// Interaction logic for GameField10x10View.xaml
    /// </summary>
    public partial class GameField10x10View : Window
    {
        public GameField10x10View(GameSetupViewModel setupViewModel)
        {
            InitializeComponent();
            DataContext = new GameFieldViewModel(setupViewModel.GameSetupModel);
        }

        private void GridLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == null) return;
            Grid myGrid = sender as Grid;
            var point = Mouse.GetPosition(myGrid);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // calc row mouse was over
            foreach (var rowDefinition in myGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // calc col mouse was over
            foreach (var columnDefinition in myGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            MessageBox.Show(row.ToString() + " " + col.ToString());
        }

        private void Timeline_Completed(object sender, EventArgs e)
        {
            GameFieldViewModel vm = DataContext as GameFieldViewModel;

            if(vm.Game.GravityRequire())
            {
                vm.Game.BallsGravityDown();
                Console.WriteLine("gravity require");
            }
            else if (vm.Game.FillEmptyRequire())
            {
                vm.Game.FillEmptyCells();
                vm.Game.ChekField();
                Console.WriteLine("fill empty cells require");
            }
            else
            {
                Console.WriteLine("nothing require");
            }
        }
    }
}
