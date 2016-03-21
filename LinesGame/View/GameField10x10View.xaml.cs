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
        public GameField10x10View()
        {
            InitializeComponent();
            GameFieldViewModel vm = new GameFieldViewModel();
            vm.OnShowSummary += vm_OnShowSummary;
            DataContext = vm;
        }

        void vm_OnShowSummary(GameFieldViewModel vm)
        {
            // У родительского окна меняем лейбл на кнопке запускающей новую игру
            object btn = Owner.FindName("newGameBtn");
            if (btn != null && btn as Button != null)
            {
                (btn as Button).Content = "Новая игра";
            }
            GameSummaryView gameSummary = new GameSummaryView(vm);
            gameSummary.Owner = this;
            gameSummary.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            gameSummary.ShowDialog();
            // Показываем родительское окно
            Owner.Show();

            if(vm.CallClose) this.Close();
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
                Console.WriteLine("fill empty cells require");
            }
            else
            {
                vm.Game.CheckField();
                //Console.WriteLine("check field require");
            }
        }

        private void GameFieldWin10x10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;
            GameFieldViewModel vm = DataContext as GameFieldViewModel;
            vm.BackToMenuCommand.Execute(null);

            this.Hide();
            Owner.Show();            
            e.Handled = true;
        }
        // При закрытии окна
        private void GameField_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameFieldViewModel vm = DataContext as GameFieldViewModel;
            vm.StopCommand.Execute(1);
        }

        private void backToMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Owner.Show();
        }
    }
}
