using TimeSheetAPI.Models.SQL;

namespace TimeSheetAPI.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name;
        public string Lastname;

        public Users(int Id) 
        {
            this.Id = Id;
        }

        public Users() { }

        //Методы на получение значений
        public string GetName
        {
            get { return Name; }
        }

        public string GetLastname
        {
            get { return Lastname; }
        }

        public void GetUserInfo() 
        {
            SQLRequest request = new SQLRequest();
            var user = request.GetUserNameAndLastname(Id);
            Name = user.Name;
            Lastname = user.Lastname;
        }
    }
}
