using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLiPhongTro.Data;
using System.Data;
using System;

namespace QuanLiPhongTro.Controllers
{
    public class UserThanhToanController : Controller
    {
        private readonly DBHelper _dbHelper;

        public UserThanhToanController(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public IActionResult Index()
        {
            try
            {
                string email = HttpContext.Session.GetString("Email");
                if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "TaiKhoan");

                ViewBag.StudentID = "2415053122232";

                string queryKhach = $@"
                    SELECT k.MaKhachThue 
                    FROM KhachThue k
                    JOIN TaiKhoan t ON k.MaTaiKhoan = t.MaTaiKhoan
                    WHERE t.Email = '{email}'";

                DataTable dtKhach = _dbHelper.ExecuteQuery(queryKhach);

                if (dtKhach.Rows.Count == 0) return RedirectToAction("Index", "UserDashboard");

                string maKhach = dtKhach.Rows[0]["MaKhachThue"].ToString();

                string queryLichSu = $@"
                    SELECT hd.MaHoaDon, hd.TongTien, hd.HanThanhToan, hd.TrangThai, hd.NgayLap, p.TenPhong
                    FROM HoaDon hd
                    JOIN HopDong h ON hd.MaHopDong = h.MaHopDong
                    JOIN PhongTro p ON h.MaPhong = p.MaPhong
                    WHERE h.MaKhachThue = '{maKhach}'
                    ORDER BY hd.HanThanhToan DESC";

                DataTable dtLichSu = _dbHelper.ExecuteQuery(queryLichSu);

                decimal tongDaThanhToan = 0;
                decimal chuaThanhToan = 0;

                if (dtLichSu != null)
                {
                    foreach (DataRow row in dtLichSu.Rows)
                    {
                        decimal tien = Convert.ToDecimal(row["TongTien"]);
                        string trangThai = row["TrangThai"].ToString() ?? "";

                        if (trangThai.Contains("Đã thanh toán") || trangThai.Contains("ĐÃ THANH TOÁN"))
                        {
                            tongDaThanhToan += tien;
                        }
                        else
                        {
                            chuaThanhToan += tien;
                        }
                    }
                }

                ViewBag.TongDaThanhToan = tongDaThanhToan;
                ViewBag.ChuaThanhToan = chuaThanhToan;

                return View(dtLichSu);
            }
            catch (Exception ex)
            {
                return Content("LỖI SQL TẠI USER: " + ex.Message);
            }
        }
    }
}