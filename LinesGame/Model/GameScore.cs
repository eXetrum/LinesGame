using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace LinesGame.Model
{
    public class GameScore : INotifyPropertyChanged
    {
        private Timer timer;
        private long seconds;
        private int movescount;
        private long score;

        public GameScore()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            seconds = 0;
            movescount = 0;
            score = 0;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ++seconds;
            OnPropertyChanged("GameTime");
            //Console.WriteLine(GameTime);
        }

        public TimeSpan GameTime
        {
            get { return TimeSpan.FromSeconds(seconds); }
            set
            { 
                seconds = value.Seconds;
                OnPropertyChanged("GameTime");
            }
        }

        public int MovesCount 
        {
            get { return movescount; }
            set 
            {
                movescount = value;
                OnPropertyChanged("MovesCount");
            }
        }

        public long Score
        {
            get { return score; }
            set 
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }

        public void Reset()
        {
            timer.Stop();
            timer.Dispose();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
            timer.Start();

            seconds = 0;
            MovesCount = 0;
            Score = 0;
            GameTime = new TimeSpan(0);
        }

        public void ResumeRecord()
        {
            timer.Enabled = true;
            timer.Start();
        }

        public void StopRecord()
        {
            timer.Stop();
            timer.Enabled = false;
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
