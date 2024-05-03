using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;
using TimeSheetAPI.Models.SQL;

namespace TimeSheetAPI.Controllers
{
    [ApiController]
    [Route("api/project")]
    public class ProjectAPIController : Controller
    {
        [HttpGet]
        public IActionResult GetProjects([FromQuery] int userId)
        {
            try
            {
                List<Project> projects; // Переменная для хранения списка проектов
                SQLRequestProject sqlRequestProject = new SQLRequestProject();
                projects = sqlRequestProject.SelectProjects(userId); // Получение списка проектов по id пользователя
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при получении списка проектов: {ex.Message}");
            }
        }

        [HttpDelete]
        public IActionResult DeleteProject([FromQuery] int code)
        {
            try
            {
                SQLRequestProject sqlRequestProject = new SQLRequestProject();
                sqlRequestProject.DeleteProject(ref code);
                return Ok("Проект успешно удален.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при удалении проекта: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateProject([FromQuery] int userId, [FromBody] Project project)
        {
            try
            {
                SQLRequestProject sqlRequestProject = new SQLRequestProject();
                sqlRequestProject.AddNewProject(userId, project); // Добавление нового проекта
                return Ok("Проект успешно создан.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при создании проекта: {ex.Message}");
            }
        }

        [HttpPatch]
        public IActionResult UpdateProject([FromQuery] int code, [FromBody] Project project)
        {
            try
            {
                SQLRequestProject sqlRequestProject = new SQLRequestProject();
                sqlRequestProject.UpdateProject(code, project); // Обновление данных проекта
                return Ok("Данные проекта успешно обновлены.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка при обновлении данных проекта: {ex.Message}");
            }
        }
    }
}
