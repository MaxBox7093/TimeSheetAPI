using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;
using TimeSheetAPI.Models.SQL;

namespace TimeSheetAPI.Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TaskAPIController : Controller
    {
        [HttpGet]
        public IActionResult GetTask([FromQuery] int id_project)
        {
            try
            {
                List<TaskProject> tasks = new List<TaskProject>();
                SQLRequestTask requestTask = new SQLRequestTask();
                tasks = requestTask.SelectTask(id_project);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult PostTask([FromBody] TaskProject task)
        {
            try 
            {
                SQLRequestTask requestTask = new SQLRequestTask();
                requestTask.AddNewTask(task);
                return Ok("Задача создана");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public IActionResult UpdateTask([FromBody] TaskProject task)
        {
            try
            {
                SQLRequestTask sqlRequestProject = new SQLRequestTask();
                sqlRequestProject.UpdateTask(task); // Обновление данных задачи
                return Ok("Данные проекта успешно обновлены.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при обновлении данных проекта: {ex.Message}");
            }
        }

        [HttpDelete]
        public IActionResult DeleteTask([FromQuery] int id_task)
        {
            try
            {
                SQLRequestTask requestTask = new SQLRequestTask();
                requestTask.DeleteTask(ref id_task);
                return Ok("Задача удалена");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
