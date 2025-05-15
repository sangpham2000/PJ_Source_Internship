using CAIT.SQLHelper;
using PJ_Source_GV.Caption;
using PJ_Source_GV.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PJ_Source_GV.Repositories
{
    public class SinhVienRes
    {
        /// <summary>
        /// lấy thông tin sinh viên theo mã sinh viên
        /// </summary>
        /// <param name="mssv"></param>
        /// <param name="hoten"></param>
        /// <param name="lop"></param>
        /// <param name="nganh"></param>
        /// <param name="khoa"></param>
        /// <param name="dieukien"></param>
        /// <param name="loaidieukien"></param>
        /// <param name="cmnd"></param>
        /// <returns></returns>
        public static async Task<SinhVien> LayThongTinSinhVien(string mssv)
        {
            var SVService = new ServiceReference.Service1Client();
            ServiceReference.SinhVien_LayItemTheoMSSVNhom_Web_CuuSinhVienResponse sV = await SVService.SinhVien_LayItemTheoMSSVNhom_Web_CuuSinhVienAsync("f7b4411394f6c03c0dab534103b5f496", mssv, "DAIHOC");
            string xml = sV.SinhVien_LayItemTheoMSSVNhom_Web_CuuSinhVienResult?.Any1.InnerXml?.ToString();
            xml = (!string.IsNullOrEmpty(xml)) ? xml : "<NewDataSet xmlns=\"\">\r\n </NewDataSet>";

            DataSet ds = new DataSet();
            //Load all xml data in to dataset
            StringReader strReader = new StringReader(xml);
            ds.ReadXml(strReader);

            if (ds.Tables.Count != 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var sinhVien = SQLCommand.MapEmptyString<SinhVien>(dr, false);

                    return sinhVien;
                }
            }

            return null;
        }

        public static async Task<bool> CapNhatActiveLanguage(SinhVien sv)
        {
            var SVService = new ServiceReference.Service1Client();
            ServiceReference.SinhVien_CapNhatActiveLanguageResponse sV = await SVService.SinhVien_CapNhatActiveLanguageAsync("0a0640dffe54cc40245a1d7e81e7f41f", sv.MSSV, sv.ActiveLanguage);

            return sV.SinhVien_CapNhatActiveLanguageResult;
        }
    }
}
