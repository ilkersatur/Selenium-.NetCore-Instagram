using Microsoft.AspNetCore.Mvc;
using SeleniumV2.MouseMethod;

namespace SeleniumV2.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MouseController : ControllerBase
    {
        [HttpPost("middle-click-center")]
        public IActionResult MiddleClickCenter()
        {
            MiddleClick middleClick = new MiddleClick();

            middleClick.MiddleClickAndBottomAction();

            return Ok();
        }


    }
}
