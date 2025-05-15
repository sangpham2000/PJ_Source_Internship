using CAIT.SQLHelper;
using PJ_Source_GV.Caption;
using PJ_Source_GV.FunctionSupport;
using PJ_Source_GV.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PJ_Source_GV.Repositories
{
    public class DanhSachRes
    {
        public static SQLCommand Insert(DanhSach item)
        {
            var connection = new SQLCommand(ConstValue.ConnectionString);
            List<DanhSach> lstItem = new List<DanhSach> { item };

            connection.ExecuteDataTable("DanhSach_Insert", lstItem.ToDataTable<DanhSach>());

            return connection;
        }

        public static SQLCommand Update(DanhSach item)
        {
            var connection = new SQLCommand(ConstValue.ConnectionString);
            List<DanhSach> lstItem = new List<DanhSach> { item };

            connection.ExecuteDataTable("DanhSach_Update", lstItem.ToDataTable<DanhSach>());

            return connection;
        }


        public static SQLCommand Delete(int id)
        {
            var connection = new SQLCommand(ConstValue.ConnectionString);
            object[] value = new object[]
            {
                id
            };

            var result = connection.ExecuteData("DanhSach_DeleteByID", value);

            return connection;
        }

        public static List<DanhSach> GetAll()
        {
            object[] value = { };
            var connection = new SQLCommand(ConstValue.ConnectionString);
            var result = connection.Query<DanhSach>("DanhSach_GetAll", value);
            return result;
        }

        public static DanhSach GetByID(int id)
        {
            object[] value = { id };
            var connection = new SQLCommand(ConstValue.ConnectionString);
            var result = connection.Query<DanhSach>("DanhSach_GetByID", value);
            
            return result.FirstOrDefault();
        }
    }
}
