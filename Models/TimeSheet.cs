namespace TimeSheetAPI.Models
{
    public class TimeSheet
    {
        public int id { get; set; }
        public string? date { get; set; }
        public int time { get; set; }
        public string? description { get; set; }
        public int ts_ref { get; set; }
    }
}
