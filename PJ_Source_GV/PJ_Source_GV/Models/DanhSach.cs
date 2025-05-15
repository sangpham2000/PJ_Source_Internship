using System;

namespace PJ_Source_GV.Models
{
    public class DanhSach
    {
        public int ID { get; set; }
        public string TenVanBan { get; set; }
        public string SoVanBan { get; set; }
        public string TenPhongBan { get; set; }
        public string TenLoaiVanBan { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string LastOperator { get; set; }
        public string RegisterDate { get; set; }
        public string LastDate { get; set; }
        public string PG_ID { get; set; }
        public string DataVersion { get; set; }
    }
}
