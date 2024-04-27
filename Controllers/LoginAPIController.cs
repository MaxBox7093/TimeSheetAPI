using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginAPIController : Controller
    {
        [HttpGet]
        public IActionResult Login([FromQuery] User_login user)
        {
            if (user == null)
            {
                return BadRequest("Данные логина и пароля не указаны");
            }

            // Пример: проверка логина и пароля в базе данных
            if (user.RequestGetIdUser() == true)
            {
                return Ok(new { userId = user.Id });
            }
            else
            {
                return Unauthorized("Неправильный логин или пароль");
            }
        }
    }
}
