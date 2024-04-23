namespace TimeSheetAPI.Models
{
    public class Users
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }

        public Users(string Name, string Lastname, int Age)
        {
            this.Name = Name;
            this.Lastname = Lastname;
            this.Age = Age;
        }
    }
}
