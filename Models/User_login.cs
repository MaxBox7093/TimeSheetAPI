using TimeSheetAPI.Models.SQL;

namespace TimeSheetAPI.Models
{
    public class User_login
    {
        public string login { get; set; }
        public string password { get; set; }
        public int Id = 0;

        public bool RequestGetIdUser() 
        {
            SQLRequest request = new SQLRequest();
            this.Id = request.GetUserId(login, password);
            if (this.Id != 0)
                return true;
            return false;
        }
    }
}
