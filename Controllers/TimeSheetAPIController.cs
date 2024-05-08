using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models.SQL;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Controllers
{
    [ApiController]
    [Route("api/timeSheet")]
    public class TimeSheetAPIController : Controller
    {
        [HttpDelete]
        public IActionResult DeleteTimeSheet([FromQuery] int id_ts)
        {
            try
            {
                SQLRequestTimeSheet sqlRequestProject = new SQLRequestTimeSheet();
                sqlRequestProject.DeleteTimeSheet(ref id_ts);
                return Ok("Проводка удалена.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при удалении проводки: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateTimeSheet([FromBody] TimeSheet timeSheet)
        {
            try
            {
                SQLRequestTimeSheet sqlRequestProject = new SQLRequestTimeSheet();
                sqlRequestProject.AddNewTimeSheet(timeSheet); // Добавление новой проводки
                return Ok("Проводка успешно создана.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при создании проводки: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetTimeSheet([FromQuery] int id_task)
        {
            try
            {
                List<TimeSheet> ts; // Переменная для хранения списка проектов
                SQLRequestTimeSheet sqlRequestTs = new SQLRequestTimeSheet();
                ts = sqlRequestTs.SelectTimeSheet(id_task); // Получение списка проектов по id пользователя
                return Ok(ts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при получении списка проектов: {ex.Message}");
            }
        }

        [HttpPatch]
        public IActionResult PatchTimeSheet([FromBody] TimeSheet ts)
        {
            try
            {
                SQLRequestTimeSheet sqlRequestTs = new SQLRequestTimeSheet();
                sqlRequestTs.UpadateTimeSheet(ts); // Получение списка проектов по id пользователя
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при получении списка проектов: {ex.Message}");
            }
        }

    }
}
