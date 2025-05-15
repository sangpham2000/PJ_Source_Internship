using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CAIT.SQLHelper;
using DCSL.DatabaseFactory;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PJ_Source_GV.Areas.API.Models;
using PJ_Source_GV.Caption;
using PJ_Source_GV.FunctionSupport;
using PJ_Source_GV.Models;
using PJ_Source_GV.Repositories;
using PJ_Source_GV.Services;
using Spire.Pdf.General.Paper.Uof;
using SSOLibCore;

namespace PJ_Source_GV.Controllers
{
    [BreadCrumb(UseDefaultRouteUrl = true, Order = 0)]
    [LecturerLoginCheck]
    public class DanhSachController : PrivateCoreController
    {
        private readonly IStringLocalizer<SharedResources> _sharedLocalizer;

        public DanhSachController(IStringLocalizer<SharedResources> sharedLocalizer)
        {
            _sharedLocalizer = sharedLocalizer;
        }

        public IActionResult Index()
        {
            //Info Page
            this.InitBreadCrumbTitle(_sharedLocalizer["Menu_DanhSach"], _sharedLocalizer["Menu_DanhSach"]);
            var cultureInfo = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;
            var identity = (ClaimsIdentity)HttpContext.User.Identity;

            return View();
        }

        [HttpPost]
        public JsonResult GetDuLieu()
        {
            var filters = Request.Form;

            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var maKhoa = identity.FindFirst("KhoaID").Value;

            var result = DanhSachRes.GetAll();

            var lst = result.AsQueryable();

            if (!string.IsNullOrEmpty(filters["search[value]"]))
            {
                string strSearch = filters["search[value]"].ToString().ToLower();
                lst = lst.Where(x => x.TenVanBan.ToLower().Contains(strSearch)
                || (!string.IsNullOrEmpty(x.SoVanBan) && x.SoVanBan.ToLower().Contains(strSearch))
                || (!string.IsNullOrEmpty(x.TenPhongBan) && x.TenPhongBan.ToLower().Contains(strSearch))
                || (!string.IsNullOrEmpty(x.TenLoaiVanBan) && x.TenLoaiVanBan.ToLower().Contains(strSearch))
                || x.NgayCapNhat.ToString("dd/MM/yyyy").ToLower().Contains(strSearch)
                );
            }

            //Sắp xếp dữ liệu
            if (!string.IsNullOrEmpty(filters["sort"]) && filters["sort"] != "0")
            {
                if (filters["order[0][dir]"] == "desc")
                {
                    lst = lst.OrderByDynamic(filters["sort"], true);
                }
                else
                {
                    lst = lst.OrderByDynamic(filters["sort"], false);
                }
            }

            int total = lst.Count();

            if (!string.IsNullOrEmpty(filters["start"]) || !string.IsNullOrEmpty(filters["length"]))
            {
                int start = int.Parse(filters["start"]);
                int length = int.Parse(filters["length"]);

                //Giá trị mặc định phân trang nếu nhận được yêu cầu nhưng giá trị không hợp lệ
                if (start < 0)
                {
                    start = 0;
                }
                if (length < 1)
                {
                    length = lst.Count();
                }

                lst = lst.Skip(start).Take(length);
            }

            var dataTable = new DataTableJS<DanhSach>
            {
                recordsTotal = total,
                recordsFiltered = total,
                data = lst.ToArray()
            };

            return Json(dataTable);
        }

        [BreadCrumb(Order = 1)]
        public IActionResult Create()
        {
            // Info Page
            this.InitBreadCrumbTitle(_sharedLocalizer["Menu_DanhSach"], _sharedLocalizer["Create"]);
            var cultureInfo = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var lstLoaiQuyetDinh = new List<string>() { "Quyết định", "Tờ trình"};
            ViewBag.ListLoaiQuyetDinh = lstLoaiQuyetDinh.Select(s => new SelectListItem { Text = s, Value = s }).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection filters)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var manv = identity.Claims.FirstOrDefault(o => o.Type == "MNV").Value;
            var tenKhoa = identity.FindFirst("TenKhoa").Value;
            var cultureInfo = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;
            var result = new SQLCommand();
            List<DanhSach> lst = new List<DanhSach>();
            var insert = new DanhSach()
            {
                SoVanBan = filters["SoVanBan"].ToString(),
                TenVanBan = filters["TenVanBan"].ToString(),
                TenLoaiVanBan = filters["LoaiVanBan"].ToString(),
                TenPhongBan = tenKhoa,
                NgayCapNhat = DateTime.Now
            };
            insert.InitLogs(manv, "Item_Insert");
            result = DanhSachRes.Insert(insert);
            return Json(result.result);
        }

        public IActionResult Edit(int id)
        {
            // Info Page
            this.InitBreadCrumbTitle(_sharedLocalizer["Edit"], _sharedLocalizer["Edit"]);
            var cultureInfo = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var manv = identity.Claims.FirstOrDefault(o => o.Type == "MNV").Value;
            var lstLoaiQuyetDinh = new List<string>() { "Quyết định", "Tờ trình" };
            var item = DanhSachRes.GetByID(id);

            ViewBag.ListLoaiQuyetDinh = lstLoaiQuyetDinh.Select(s => new SelectListItem {Text = s, Value = s ,Selected = s == item.TenLoaiVanBan}).ToList();
            ViewBag.Item = item;

            return View();
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection filters)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var manv = identity.Claims.FirstOrDefault(o => o.Type == "MNV").Value;
            var cultureInfo = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture;
            var tenKhoa = identity.FindFirst("TenKhoa").Value;
            var result = new SQLCommand();
            List<DanhSach> lst = new List<DanhSach>();
            var update = new DanhSach()
            {
                ID = int.Parse(filters["ID"].ToString()),
                SoVanBan = filters["SoVanBan"].ToString(),
                TenVanBan = filters["TenVanBan"].ToString(),
                TenLoaiVanBan = filters["LoaiVanBan"].ToString(),
                TenPhongBan = tenKhoa,
                NgayCapNhat = DateTime.Now
            };
            update.InitLogs(manv, "Item_Update");
            result = DanhSachRes.Update(update);
            return Json(result.result);
        }

        public JsonResult Delete(int id)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            var manv = identity.Claims.FirstOrDefault(o => o.Type == "MNV").Value;
            var item = DanhSachRes.GetByID(id);
            var result = new SQLCommand();
            result = DanhSachRes.Delete(id);
            return Json(result.result);
        }
    }
}
