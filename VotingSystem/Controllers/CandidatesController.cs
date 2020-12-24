using System;
using System.IO;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class CandidatesController : MainController
    {
        // GET: Candidates
        public ActionResult Index()
        {
            var list = new CandidateVM().GetAll(true);
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CandidateVM obj)
        {
            obj.ID = Guid.NewGuid();
            if (obj.PhotoFile != null)
            {
                string PhotoFile = "Photo" + DateTime.Now.Ticks + Path.GetExtension(obj.PhotoFile.FileName);
                obj.Photo = "/Content/images/" + PhotoFile;
                PhotoFile = Path.Combine(Server.MapPath("~/Content/images/"), PhotoFile);
                obj.PhotoFile.SaveAs(PhotoFile);
            }
            else
            {
                obj.Photo = "";
            }
            if (obj.ElectoralSymbolFile != null)
            {
                string ElectoralSymbolFile = "Symbol" + DateTime.Now.Ticks + Path.GetExtension(obj.ElectoralSymbolFile.FileName);
                obj.ElectoralSymbol = "/Content/images/" + ElectoralSymbolFile;
                ElectoralSymbolFile = Path.Combine(Server.MapPath("~/Content/images/"), ElectoralSymbolFile);
                obj.ElectoralSymbolFile.SaveAs(ElectoralSymbolFile);
            }
            else
            {
                obj.ElectoralSymbol = "";
            }

            var result = new CandidateVM().AddCandidate(obj);
            if (result == 1)
            {
                if (obj.Photo != "")
                {
                    var photofile = Server.MapPath(obj.Photo);
                    if (System.IO.File.Exists(photofile))
                    {
                        System.IO.File.Delete(photofile);
                    }
                }
                if (obj.ElectoralSymbol != "")
                {
                    var symbolfile = Server.MapPath(obj.ElectoralSymbol);
                    if (System.IO.File.Exists(symbolfile))
                    {
                        System.IO.File.Delete(symbolfile);
                    }
                }
            }
            return Json(new { Code = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(Guid ID)
        {
            var obj = new CandidateVM().GetObj(ID);
            return View(obj);
        }
        [HttpPost]
        public ActionResult Edit(CandidateVM obj)
        {
            string oldphoto = obj.Photo;
            string oldsymbol = obj.ElectoralSymbol;
            if (obj.PhotoFile != null)
            {
                string PhotoFile = "Photo" + DateTime.Now.Ticks + Path.GetExtension(obj.PhotoFile.FileName);
                obj.Photo = "/Content/images/" + PhotoFile;
                PhotoFile = Path.Combine(Server.MapPath("~/Content/images/"), PhotoFile);
                obj.PhotoFile.SaveAs(PhotoFile);
            }
            else
            {
                obj.Photo = "";
            }
            if (obj.ElectoralSymbolFile != null)
            {
                string ElectoralSymbolFile = "Symbol" + DateTime.Now.Ticks + Path.GetExtension(obj.ElectoralSymbolFile.FileName);
                obj.Photo = "/Content/images/" + ElectoralSymbolFile;
                ElectoralSymbolFile = Path.Combine(Server.MapPath("~/Content/images/"), ElectoralSymbolFile);
                obj.ElectoralSymbolFile.SaveAs(ElectoralSymbolFile);
            }
            else
            {
                obj.ElectoralSymbol = "";
            }
            var result = new CandidateVM().EditCandidate(obj);
            if (result == 1)
            {
                if (obj.Photo != "")
                {
                    var photofile = Server.MapPath(oldphoto);
                    if (System.IO.File.Exists(photofile))
                    {
                        System.IO.File.Delete(photofile);
                    }
                }
                if (obj.ElectoralSymbol != "")
                {
                    var symbolfile = Server.MapPath(oldsymbol);
                    if (System.IO.File.Exists(symbolfile))
                    {
                        System.IO.File.Delete(symbolfile);
                    }
                }
            }
            return Json(new { Code = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(Guid ID)
        {
            var result = new CandidateVM().DeleteCandidate(ID);
            return Json(new { Code = result }, JsonRequestBehavior.AllowGet);
        }

    }
}