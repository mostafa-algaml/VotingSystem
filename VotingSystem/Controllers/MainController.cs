using System.Web.Mvc;
using System.Web.Routing;

namespace VotingSystem.Controllers
{
    public class MainController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string reqLink = Request.RawUrl.ToString().Trim().ToLower();
            if (System.Web.HttpContext.Current.Session["UserData"] == null && !reqLink.Contains("account") && reqLink != "/")
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Index", area = "" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}