using LinqToTwitter;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    public class SysAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;

        public SysAdminController(ApplicationDbContext context, UserAuthDbContext authDbContext)
        {
            _context = context;
            _authContext = authDbContext;
        }

        // GET: SysAdminController
        public ActionResult Index()
        {
            ViewBag.OrganizationList = _context.Organizations.ToList();
            return View();
        }


        // GET: SysAdminController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var organization = _context.Organizations.Find(id);

            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = _context.Organizations.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _context.Organizations.Remove(obj);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Category deleted successfully";
            return RedirectToAction("Index");
        }

        // GET: SysAdminController/Create
        public ActionResult Create()
        {
            ViewBag.UserList = _authContext.Users.ToList();
            return View();
        }

        // POST: SysAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<ActionResult> Create(IFormCollection collection, Category obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string OrganizationName = collection["Name"];
                    string ManagerId = collection["ManagerId"];
                    if (OrganizationName == null || ManagerId == null) ViewBag.Error = "We could not create the organization. Please try again.";
                    Organization organization = new();
                    organization.IsActive = true;
                    organization.Name = OrganizationName;
                    organization.ManagerId = ManagerId;
                    var OrgUser = _authContext.Users.Find(ManagerId);
                    if (OrgUser == null) ViewBag.Error = "We could not create the organization. Please try again.";
                    organization.ManagerName = $"{OrgUser.FirstName} {OrgUser.LastName}";
                    var PostedOganization = _context.Organizations.Add(organization);
                    await _context.SaveChangesAsync();
                }
                //Redirect to Error Page
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SysAdminController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var organization = _context.Organizations.Find(id);

            if (organization == null)
            {
                return NotFound();
            }
            ViewBag.UserList = _authContext.Users.ToList();
            return View(organization);
        }

        // POST: SysAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection collection, int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Organization organization = new();
                    if (id != null)
                    {
                        organization = await _context.Organizations.FindAsync(id);
                    }
                    else
                    {
                        ViewBag.Error = "We could not find this organization.";
                        return RedirectToAction(nameof(Index));
                    }
                    string OrganizationName = collection["Name"];
                    string ManagerId = collection["ManagerId"];
                    if (OrganizationName == null || ManagerId == null) ViewBag.Error = "We could not create the organization. Please try again.";
                    if (collection["IsActive"] == "True") organization.IsActive = true;
                    if (collection["IsActive"] != "True") organization.IsActive = false;
                    organization.Name = OrganizationName;
                    organization.ManagerId = ManagerId;
                    var OrgUser = _authContext.Users.Find(ManagerId);
                    if (OrgUser == null) ViewBag.Error = "We could not create the organization. Please try again.";
                    organization.ManagerName = $"{OrgUser.FirstName} {OrgUser.LastName}";
                    _context.Organizations.Update(organization);
                    await _context.SaveChangesAsync();
                    ViewBag.Success = "Organization updated successfully.";
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "There was a problem with the form.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //// GET: SysAdminController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //	return View();
        //}

        //// POST: SysAdminController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //	try
        //	{
        //		return RedirectToAction(nameof(Index));
        //	}
        //	catch
        //	{
        //		return View();
        //	}
        //}
    }
}
