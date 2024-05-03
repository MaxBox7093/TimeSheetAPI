using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace TimeSheetAPI.Models.SQL
{
    public class SQLRequest: SQLConnection
    {
        private SqlConnection connection;

        public SQLRequest() 
        {
            connection = ConnectionDB();
        }

        //Регистрация пользователя в DB
        public void InsertRegistrationUser()
        {
            //Транзакция для отслеживания состояния выполнения нескольких запросов
            SqlTransaction transaction = connection.BeginTransaction();

            try 
            {
                string name = "Супер";
                string lastname = "Учетка";

                string login = "Admin";
                string password = "0000";


                // Вставка записи в User
                string insertTable1Query = "INSERT INTO Users (name, lastname) VALUES (@name, @lastname); SELECT SCOPE_IDENTITY();";
                SqlCommand command1 = new SqlCommand(insertTable1Query, connection, transaction);
                command1.Parameters.AddWithValue("@name", name);
                command1.Parameters.AddWithValue("@lastname", lastname);
                int lastInsertedId = Convert.ToInt32(command1.ExecuteScalar());

                // Вставка записи в User_login
                string insertTable2Query = "INSERT INTO Users_login (id_user_lg, login, password) VALUES (@foreign_key, @login, @password);";
                SqlCommand command2 = new SqlCommand(insertTable2Query, connection, transaction);
                command2.Parameters.AddWithValue("@foreign_key", lastInsertedId);
                command2.Parameters.AddWithValue("@login", login);
                command2.Parameters.AddWithValue("@password", password);

                command2.ExecuteNonQuery();

                transaction.Commit();
                Console.WriteLine("Транзакция выполнена успешно.");
            }

            catch (Exception ex)
            {
                // В случае ошибки откатываем транзакцию
                transaction.Rollback();
                Console.WriteLine("Ошибка при выполнении транзакции: " + ex.Message);
            }
        }

        //Получение id пользователся по логину и паролю
        public int GetUserId(string login, string password) 
        {
            string request = "SELECT Id_user_lg FROM Users_login WHERE login = @Login AND password = @Password";
            var command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@Login", login);
            command.Parameters.AddWithValue("@Password", password);
            return Convert.ToInt32(command.ExecuteScalar());
        }

        //Получение имени и фамилии по id
        //Возвращает объект User с входным id
        public Users GetUserNameAndLastname(int Id_user) 
        {
            Users user = new Users();
            string request = "INSERT INTO ";
            var command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@Id_user", Id_user);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user.Name = reader.GetString(0);
                    user.Lastname = reader.GetString(1);
                    user.Id = Id_user;
                }
            }

            return user;
        }
    }
}
