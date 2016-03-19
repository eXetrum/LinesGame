using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LinesGame.Model;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;

namespace LinesGame.ViewModel
{
    public class GameFieldViewModel : ViewModelBase
    {
        private bool gameover;


        public GameFieldViewModel(FieldSetup SetupModel)
        {
            Game = new Model.Game(SetupModel);
            _canExecuteSelect = true;
            _canExecutePauseResume = true;
            _canExecuteStop = true;
            gameover = false;
        }

        public Game Game { get; private set; }

        public bool GameRunning
        {
            get
            {
                return Game.Running;
            }
            set
            {
                Game.Running = value;
                OnPropertyChanged("GameRunning");
            }
        }

        public bool GameOver
        {
            get
            {
                return gameover;
            }
            set
            {
                gameover = value;
            }
        }

        public void PauseGame()
        {
            GameRunning = false;
            Game.PauseGame();
        }

        public void ResumeGame()
        {
            GameRunning = true;
            Game.ResumeGame();
        }

        public void StopGame()
        {
            GameOver = true;
            Game.StopGame();
        }

        public void OnCommand(object param)
        {
            if (GameOver) return;

            if (!GameRunning)
            {
                if (MessageBox.Show("Продолжить игру ?", "Игра на паузе", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    ResumeGame();
                }
                else
                {
                    return;
                }
            }
            //MessageBox.Show("PARAM: + " + param.ToString());
            //Selected = !Selected;
            //OnPropertyChanged("Selected");
            //MessageBox.Show(Selected.ToString());
            //PropertyChanged(this, new PropertyChangedEventArgs("Selected"));
            int index = int.Parse((string)param);
            bool selectionBefore = Game.Field[index].Selected;
            bool swapOK = false;             
            // Если выделения не было - значит кликнули на втором шаре (т.е. у первого шара есть выделение у текущего нет - нужна операция обмена местами)
            if (!selectionBefore) {                
                int sourceIndex = -1;
                for (int i = 0; i < Game.Field.Rows && sourceIndex == -1; ++i)
                {
                    for (int j = 0; j < Game.Field.Columns && sourceIndex == -1; ++j)
                    {
                        if (Game.Field[i * Game.Field.Columns + j].Selected) sourceIndex = i * Game.Field.Columns + j;
                    }
                }
                // Выделения нет ни на одной ячейке
                if (sourceIndex != -1)
                {
                    swapOK = Game.Field.SwapCells(index, sourceIndex);
                }
            }
            else
            {
                
            }

            if (Game.Field[index].CellColor.Equals(Colors.Transparent)) return;

            for (int i = 0; i < Game.Field.Rows; ++i)
            {
                for (int j = 0; j < Game.Field.Columns; ++j)
                {
                    Game.Field[i * Game.Field.Columns + j].Selected = false;
                }
            }

            if (!swapOK)
            {
                if (!selectionBefore)
                {
                    try
                    {
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Sound\select_cell.wav");
                        player.Play();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception while sound play: " + ex.Message);
                    }
                }
                Game.Field[index].Selected = !selectionBefore;
            }
            else
            {
                try
                {
                    //swap_cells
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Sound\swap_cells.wav");
                    player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception while sound play: " + ex.Message);
                }
                // Увеличиваем количество ходов
                ++Game.Score.MovesCount;
                // 
                Game.ChekField();
                //List<Game.Path> lines = Game.GetLines();
                //foreach (var l in lines)
                //{
                //    l.GetPath();
                //    DoubleAnimation animation = new DoubleAnimation();
                //    animation.Duration = new Duration(TimeSpan.FromSeconds(2));
                //    animation.From = 1;
                //    animation.To = 0;
                //    animation.FillBehavior = FillBehavior.Stop;
                    
                //}
                //animation.Changed += animation_Changed;
                //void animation_Changed(object sender, EventArgs e)
            }
    
        }

        public void OnPauseResumeCommand(object param)
        {
            if (!GameRunning)
            {
                ResumeGame();
            } else {
                PauseGame();
            }
        }

        public void OnStopCommand(object param)
        {
            StopGame();
        }

        private ICommand _clickCommand;
        private ICommand _pauseResumeCommand;
        private ICommand _stopCommand;

        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler((object p) => OnCommand(p), _canExecuteSelect));
            }
        }

        public ICommand PauseResumeCommand
        {
            get
            {
                return _pauseResumeCommand ?? (_pauseResumeCommand = new CommandHandler((object p) => OnPauseResumeCommand(p), _canExecutePauseResume));
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new CommandHandler((object p) => OnStopCommand(p), _canExecuteStop));
            }
        }

        private bool _canExecuteSelect;
        private bool _canExecutePauseResume;
        private bool _canExecuteStop;
    }

    //private delegate void WorkItem(object param);

    public class CommandHandler : ICommand
    {
        private Action<object> _action;
        //private WorkItem _action;
        private bool _canExecute;
        public CommandHandler(Action<object> action, bool canExecute)//WorkItem action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
