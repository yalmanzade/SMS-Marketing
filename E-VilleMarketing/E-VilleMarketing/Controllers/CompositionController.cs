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
                passedLayout = "_ClientLayout";
            }
            else if (HttpContext.Session.GetInt32("userID").HasValue)
            {
                passedLayout = "_UserLayout";
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
