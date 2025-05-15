using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PJ_Source_GV.Models
{
    public class SinhVien
    {
        public string MSSV { get; set; } 
        public bool IsQuaTienDoDaoTao { get; set; }
        public string HoLot { get; set; }
        public string Ten { get; set; }
        public string HoTen {
            get
            {
                return HoLot + " " + Ten;
            }
        }
        public string HoTen_En
        {
            get
            {
                return Ten + " " + HoLot;
            }
        }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string sNgaySinh
        {
            get
            {
                return NgaySinh.ToString("dd/MM/yyyy");
            }
        }
        public string NamSinh { get; set; }
        public string NoiSinh { get; set; }
        public int ClassID { get; set; }
        public string Lop { get; set; }
        public string Nganh { get; set; }
        public string Khoa { get; set; }
        public string HeDaoTao { get; set; }
        public string NganhID { get; set; }
        public string KhoaID { get; set; }
        public string HeDaoTaoID { get; set; }
        public string DiaChiLienLac { get; set; }
        public string HKSoNha { get; set; }
        public string HKSoNhaDuongPhuong { get; set; }
        public string HKPhuongXaThiTran { get; set; }
        public string HKQuan { get; set; }
        public string HKTP { get; set; }
        public string HKFull
        {
            get
            {
                return HKSoNha + ", " + HKSoNhaDuongPhuong + ", " + HKPhuongXaThiTran + ", " + HKQuan + ", " + HKTP;
            }
        }
        public string DienThoaiNha { get; set; }
        public string DienThoaiDiDong { get; set; }
        public string DanToc { get; set; }
        public string TonGiao { get; set; }
        public List<string> DienChinhSach { get; set; }
        public string DienChinhSach1 { get; set; }
        public string DienChinhSach2 { get; set; }
        public string DienChinhSach3 { get; set; }
        public string DienChinhSach4 { get; set; }
        public DateTime NgayVaoDoan { get; set; }
        public string NgayVaoDoan_ChucVuCaoNhat { get; set; }
        public DateTime NgayVaoDang { get; set; }
        public string NgayVaoDang_ChiBo { get; set; }
        public DateTime NgayVaoHoiSV { get; set; }
        public DateTime NgayNhapNgu { get; set; }
        public DateTime NgayXuatNgu { get; set; }
        public int SoNamThamGiaBoDoi { get; set; }
        public int SoNamThamGiaThanhNienXungPhong { get; set; }
        public DateTime NgayNgoaiTru { get; set; }
        public string PhongKTX { get; set; }
        public int SoAnhChiEmRuot { get; set; }
        public byte[] Hinh { get; set; }
        public int NamVaoTruong_DauTien { get; set; }
        public int NamVaoTruong { get; set; }
        public string CMND { get; set; }
        public DateTime NgayCapCMND { get; set; }
        public string NoiCapCMND { get; set; }
        public string SoBaoDanhTHPT { get; set; }
        public string HePhoThong { get; set; }
        public string DiemTrungTuyen { get; set; }
        public string SoBaoDanh { get; set; }
        public string NganhDuThi { get; set; }
        public string MaNganhDuThi { get; set; }
        public string DoiTuong { get; set; }
        public string KhuVuc { get; set; }
        public string HoTenCha { get; set; }
        public int NamSinhCha { get; set; }
        public string NgheCha { get; set; }
        public string DiaChiCha { get; set; }
        public string DTCha { get; set; }
        public string CoQuanCha { get; set; }
        public string DTCoQuanCha { get; set; }
        public string HoTenMe { get; set; }
        public string DiaChiMe { get; set; }
        public string NgheMe { get; set; }
        public int NamSinhMe { get; set; }
        public string DTMe { get; set; }
        public string CoQuanMe { get; set; }
        public string DTCoQuanMe { get; set; }
        public string HoTenVoChong { get; set; }
        public int NamSinhVoChong { get; set; }
        public string NgheVoChong { get; set; }
        public string DTVoChong { get; set; }
        public string CoQuanVoChong { get; set; }
        public string DTCoQuanVoChong { get; set; }
        public DateTime LanThayDoiCuoi { get; set; }
        public int SoLanChoPhepCapNhat { get; set; }
        public string Lop10 { get; set; }
        public string Lop11 { get; set; }
        public string Lop12 { get; set; }
        public string NangKhieu { get; set; }
        public string DoiTuongBanThan { get; set; }
        public string NgheNghiepBanThan { get; set; }
        public string NoiCongTacBanThan { get; set; }
        public string DienThoaiCoQuanBanThan { get; set; }
        public DateTime NgayTotNghiepTHPT { get; set; }
        public string LoaiVanBangTHPT { get; set; }
        public string ThamTraVanBang { get; set; }
        public string NoiCapVanBangTHPT { get; set; }
        public string SoVanBangTNPT { get; set; }
        public DateTime ThoiGianVaoTruong { get; set; }
        public string HoiDongThiTNPT { get; set; }
        public bool PhaiDoiMatKhau { get; set; }
        public string SoVaoSoVanBangTHPT { get; set; }
        public bool IsActiveZ { get; set; }
        public string LopCN2 { get; set; }
        public string HoChieu { get; set; }
        public string QuocTich { get; set; }
        public DateTime VISA_NgayBatDau { get; set; }
        public DateTime VISA_NgayKetThuc { get; set; }
        public string Lop09 { get; set; }
        public string Theme { get; set; }
        public string Add_SoNha { get; set; }
        public string Add_DuongPhuong { get; set; }
        public string Add_PhuongXaThiTran { get; set; }
        public string Add_QuanHuyen { get; set; }
        public string Add_TinhTP { get; set; }
        public byte[] MaVach { get; set; }
        public string EmailKhac { get; set; }
        public DateTime ThoiGianRaTruong { get; set; }
        public DateTime ThoiGianRaTruongToiDa { get; set; }
        public string CMND_SoNha { get; set; }
        public string CMND_DuongPhuong { get; set; }
        public string CMND_PhuongXaThiTran { get; set; }
        public string CMND_QuanHuyen { get; set; }
        public string CMND_TinhTP { get; set; }
        public string Email { get; set; }
        public string HoanCanhGiaDinhTomTat { get; set; }
        public string Lop12_HocTap_DTB { get; set; }
        public string Lop12_HocTap_XL { get; set; }
        public string Lop12_RenLuyen_DTB { get; set; }
        public string Lop12_RenLuyen_XL { get; set; }
        public string CanSuLop { get; set; }
        public string CourseID { get; set; }
        public string LopCN2_NganhID { get; set; }
        public string LopCN2_Nganh { get; set; }
        public string LopCN2_KhoaID { get; set; }
        public string LopCN2_Khoa { get; set; }
        public string LopCN2_HeDaoTaoID { get; set; }
        public string LopCN2_HeDaoTao { get; set; }
        public string LopCN2_CourseID { get; set; }
        public string TinhTrangVaoRaID { get; set; }
        public int LopCN2_NamVaoTruong { get; set; }
        public string Lop11_HocTap_DTB { get; set; }
        public string Lop11_HocTap_XL { get; set; }
        public string Lop11_RenLuyen_DTB { get; set; }
        public string Lop11_RenLuyen_XL { get; set; }
        public string Lop10_HocTap_DTB { get; set; }
        public string Lop10_HocTap_XL { get; set; }
        public string Lop10_RenLuyen_DTB { get; set; }
        public string Lop10_RenLuyen_XL { get; set; }
        public string EmailCha { get; set; }
        public string EmailMe { get; set; }
        public string EmailNguoiThan { get; set; }
        public string EmailNguoiThan_QuanHe { get; set; }
        public string CanSuLop_ThuocTo { get; set; }
        public string GiaCanh_LamThem_ViTri { get; set; }
        public string GiaCanh_LamThem_NoiLamViec { get; set; }
        public string GiaCanh_NguonThuHocPhi { get; set; }
        public string ChuyenNganh { get; set; }
        public string ChuyenNganhID { get; set; }
        public string NguyenQuan { get; set; }
        public string ACE_MSSVs { get; set; }
        public DateTime ThoiDiemDKMHGanNhat { get; set; }
        public string HocKyCuoi { get; set; }
        public string CanBoDoanCaoNhat { get; set; }
        public string CanBoLopCaoNhat { get; set; }
        public string ChuongTrinhTiengAnhDauNam { get; set; }
        public int DaTungDocThuChucMung { get; set; }
        public string Lop10_HocTap_DTB_HK1 { get; set; }
        public string Lop10_HocTap_DTB_HK2 { get; set; }
        public string Lop11_HocTap_DTB_HK1 { get; set; }
        public string Lop11_HocTap_DTB_HK2 { get; set; }
        public string Lop12_HocTap_DTB_HK1 { get; set; }
        public string Lop12_HocTap_DTB_HK2 { get; set; }
        public string VanBang_Nganh { get; set; }
        public string TruongDaiHoc { get; set; }
        public string Lop12_LHS_QuocGia { get; set; }
        public string Lop12_LHS_TinhTP { get; set; }
        public string Lop12_LHS_Truong { get; set; }
        public string NamTiepNhan { get; set; }
        public string DonViTiepNhan { get; set; }
        public int ThoiDiemDKMHGanNhat_IsSVDK { get; set; }
        public int SoLuotDongBoTinhTrang { get; set; }
        public string TruongDaiHoc_MSSV { get; set; }
        public double TruongDaiHoc_TCTL { get; set; }
        public string PhienBan { get; set; }
        public string ActiveLanguage { get; set; }
        public string BACDAOTAO { get; set; }
        public int ThuTuTrongGD { get; set; }
        public bool IsNguoiHocDHDauTien { get; set; }
        public int SoLuotXemThuChucMung { get; set; }

        #region partial
        public bool IsBoiDuongSDH { get; set; }
        public int NienKhoaID { get; set; }
        public string KhoiLop { get; set; }
        #endregion

        #region Nộp hồ sơ tuyển sinh
        public int[] loaifileinsert { get; set; }
        public string[] lstHsTuyenSinh_TrangThai { get; set; }
    public bool IsDaTimHieuTDTU { get; set; }
        #endregion

        #region Địa điểm tự học
        public bool IsCamDangKyTuHoc { get; set; }
        #endregion

    }
}
