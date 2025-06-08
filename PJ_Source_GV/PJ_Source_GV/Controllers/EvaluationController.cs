using System;
using System.Security.Claims;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Localization;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using PJ_Source_GV.FunctionSupport;
using PJ_Source_GV.Models;
using SSOLibSVCore;
using PJ_Source_GV.Repositories;

namespace PJ_Source_GV.Controllers
{
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    public class EvaluationController : PrivateSVCoreController
    {
        private readonly IStringLocalizer<StandardController> _localizer;
        private readonly IStandardRepository _standardRepository;

        public EvaluationController(IStringLocalizer<StandardController> localizer, IStandardRepository standardRepository)
        {
            _localizer = localizer;
            _standardRepository = standardRepository;
        }

        [StudentLoginCheck]
        public async Task<IActionResult> Index()
        {
            //Info Page
            this.InitBreadCrumbTitle(_localizer["Evaluation"], _localizer["Evaluation"]);
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