using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models.SQL;

namespace TimeSheetAPI.Controllers
{
    [ApiController]
    [Route("api/getAllTimeSheetByDate")]
    public class GetAllTimeSheetByDateAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetAllTimeSheet([FromQuery] int user_id, [FromQuery] string dateStart, [FromQuery] string dateEnd)
        {
            SQLRequestTimeSheetAllData data = new SQLRequestTimeSheetAllData();
            var result = data.SelectTimeSheetByDate(user_id, dateStart, dateEnd);
            return Ok(result);
        }
    }
}
