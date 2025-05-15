using System.Security.Claims;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Localization;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using PJ_Source_GV.FunctionSupport;
using SSOLibSVCore;

namespace PJ_Source_GV.Controllers
{
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    public class CreateStandardController : PrivateSVCoreController
    {
        private readonly IStringLocalizer<CreateStandardController> _localizer;

        public CreateStandardController(IStringLocalizer<CreateStandardController> localizer)
        {
            _localizer = localizer;
        }

        [StudentLoginCheck]
        public async Task<IActionResult> Index()
        {
            //Info Page
            this.InitBreadCrumbTitle(_localizer["CreateStandard"], _localizer["CreateStandard"]);
            var cultureInfo = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;
            string mssv = ((ClaimsIdentity)User.Identity).FindFirst("MSSV").Value;

            return View();
        }

        public new ActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
        
            ActionResult actionResult = base.Logout();
            HttpContext.Session.Clear();
            return actionResult;
        }
        
    }
}