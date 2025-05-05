using DATN_API.DTOs;
using System;

namespace DATN_API.Service
{
    public interface IThongKeService
    {
        List<ThongKeDoanhThuNgayDTO> ThongKeDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay);
        List<ThongKeDoanhThuNgayDTO> ThongKeDoanhThuTheoThang(DateTime tuNgay, DateTime denNgay);
        List<ThongKeDoanhThuNgayDTO> ThongKeDoanhThuTheoNam(DateTime tuNgay, DateTime denNgay);
        List<ThongKeDoanhThuSachDTO> ThongKeDoanhThuTheoSach(DateTime tuNgay, DateTime denNgay);
        List<ThongKeDoanhThuTheLoaiDTO> ThongKeDoanhThuTheoTheLoai(DateTime tuNgay, DateTime denNgay);

    }
}
