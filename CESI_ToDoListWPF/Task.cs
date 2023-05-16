using System;
using System.ComponentModel;

namespace CESI_ToDoListWPF
{
    public class Task : INotifyPropertyChanged
    {
        private int id;
        private string title;
        private bool completed;
        private DateTime dateTime;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public bool Completed
        {
            get { return completed; }
            set
            {
                completed = value;
                OnPropertyChanged(nameof(Completed));
            }
        }

        public DateTime DateTime
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                OnPropertyChanged(nameof(DateTime));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
