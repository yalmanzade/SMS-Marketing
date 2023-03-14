using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    public class ErrorController : Controller
    {
        // GET: ErrorController
        public ActionResult Index()
        {
            object? error = TempData["Error"];
            if (error != null)
            {
                UserErrorModel model = new UserErrorModel();
                model.ErrorMessage = error.ToString();
                TempData["Error"] = null;
                return View(model);
            }
            return View();
        }
    }
}
