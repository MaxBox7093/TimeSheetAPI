using System.Diagnostics.Eventing.Reader;

namespace TimeSheetAPI.Models
{
    public class TaskProject
    {
        public int Id_task { get; set; }
        public string Task_name { get; set; }
        public int Task_ref { get; set; }
        public bool Is_active_task { get; set; }
    }
}
