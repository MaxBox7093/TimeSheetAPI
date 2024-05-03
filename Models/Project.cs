namespace TimeSheetAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string? ProjectName { get; set; }
        public bool IsActiveProject { get; set; }
    }
}
