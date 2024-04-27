using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class APIController : Controller
    {
        private static List<Person> users1 = new List<Person>
        {
            new Person { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
            new Person { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
            new Person { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
        };

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(users1);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var user = users1.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            var user = users1.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            users1.Remove(user);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser(Person user)
        {
            user.Id = Guid.NewGuid().ToString();
            users1.Add(user);
            return Ok(user);
        }

        [HttpPut]
        public IActionResult UpdateUser(Person userData)
        {
            var user = users1.FirstOrDefault(u => u.Id == userData.Id);
            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            user.Age = userData.Age;
            user.Name = userData.Name;
            return Ok(user);
        }
    }
}
