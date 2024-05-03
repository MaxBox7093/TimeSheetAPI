using Microsoft.Data.SqlClient;

namespace TimeSheetAPI.Models.SQL
{
    public class SQLRequestProject : SQLConnection
    {
        private SqlConnection connection;

        public SQLRequestProject()
        {
            connection = ConnectionDB();
        }

        //Добавление нового проекта по id пользователя
        public void AddNewProject(int id_user, Project project)
        {
            //Транзакция для отслеживания состояния выполнения нескольких запросов
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                int code = project.Code;
                string project_name = project.ProjectName;
                bool is_active_project = project.IsActiveProject;

                //Запрос на добавление нового проекта
                string request_project = "INSERT INTO Project (code, project_name, is_active_project) VALUES (@code, @project_name, @is_active_project); SELECT SCOPE_IDENTITY();";
                var command_project = new SqlCommand(request_project, connection, transaction);
                command_project.Parameters.AddWithValue("@code", code);
                command_project.Parameters.AddWithValue("@project_name", project_name);
                command_project.Parameters.AddWithValue("@is_active_project", is_active_project);

                //Получение id проекта в результате добавления нового проекта
                int projectId = Convert.ToInt32(command_project.ExecuteScalar());

                //Привязка данных в таблице связей Project_control
                string request_project_control = "INSERT INTO Project_control (Id_user, id_project) VALUES (@Id_user, @id_project)";
                var command_project_control = new SqlCommand(request_project_control, connection, transaction);
                command_project_control.Parameters.AddWithValue("@Id_user", id_user);
                command_project_control.Parameters.AddWithValue("@id_project", projectId);

                command_project_control.ExecuteNonQuery();

                //Если все прошло успешно, фиксируем транзакцию
                transaction.Commit();
            }
            catch (Exception ex)
            {
                //В случае ошибки откатываем транзакцию
                transaction?.Rollback();
                Console.WriteLine("Ошибка при добавлении проекта: " + ex.Message);
            }
        }

        //Каскадное удаление проекта
        public void DeleteProject(ref int code) 
        {
            try
            {
                string request = "DELETE Project WHERE code=@code";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@code", code);

                command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Ошибка при обновлении проекта: " + ex.Message);
            }
        }

        //Обновление данных о проекте
        public void UpdateProject(int code, Project project)
        {
            int newCode = project.Code;
            string newProjectName = project.ProjectName;
            bool newIsActiveProject = project.IsActiveProject;
            try
            {
                //Запрос на обновление данных проекта
                string request = "UPDATE Project SET code = @newCode, project_name = @newProjectName, is_active_project = @newIsActiveProject WHERE code = @code";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@newCode", newCode);
                command.Parameters.AddWithValue("@newProjectName", newProjectName);
                command.Parameters.AddWithValue("@newIsActiveProject", newIsActiveProject);
                command.Parameters.AddWithValue("@code", code);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обновлении проекта: " + ex.Message);
            }
        }
        
        //Нахождение проектов по id пользователя
        public List<Project> SelectProjects(int userId)
        {
            List<Project> projects = new List<Project>();

            try
            {
                // Запрос на выборку всех проектов для конкретного пользователя
                string request = "SELECT * FROM Project JOIN Project_control ON Project.Id_project = Project_control.id_project " +
                    "WHERE Project_control.Id_user = @userId";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@userId", userId);

                using (var reader = command.ExecuteReader())
                {
                    // Перебираем результаты запроса
                    while (reader.Read())
                    {
                        Project project = new Project();

                        // Получаем данные о проекте
                        project.Id = reader.GetInt32(reader.GetOrdinal("Id_project"));
                        project.Code = reader.GetInt32(reader.GetOrdinal("code"));
                        project.ProjectName= reader.GetString(reader.GetOrdinal("project_name"));
                        project.IsActiveProject = reader.GetBoolean(reader.GetOrdinal("is_active_project"));

                        projects.Add(project);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выборе проектов: " + ex.Message);
            }

            return projects;
        }
    }
}
