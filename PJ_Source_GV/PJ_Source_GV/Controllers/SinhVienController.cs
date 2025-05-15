using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using PJ_Source_GV.Models;
using PJ_Source_GV.FunctionSupport;
using PJ_Source_GV.Repositories;
using SSOLibSVCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PJ_Source_GV.Controllers
{
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    public class SinhVienController : PrivateSVCoreController
    {
        private readonly IStringLocalizer<SinhVienController> _localizer;

        public SinhVienController(IStringLocalizer<SinhVienController> localizer)
        {
            _localizer = localizer;
        }

        [StudentLoginCheck]
        public async Task<IActionResult> Index()
        {
            //Info Page
            this.InitBreadCrumbTitle(_localizer["HomeMenu1"], _localizer["HomeMenu1"]);
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
