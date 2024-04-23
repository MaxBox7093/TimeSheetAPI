namespace TimeSheetAPI.Models
{
    public class User_login : Users
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public User_login(string Name, string Lastname, int Age, string Login, string Password)
            : base(Name, Lastname, Age)
        {
            this.Login = Login;
            this.Password = Password;
        }
    }
}
