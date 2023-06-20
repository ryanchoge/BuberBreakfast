using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers
{
   
   // [ApiController]
    public class ErrorsController : ControllerBase
    { 
        [Route("/errors")]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
