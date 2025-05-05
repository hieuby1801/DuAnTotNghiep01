using DATN_API.DTOs;
using System;

namespace DATN_API.Service
{
    public interface IThongKeService
    {
        List<ThongKeDoanhThuNgayDTO> ThongKeDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay);
    }
}
