using System;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class ElectionsController : MainController
    {
        // GET: Elections
        public ActionResult Index()
        {
            if (ElectionVM.ElectionFinished())
            {
                return RedirectToAction("ElectionResult");
            }
            if (Session["UserData"] != null)
            {
                var obj = (AccountVM)Session["UserData"];
                if (obj.IsVoted)
                {
                    return RedirectToAction("Soon");
                }
            }
            var list = new CandidateVM().GetAll(true);
            return View(list);
        }
        // GET: Elections
        public ActionResult ElectionResult()
        {
            if (ElectionVM.ElectionFinished())
            {
                var list = new CandidateVM().GetAll(false);
                return View(list);
            }
            var obj = (AccountVM)Session["UserData"];
            if (obj.IsVoted)
            {
                return RedirectToAction("Soon");
            }
            return RedirectToAction("Index");
        }
        // GET: Elections
        public ActionResult Soon()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddVote(Guid CandidateID)
        {
            var obj = (AccountVM)Session["UserData"];
            var result = new ElectionVM().AddElection(obj.ID, CandidateID);
            if(result == 1)
            {
                obj.IsVoted = true;
                Session["UserData"] = obj;
            }
            return Json(new { Code = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddDate(DateTime ElecDate)
        {
            var result = new ElectionVM().AddEditElectionDate(ElecDate);
            return Json(new { Code = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearData()
        {
            var result = new AccountVM().ClearAllData();
            return Json(new { Code = result }, JsonRequestBehavior.AllowGet);
        }
        
    }
}