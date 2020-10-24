using Optica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Optica.Controllers
{
    [Authorize]
    public class ActeMedicalController : Controller
    {
        private AppDbContext ctx = new AppDbContext();
        private DTraitement Fchaine = new DTraitement();
        
        [Route("acte-medical/", Name = "_listeActeMedical")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("acte-medical/add/", Name = "_AddActeMedical")]
       public ActionResult Add(ActeMedical act)
       {
           if (Request.HttpMethod == "POST")
           {
               if (ModelState.IsValid)
               {
                   var act2 = new ActeMedical { Code = act.Code.Trim(), Designation = Fchaine.Title(act.Designation.Trim()), DateEnreg = DateTime.Now };
                   ctx.ActeMedicals.Add(act2);
                   ctx.SaveChanges();
               }
               
           }
           ViewBag.acteMedicales = ctx.ActeMedicals.Where(x => x.Del == false).ToList();
            return View(new ActeMedical());
       }

        [Route("acte-medical/{id}/", Name = "_updateActeMedical")]
        public ActionResult Update(int id, ActeMedical act)
        {
            var act2 = ctx.ActeMedicals.Find(id);
            if (act2 != null)
            {
                if (Request.HttpMethod == "POST")
                {
                    if (ModelState.IsValid)
                    {
                        act2.Code = act.Code.Trim();
                        act2.Designation = Fchaine.Title(act.Designation.Trim());
                        ctx.SaveChanges();
                    }
                }
                ViewBag.acteMedicales = ctx.ActeMedicals.Where(x => x.Del == false).ToList();
                return View("Add", act2);
            }
            return  RedirectToAction("Add");
        }
	}
}