using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
namespace TimeSheetAPI.Models.SQL
{
    public class SQLRequest
    {
        private SqlConnection connection;
        public SQLRequest() 
        {
            SQLConnection db = new SQLConnection();
            this.connection = db.ConnectionDB();
        }

        public void InsertRegistrationUser(User_login user)
        {
            try 
            {
                string name = user.Name;
                string lastname = user.Lastname;
                int age = user.Age;

                string login = user.Login;
                string password = user.Password;

                SqlTransaction transaction = connection.BeginTransaction();

                // Вставка записи в User
                string insertTable1Query = "INSERT INTO Users (name, lastname, age) VALUES (@name, @lastname, @age); SELECT SCOPE_IDENTITY();";
                SqlCommand command1 = new SqlCommand(insertTable1Query, connection, transaction);
                command1.Parameters.AddWithValue("@name", name);
                command1.Parameters.AddWithValue("@lastname", lastname);
                command1.Parameters.AddWithValue("@age", age);
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

            catch(Exception ex)
            {
                Console.WriteLine("Ошибка добавления пользователя\n" + ex.Message);
            }
        }

        public void Select(ref string login, ref string password) 
        {
            string sqlExpression = $"SELECT name, lastname FROM Users WHERE login == {login} AND password == {password}";
        }
    }
}
