using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Tasks = new ObservableCollection<Task>();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTask.Text;
            if (!string.IsNullOrWhiteSpace(title))
            {
                Tasks.Add(new Task { Title = title });
                txtTask.Text = string.Empty;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Task task = (Task)btn.Tag;
            Tasks.Remove(task);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
