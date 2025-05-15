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

namespace PJ_Source_GV.Areas.Main.Controllers
{
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [StudentLoginCheck]
    [Area("Main")]
    public class SinhVienDemoController : PrivateSVCoreController
    {
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;
        public SinhVienDemoController(IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index()
        {
            this.InitBreadCrumbTitle(_sharedLocalizer["Menu_SinhVien"], _sharedLocalizer["Menu_SinhVien"]);
            var cultureInfo = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var mssv = identity.FindFirst("MSSV").Value;

            return View();
        }
    }
}
