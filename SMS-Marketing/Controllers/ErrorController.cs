using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    public class ErrorController : Controller
    {
        // GET: ErrorController
        public ActionResult Index()
        {
            if (ViewBag.Error)
            {
                UserErrorModel model = new UserErrorModel();
                model.ErrorMessage = ViewBag.Error;
                return View(model);
            }
            return View();
        }

        //// GET: ErrorController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ErrorController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ErrorController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ErrorController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ErrorController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ErrorController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ErrorController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
