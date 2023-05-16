using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CESI_ToDoListWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Task> tasks;
        private ObservableCollection<int> hours;
        private ObservableCollection<int> minutes;

        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public ObservableCollection<int> Hours
        {
            get { return hours; }
            set
            {
                hours = value;
                OnPropertyChanged(nameof(Hours));
            }
        }

        public ObservableCollection<int> Minutes
        {
            get { return minutes; }
            set
            {
                minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            InitializeTimeData();
            DatabaseManager.CreateDatabase(); // Créer la base de données si nécessaire
            Tasks = DatabaseManager.LoadTasks(); // Charger les tâches à partir de la base de données
        }

        private void InitializeTimeData()
        {
            Hours = new ObservableCollection<int>();
            for (int hour = 0; hour < 24; hour++)
            {
                Hours.Add(hour);
            }

            Minutes = new ObservableCollection<int>();
            for (int minute = 0; minute < 60; minute++)
            {
                Minutes.Add(minute);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTask.Text;
            int selectedHour = (int)cmbHours.SelectedItem;
            int selectedMinute = (int)cmbMinutes.SelectedItem;
            DateTime selectedDateTime = dpTaskDate.SelectedDate ?? DateTime.Now;
            DateTime combinedDateTime = new DateTime(selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, selectedHour, selectedMinute, 0);

            if (!string.IsNullOrWhiteSpace(title))
            {
                Task newTask = new Task
                {
                    Title = title,
                    DateTime = combinedDateTime
                };
                Tasks.Add(newTask);
                txtTask.Text = string.Empty;
                dpTaskDate.SelectedDate = null;
                cmbHours.SelectedIndex = -1;
                cmbMinutes.SelectedIndex = -1;
                DatabaseManager.SaveTasks(Tasks); // Sauvegarder les tâches dans la base de données
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Task task = (Task)btn.Tag;
            Tasks.Remove(task);
            DatabaseManager.SaveTasks(Tasks); // Sauvegarder les tâches dans la base de données
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
