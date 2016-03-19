using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinesGame.Model
{   
    public class FieldSetup : INotifyPropertyChanged
    {
        private Utils.FieldType fieldtype;
        private int ballcount;

        public FieldSetup() { }

        public Utils.FieldType FieldType
        {
            get 
            {
                return fieldtype; 
            }
            set
            {
                if (this.fieldtype == value) return;
                this.fieldtype = value;

                OnPropertyChanged("FieldType");
                OnPropertyChanged("IsFieldSize10x10");
                OnPropertyChanged("IsFieldSize20x20");
                OnPropertyChanged("IsFieldSize17x19");
            }
        }

        public bool IsFieldSize10x10 
        { 
            get { return fieldtype == Utils.FieldType.Field10x10; }
            set { FieldType = value ? Utils.FieldType.Field10x10 : fieldtype; }
        }

        public bool IsFieldSize20x20 
        {
            get { return fieldtype == Utils.FieldType.Field20x20; }
            set { FieldType = value ? Utils.FieldType.Field20x20 : fieldtype; }
        }
        public bool IsFieldSize17x19 
        {
            get { return fieldtype == Utils.FieldType.Field17x19; }
            set { FieldType = value ? Utils.FieldType.Field17x19 : fieldtype; }
        }

        public int BallCount
        {
            get { return ballcount; }
            set 
            {
                ballcount = value;
                OnPropertyChanged("BallCount");
            }
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
