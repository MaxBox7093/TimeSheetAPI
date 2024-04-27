using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TimeSheetAPI.Models.SQL
{
    public class Authenticator : SQLConnection
    {
        private int userId;
        private string login;
        private string password;

        public Authenticator(ref string login, ref string password) 
        {
            this.login = login;
            this.password = password;
            connection = ConnectionDB();
        }

        //Валидация данных, проверка на пустые строки
        private bool Validator() 
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
                return true;
            else return false;
        }
	}
}
