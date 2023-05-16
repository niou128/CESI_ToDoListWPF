using System.ComponentModel;

namespace CESI_ToDoListWPF
{
    public class Task : INotifyPropertyChanged
    {
        private string title;
        private bool completed;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
