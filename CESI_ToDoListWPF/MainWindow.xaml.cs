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
        private ObservableCollection<ToDoTask> tasks = new ObservableCollection<ToDoTask>();
        private ObservableCollection<int> hours = new ObservableCollection<int>();
        private ObservableCollection<int> minutes = new ObservableCollection<int>();

        public ObservableCollection<ToDoTask> Tasks
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
            var dbContext = new ApplicationDbContext(); // Créer une instance de ApplicationDbContext
            dbContext.InitializeDatabase(); // Créer la base de données si nécessaire
            Tasks = dbContext.LoadTasks(); // Charger les tâches à partir de la base de données
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
                ToDoTask newTask = new ToDoTask
                {
                    Title = title,
                    DateTime = combinedDateTime
                };
                Tasks.Add(newTask);
                txtTask.Text = string.Empty;
                dpTaskDate.SelectedDate = null;
                cmbHours.SelectedIndex = -1;
                cmbMinutes.SelectedIndex = -1;
                var dbContext = new ApplicationDbContext();
                dbContext.SaveTasks(Tasks); // Sauvegarder les tâches dans la base de données
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ToDoTask task = (ToDoTask)btn.Tag;
            Tasks.Remove(task);
            var dbContext = new ApplicationDbContext();
            dbContext.SaveTasks(Tasks); // Sauvegarder les tâches dans la base de données
        }

        private void chkCompleted_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ToDoTask task = (ToDoTask)checkBox.DataContext;
            task.Completed = checkBox.IsChecked ?? false;
            var dbContext = new ApplicationDbContext();
            dbContext.UpdateTask(task); // Mettre à jour la tâche dans la base de données
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
