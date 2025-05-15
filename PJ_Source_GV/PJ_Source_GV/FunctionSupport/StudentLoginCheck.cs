using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SSOLibSVCore;
using PJ_Source_GV.Caption;
using PJ_Source_GV.Models;
using PJ_Source_GV.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PJ_Source_GV.FunctionSupport
{
    public class StudentLoginCheck : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            if ((filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(AnonymousLoginCheck), false).Any())
            {
                await next();
                return;
            }

            PrivateSVCoreController controller = (PrivateSVCoreController)filterContext.Controller;
            var actionResult = await controller.OnLoad();
            if (actionResult != null)
            {
                filterContext.Result = actionResult;
                return;
            }
            if (controller.CurrentUser == null)
            {
                filterContext.Result = new RedirectResult(Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(controller.Request));
                return;
            }


            var identity = (ClaimsIdentity)filterContext.HttpContext.User.Identity;
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            if (identity.FindFirst("MSSV") == null)
            {
                identity.Claims.ToList().ForEach(d => identity.RemoveClaim(d));
                var stuEmail = controller.CurrentUser.MSSV + "@student.tdtu.edu.vn";
                var mssv = controller.CurrentUser.MSSV;
                //SinhVien sv = await SinhVienRes.LayThongTinSinhVien(mssv);

                ////Set ngôn ngữ
                //if (!string.IsNullOrEmpty(sv.ActiveLanguage))
                //{

                //    filterContext.HttpContext.Response.Cookies.Append(
                //        CookieRequestCultureProvider.DefaultCookieName,
                //        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(sv.ActiveLanguage)),
                //        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                //    );
                //}

                //thêm thông tin Authorization
                Claim cl;

                identity.AddClaim(new Claim(ClaimTypes.Email, stuEmail));
                //identity.AddClaim(new Claim(ClaimTypes.Name, sv.HoTen));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Nguyen A"));
                //identity.AddClaim(new Claim(ClaimTypes.Surname, sv.HoTen + "<br/>" + controller.CurrentUser.MSSV, ClaimValueTypes.String));
                identity.AddClaim(new Claim(ClaimTypes.Surname, "Nguyen A" + "<br/>" + controller.CurrentUser.MSSV, ClaimValueTypes.String));

                identity.AddClaim(new Claim(ClaimTypes.Version, controller.CurrentUser.MSSV + " -- " + stuEmail));
                identity.AddClaim(new Claim("MSSV", controller.CurrentUser.MSSV));
                //identity.AddClaim(new Claim("MSSV", sv.MSSV));
                //identity.AddClaim(new Claim("MaKhoa", sv.KhoaID));
                //identity.AddClaim(new Claim("NamTuyenSinh", sv.NamVaoTruong.ToString()));
                //identity.AddClaim(new Claim("MaNganh", sv.NganhID));
                //identity.AddClaim(new Claim("Lop", sv.Lop));
                //identity.AddClaim(new Claim("HeDaoTaoID", sv.HeDaoTaoID));

                if (principal?.Identity != null && principal.Identity.IsAuthenticated)
                {
                    await filterContext.HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal
                    );
                }
                
            }

            var resultContext = await next();
        }
    }
}
