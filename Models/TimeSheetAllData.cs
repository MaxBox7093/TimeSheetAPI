namespace TimeSheetAPI.Models
{
    public class TimeSheetAllData
    {
        public int id_ts { get; set; }
        public string? date_ts { get; set; }
        public int time_ts { get; set; }
        public string? description_ts { get; set; }
        public int ts_ref { get; set; }
        public string? task_name { get; set; }
        public string? project_name { get; set; }
    }
}
