using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;
using TimeSheetAPI.Models.SQL;

namespace TimeSheetAPI.Controllers
{
    [ApiController]
    [Route("api/getAllTimeSheet")]
    public class GetAllTimeSheetAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetAllTimeSheet([FromQuery] int user_id)
        {
            List<TimeSheetAllData> dataTS = new List<TimeSheetAllData>();
            SQLRequestTimeSheetAllData data = new SQLRequestTimeSheetAllData();
            dataTS = data.SelectAllTimeSheet(user_id);
            return Ok(dataTS);
        }
    }
}
