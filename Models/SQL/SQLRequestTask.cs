using Microsoft.Data.SqlClient;

namespace TimeSheetAPI.Models.SQL
{
    public class SQLRequestTask : SQLConnection
    {
        private SqlConnection connection;

        public SQLRequestTask()
        {
            connection = ConnectionDB();
        }

        //Добавление новой задачи по коду проекта
        public void AddNewTask(TaskProject task)
        {
            try
            {   
                string task_name = task.Task_name;
                int task_ref = task.Task_ref;
                bool is_active_task = task.Is_active_task;

                //Запрос на добавление нового проекта
                string request = "INSERT INTO Task (task_name, task_ref, is_active_task) VALUES (@task_name, @task_ref, @is_active_task)";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@task_name", task_name);
                command.Parameters.AddWithValue("@task_ref", task_ref);
                command.Parameters.AddWithValue("@is_active_task", is_active_task);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении проекта: " + ex.Message);
            }
        }

        //Каскадное удаление задачи
        public void DeleteTask(ref int id_task)
        {
            try
            {
                string request = "DELETE Task WHERE Id_task=@id_task";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@id_task", id_task);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении задачи: " + ex.Message);
            }
        }

        //Обновление данных о задаче
        public void UpdateTask(TaskProject task)
        {
            int id_task = task.Id_task;
            string new_task_name = task.Task_name;
            bool new_is_active_task = task.Is_active_task;
            try
            {
                //Запрос на обновление данных проекта
                string request = "UPDATE Task SET task_name = @new_task_name, is_active_task = @new_is_active_task WHERE Id_task = @id_task";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@id_task", id_task);
                command.Parameters.AddWithValue("@new_task_name", new_task_name);
                command.Parameters.AddWithValue("@new_is_active_task", new_is_active_task);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обновлении проекта: " + ex.Message);
            }
        }

        //Получение задач по id проекта
        public List<TaskProject> SelectTask(int Id_project)
        {
            List<TaskProject> Tasks = new List<TaskProject>();

            try
            {
                // Запрос на выборку всех проектов для конкретного пользователя
                string request = "SELECT * FROM Task WHERE task_ref = @Id_project";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@Id_project", Id_project);

                using (var reader = command.ExecuteReader())
                {
                    // Перебираем результаты запроса
                    while (reader.Read())
                    {
                        TaskProject task = new TaskProject();

                        // Получаем данные о проекте
                        task.Id_task = reader.GetInt32(reader.GetOrdinal("Id_task"));
                        task.Task_name = reader.GetString(reader.GetOrdinal("task_name"));
                        task.Task_ref = reader.GetInt32(reader.GetOrdinal("task_ref"));
                        task.Is_active_task = reader.GetBoolean(reader.GetOrdinal("is_active_task"));

                        Tasks.Add(task);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выборе проектов: " + ex.Message);
            }

            return Tasks;
        }
    }
}
