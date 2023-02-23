using E_VilleMarketing.Data;
using Microsoft.AspNetCore.Mvc;

namespace E_VilleMarketing.Controllers
{
    public class CompositionController : Controller
    {
        private readonly DatabaseContext _context;
        public string passedLayout = "null";
        public CompositionController(DatabaseContext context)
        {
            _context = context;
        }
        public void CheckSession()
        {
            if (HttpContext.Session.GetInt32("clientID").HasValue)
            {
                Console.WriteLine("has value");
                passedLayout = "_ClientLayout";
            }
        }
        public IActionResult Index()
        {
            CheckSession();
            ViewData["Layout"] = passedLayout;
            return View();
        }
    }
}
