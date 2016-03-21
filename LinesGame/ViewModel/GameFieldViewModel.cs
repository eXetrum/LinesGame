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
using LinesGame.Properties;
using System.ComponentModel;

namespace LinesGame.ViewModel
{
    public class GameFieldViewModel : ViewModelBase
    {
        public delegate void OnShowGameSummaryEventHandler(GameFieldViewModel vm);
        public event OnShowGameSummaryEventHandler OnShowSummary;

        public GameFieldViewModel()
        {
            Game = new Model.Game();
            _canExecuteSelect = true;
            _canExecutePauseResume = true;
            _canExecuteStop = true;
            _canExecuteBackToMenu = true;
            Game.OnOver += Game_OnOver;
            CallClose = true;
        }

        void Game_OnOver()
        {
            Console.WriteLine("GameVM OnOver event received");

            if (OnShowSummary != null)
            {
                Console.WriteLine("ON SHOW SUMMARY");
                OnShowSummary(this);
            }
            else
            {
                Console.WriteLine("OnShowSummary == null");
            }
        }
        // Доступ к объекту игры
        public Game Game { get; set; }
        // Закрывать окно при завершении игры или нет
        public bool CallClose { get; set; }
        // Обработка команды клика по шару(клетке)
        public void OnSelectCommand(object param)
        {
            // Если игра не запущена или уже окончена - ничего не делаем
            if (Game._GameStatus == Model.Game.GameStatus.End || Game._GameStatus == Model.Game.GameStatus.PrepareOK) return;
            // Если игра на паузе - предложим продолжить
            if (Game._GameStatus == Model.Game.GameStatus.Paused)
            {
                if (MessageBox.Show("Продолжить игру ?", "Игра на паузе", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question, 
                    MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    Game.ResumeGame();
                }
                else
                {
                    return;
                }
            }
            // Получаем параметр (индекс ячейки по которой кликнули наполе)
            int index = int.Parse((string)param);
            // Сохраняем состояние выделения до всей работы
            bool selectionBefore = Game.Field[index].Selected;
            // Маркер обмена
            bool swapOK = false;             
            // Если выделения не было - значит кликнули на втором шаре 
            // (т.е. у первого шара есть выделение у текущего нет - нужна операция обмена местами)
            if (!selectionBefore) {                
                // Находим индекс шара(ячейки) на поле у которой есть выделение
                int sourceIndex = -1;
                for (int i = 0; i < Game.Field.Rows && sourceIndex == -1; ++i)
                {
                    for (int j = 0; j < Game.Field.Columns && sourceIndex == -1; ++j)
                    {
                        if (Game.Field[i * Game.Field.Columns + j].Selected) sourceIndex = i * Game.Field.Columns + j;
                    }
                }
                // Если нашли выделенную ячейку
                if (sourceIndex != -1)
                {
                    // Меняемся местами с текущей
                    swapOK = Game.Field.SwapCells(index, sourceIndex);
                }
            }

            // Если на пустой клетке клик
            if (Game.Field[index].CellColor.Equals(Colors.Transparent)) return;
            // Снимаем выдление с всех шаров
            for (int i = 0; i < Game.Field.Rows; ++i)
            {
                for (int j = 0; j < Game.Field.Columns; ++j)
                {
                    Game.Field[i * Game.Field.Columns + j].Selected = false;
                }
            }
            // Если был произведен обмен местами ячеек успешно
            if (swapOK)
            {
                // Опция звуков включена или нет
                if (Settings.Default.Sound == Utils.SoundSettings.Enabled)
                {
                    try
                    {
                        // Проигрываем звук обмена
                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Sound\swap_cells.wav");
                        player.Play();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception while sound play: " + ex.Message);
                    }
                }
                // Увеличиваем количество ходов
                ++Game._GameScore.ElapsedMoves;
                // Если игра на ходы - отнимаем от оставшихся
                if (Game._GameScore.GameType == Utils.GameType.LimitedMoves) Game.Remain--;
                // Обнуляем счетчик комбо линий
                Game.ClearCombo();
                // Запускаем проверку игрового поля
                Game.CheckField();
            }
            // Если обмена шаров местами небыло
            else
            {
                // Если не было выделения - выделяем и проигрываем звук если заданы настройки
                if (!selectionBefore)
                {
                    if (Settings.Default.Sound == Utils.SoundSettings.Enabled)
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
                }
                // Выделяем если не было выделения
                Game.Field[index].Selected = !selectionBefore;                
            }    
        }
        // Обрабатываем команду запуска/остановки/паузы игры
        public void OnStartPauseResumeCommand(object param)
        {
            // Нажали на кнопку старт
            if (Game._GameStatus == Model.Game.GameStatus.PrepareOK)
            {
                // Запустили игру
                Game.StartGame();
                Console.WriteLine("Game Start");
            // Нажали снова на кнопку - ставим игру на паузу
            } else if( Game._GameStatus == Model.Game.GameStatus.Running ){
                Game.PauseGame();
                Console.WriteLine("Game PAUSED");
            }
            // Еще раз нажали - продолжаем игру
            else if (Game._GameStatus == Model.Game.GameStatus.Paused)
            {
                Game.ResumeGame();
                Console.WriteLine("Game Resumed");
            }
        }
        // Обработка команды нажатия кнопки "завершить игру"
        public void OnStopCommand(object param)
        {
            // Проверим чтобы игра не была завершена
            if (Game._GameStatus == Model.Game.GameStatus.End) return;
            // Если не передан параметр - значит завершаем НЕ при закрытии окна и вызове события OnClosing
            if (param == null)
            {
                // Выставляем маркер которые позволит закрыть окно по завершению
                CallClose = true; 
            }
            // В противномслучае команда сработала при закрытии окна и вызове обработчика
            // события OnClosing, значит нам не нужно закрывать окно игры оно и так уже закрывается
            else
            {
                // Маркер соответственно выставляем
                CallClose = false;
            }
            // Останавливаем игру
            Game.StopGame();
        }
        // Обработка команды назад к меню
        public void OnBackToMenuCommand(object param)
        {
            // Если игра запущена
            if (Game._GameStatus != Model.Game.GameStatus.Running) return;
            // Ставим на паузу
            Game.PauseGame();
            // Сообщаем что игра на паузе
            MessageBox.Show("Игра приостановлена");
        }

        private ICommand _clickCommand;
        private ICommand _startPauseResumeCommand;
        private ICommand _stopCommand;
        private ICommand _backToMenuCommand;

        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler((object p) => OnSelectCommand(p), _canExecuteSelect));
            }
        }

        public ICommand StartPauseResumeCommand
        {
            get
            {
                return _startPauseResumeCommand ?? (_startPauseResumeCommand = new CommandHandler((object p) => OnStartPauseResumeCommand(p), _canExecutePauseResume));
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new CommandHandler((object p) => OnStopCommand(p), _canExecuteStop));
            }
        }

        public ICommand BackToMenuCommand
        {
            get
            {
                return _backToMenuCommand ?? (_backToMenuCommand = new CommandHandler((object p) => OnBackToMenuCommand(p), _canExecuteBackToMenu));
            }
        }

        private bool _canExecuteSelect;
        private bool _canExecutePauseResume;
        private bool _canExecuteStop;
        private bool _canExecuteBackToMenu;
    }
    // Класс реализующий интерфейс icommand. Используем для работы с командами
    public class CommandHandler : ICommand
    {
        private Action<object> _action;
        private bool _canExecute;
        public CommandHandler(Action<object> action, bool canExecute)
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