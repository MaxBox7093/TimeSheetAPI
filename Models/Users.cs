namespace TimeSheetAPI.Models
{
    public class Users
    {
        public string Name;
        public string Lastname;
        public int Age;

        //Методы на получение значений
        public string GetName
        {
            get { return Name; }
        }

        public string GetLastname
        {
            get { return Lastname; }
        }

        public int GetAge 
        {
            get { return Age; }
        }

        public Users(string Name, string Lastname, int Age)
        {
            this.Name = Name;
            this.Lastname = Lastname;
            this.Age = Age;
        }
    }
}
