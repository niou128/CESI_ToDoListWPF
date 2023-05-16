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

        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            DatabaseManager.CreateDatabase(); // Créer la base de données si nécessaire
            Tasks = DatabaseManager.LoadTasks(); // Charger les tâches à partir de la base de données
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTask.Text;
            if (!string.IsNullOrWhiteSpace(title))
            {
                Tasks.Add(new Task { Title = title });
                txtTask.Text = string.Empty;
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
