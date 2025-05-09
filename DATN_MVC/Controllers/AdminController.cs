﻿using DATN_MVC.DTOs;
using DATN_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;

namespace DATN_MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7189/api/");
        }
        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var model = new Modeltong();
            var response = await _httpClient.GetAsync("Sachs/Laysach");
            var response2 = await _httpClient.GetAsync("DangNhaps/LayNguoiDung");
            var response3 = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
            var respone4 = await _httpClient.GetAsync("Sachs/Laynhacungcap");

            if (respone4.IsSuccessStatusCode)
            {
                var json4 = await respone4.Content.ReadAsStringAsync();
                model.nhaCungCaps = JsonConvert.DeserializeObject<List<NhaCungCap>>(json4) ?? new List<NhaCungCap>();
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                model.Saches = JsonConvert.DeserializeObject<List<Sach>>(json) ?? new List<Sach>();
            }

            if (response2.IsSuccessStatusCode)
            {
                var json2 = await response2.Content.ReadAsStringAsync();
                model.NguoiDungs = JsonConvert.DeserializeObject<List<NguoiDung>>(json2) ?? new List<NguoiDung>();
            }

            if (response3.IsSuccessStatusCode)
            {
                var json3 = await response3.Content.ReadAsStringAsync();
                model.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(json3) ?? new List<TheLoai>();
            }

            return View(model); // truyền đầy đủ sang view Admin
        }

        [HttpGet]
        public async Task<IActionResult> ThemSach()
        {
            var modeltong = new Modeltong();
            // Lấy thể loại
            var resTheLoai = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
            if (resTheLoai.IsSuccessStatusCode)
            {
                var theLoaiJson = await resTheLoai.Content.ReadAsStringAsync();
                modeltong.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(theLoaiJson) ?? new List<TheLoai>();
            }
            return View(modeltong);
        }
        public async Task<IActionResult> DanhSach()
        {
            var model = new Modeltong();
            var response = await _httpClient.GetAsync("Sachs/getOnlySach");
            if (response.IsSuccessStatusCode)
            {
                var json= await response.Content.ReadAsStringAsync();
                model.Saches = JsonConvert.DeserializeObject<List<Sach>>(json)?? new List<Sach>();
                return View(model);
            }
            return View();
        }

        public async Task<IActionResult> DanhSachPartial()
        {
            var response = await _httpClient.GetAsync("Sachs/getOnlySach");
            var data = new List<Sach>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<Sach>>(json) ?? new List<Sach>();
            }

            return PartialView("_DanhSachPartial", data);
        }

        [HttpPost]
        public async Task<IActionResult> ThemSach(Modeltong dto, [FromForm] IFormFile HinhAnhFile, [FromForm] string[] TheLoaiDuocChon)
        {
            dto.ThemSachDto.ListTheLoai = string.Join(",", TheLoaiDuocChon);
            if (HinhAnhFile != null && HinhAnhFile.Length > 0)
            {
                // Tạo tên file duy nhất
                var fileName = Path.GetFileName(HinhAnhFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                // Lưu file vào wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await HinhAnhFile.CopyToAsync(stream);
                }

                // Gán tên file vào model
                dto.ThemSachDto.HinhAnh = "~/images/" + fileName;
            }
            var response = await _httpClient.PostAsJsonAsync("Sachs/ThemSach", dto.ThemSachDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["ThongBao"] = "Thêm sách thành công";
                return RedirectToAction("QuanLySach"); // hoặc trang nào em muốn
            }

            // Có thể đọc lỗi từ API nếu cần
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Thêm sách thất bại: {errorContent}");
            
            return RedirectToAction("ThemSach");
        }
        public async Task<IActionResult> QuanLySach()
        {
            var model = new Modeltong();
            var response = await _httpClient.GetAsync("Sachs/getOnlySach");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                model.Saches = JsonConvert.DeserializeObject<List<Sach>>(json) ?? new List<Sach>();
                return View(model);
            }
            return View();
        }
        public async Task<IActionResult> CapNhatSach(int maSach)
        {
            var modeltong = new Modeltong
            {
                ThemSachDto = new ThemSachDto()
            };
            // Lấy thể loại
            var resTheLoai = await _httpClient.GetAsync("Sachs/LayTatCaTheLoai");
            if (resTheLoai.IsSuccessStatusCode)
            {
                var theLoaiJson = await resTheLoai.Content.ReadAsStringAsync();
                modeltong.TheLoais = JsonConvert.DeserializeObject<List<TheLoai>>(theLoaiJson) ?? new List<TheLoai>();
            }

            var response = await _httpClient.GetAsync($"Sachs/SachChiTiet/{maSach}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                modeltong.ThemSachDto = JsonConvert.DeserializeObject<ThemSachDto>(content);                
            }

            return View(modeltong);
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatSach(Modeltong dto, [FromForm] IFormFile? HinhAnhFile, [FromForm] string[] TheLoaiDuocChon)
        {
            dto.ThemSachDto.ListTheLoai = string.Join(",", TheLoaiDuocChon);
            if (HinhAnhFile != null && HinhAnhFile.Length > 0)
            {
                // Tạo tên file duy nhất
                var fileName = Path.GetFileName(HinhAnhFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                // Lưu file vào wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await HinhAnhFile.CopyToAsync(stream);
                }

                // Gán tên file vào model
                if (fileName.Contains("~/images/"))
                {
                    // Đã chứa chuỗi "~/images/"
                    dto.ThemSachDto.HinhAnh = fileName;
                }
                else
                {
                    dto.ThemSachDto.HinhAnh = "~/images/" + fileName;
                }                
            }
            var response = await _httpClient.PostAsJsonAsync("Sachs/CapNhat", dto.ThemSachDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["ThongBao"] = "Cập nhật sách thành công";
                return RedirectToAction("QuanLySach"); // hoặc trang nào em muốn
            }

            // Có thể đọc lỗi từ API nếu cần
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Thêm sách thất bại: {errorContent}");

            return RedirectToAction();
        }
        public IActionResult ThongKe()
        {
            return View();
        }

    }

}
