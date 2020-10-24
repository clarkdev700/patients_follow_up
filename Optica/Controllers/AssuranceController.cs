using Optica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Optica.Controllers
{
    [Authorize]
    public class AssuranceController : Controller
    {

        private AppDbContext ctx = new AppDbContext();
        
        [Route("assurance/", Name = "_listeAssurance")]
        public ActionResult Index()
        {
            var assurances = ctx.Assurances.Where(x => x.Del == false).ToList();
            ViewBag.asssurances = assurances;
            return PartialView("listeAssurance");
        }

        [Route("assurance/add/", Name = "_addAssurance")]
        public ActionResult Add(Assurance a)
        {
            if (Request.HttpMethod == "POST")
            {
                if (ModelState.IsValid)
                {
                    var a2 = new Assurance { Code = a.Code.Trim(), Nom = a.Nom.Trim().ToUpper(), DateEnreg = DateTime.Now };
                    ctx.Assurances.Add(a2);
                    ctx.SaveChanges();
                } 
                
            }
            var assurances = ctx.Assurances.Where(x => x.Del == false).ToList();
            ViewBag.assurances = assurances;
            return View(new Assurance());
        }

        [Route("assurance/{id}/", Name = "_updateAssurance")]
        public ActionResult Update(int id, Assurance a)
        {
            var a2 = ctx.Assurances.Find(id);
            if (a2 != null)
            {
                if (Request.HttpMethod == "POST")
                {
                    if (ModelState.IsValid)
                    {
                        a2.Code = a.Code.Trim();
                        a2.Nom = a.Nom.Trim().ToUpper();
                        ctx.SaveChanges();
                    }
                }
                var assurances = ctx.Assurances.Where(x => x.Del == false).ToList();
                ViewBag.assurances = assurances;
                return View("Add", a2);
            }
            return RedirectToAction("Add");
        }
	}
}