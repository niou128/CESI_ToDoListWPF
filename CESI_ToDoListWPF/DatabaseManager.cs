using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace CESI_ToDoListWPF
{
    public static class DatabaseManager
    {
        private static string databaseFile = "todo.db";

        public static void CreateDatabase()
        {
            if (!File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);

                using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE Tasks (Title TEXT, Completed INTEGER)", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static ObservableCollection<Task> LoadTasks()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            if (!File.Exists(databaseFile))
                return tasks;

            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Tasks", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task
                            {
                                Title = reader["Title"].ToString(),
                                Completed = Convert.ToBoolean(reader["Completed"])
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
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();

                // Supprimer toutes les tâches existantes
                using (SQLiteCommand deleteCommand = new SQLiteCommand("DELETE FROM Tasks", connection))
                {
                    deleteCommand.ExecuteNonQuery();
                }

                // Insérer les nouvelles tâches
                using (SQLiteCommand insertCommand = new SQLiteCommand(connection))
                {
                    foreach (Task task in tasks)
                    {
                        insertCommand.CommandText = "INSERT INTO Tasks (Title, Completed) VALUES (@title, @completed)";
                        insertCommand.Parameters.AddWithValue("@title", task.Title);
                        insertCommand.Parameters.AddWithValue("@completed", task.Completed);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private static string GetConnectionString()
        {
            return $"Data Source={databaseFile};Version=3;";
        }
    }
}
