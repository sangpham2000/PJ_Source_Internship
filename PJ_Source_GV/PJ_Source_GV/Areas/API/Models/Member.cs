using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PJ_Source_GV.Areas.API.Models
{
    public class Member
    {
        public int id { get; set; }
        public int group_id { get; set; }
        public string email { get; set; }
        public string madonvi { get; set; }
        public static string getTableName() { return "lp_groupuser"; }
    }
}
