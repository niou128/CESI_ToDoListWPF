using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace CESI_ToDoListWPF
{
    public static class DatabaseManager
    {
        private const string DatabasePath = "todolist.db";

        public static void CreateDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Tasks (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT, DateTime DATETIME, Completed INTEGER)";
                    command.ExecuteNonQuery();
                }
            }
        }

        public static ObservableCollection<Task> LoadTasks()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT Id, Title, DateTime, Completed FROM Tasks ORDER BY DateTime ASC";

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                DateTime = reader.GetDateTime(2),
                                Completed = reader.GetBoolean(3)
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;
        }

        public static void SaveTasks(ObservableCollection<Task> tasks)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM Tasks";
                    command.ExecuteNonQuery();

                    foreach (Task task in tasks)
                    {
                        command.CommandText = $"INSERT INTO Tasks (Title, DateTime, Completed) VALUES ('{task.Title}', '{task.DateTime.ToString("yyyy-MM-dd HH:mm:ss")}', {(task.Completed ? 1 : 0)})";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void UpdateTask(Task task)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"UPDATE Tasks SET Completed={(task.Completed ? 1 : 0)} WHERE Id={task.Id}";
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
