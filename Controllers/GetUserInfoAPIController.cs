using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeSheetAPI.Models;

namespace TimeSheetAPI.Controllers
{
    [ApiController]
    [Route("api/getUserInfo")]
    public class GetUserInfoAPIController : Controller
    {
        [HttpGet]
        public ActionResult GetUserInfo([FromQuery] int id) 
        {
            Users user = new Users(id);
            user.GetUserInfo();
            return Ok(user);
        }
    }
}
