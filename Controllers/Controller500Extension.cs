using Microsoft.AspNetCore.Mvc;


namespace tutorial_5_s23572.Controllers;


public static class Controller500Extension
{
    public static StatusCodeResult InternalServerError(this ControllerBase self)
    {
        return self.StatusCode(StatusCodes.Status500InternalServerError);
    }
}