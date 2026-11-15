using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webbanhangadidas_65134070.Filters;
using webbanhangadidas_65134070.Models;

namespace webbanhangadidas_65134070.Controllers
{
    [AdminOnly]
    public class SanPhamAdmin_65134070Controller : Controller
    {
        private Project_65134070Entities1 db = new Project_65134070Entities1();

        // ===================== INDEX =====================
        public ActionResult Index()
        {
            var sanPham = db.San_Pham.Include(s => s.Loai_San_Pham).OrderByDescending(s => s.MaSP);
            return View(sanPham.ToList());
        }

        // ===================== DETAILS =====================
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sanPham = db.San_Pham.Include(s => s.Loai_San_Pham).FirstOrDefault(s => s.MaSP == id);
            if (sanPham == null) return HttpNotFound();
            return View(sanPham);
        }

        // ===================== CREATE =====================
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.Loai_San_Pham, "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(San_Pham sanPham, HttpPostedFileBase fileAnh)
        {
            ValidateProductInput(sanPham, fileAnh);
            if (ModelState.IsValid)
            {
                if (fileAnh != null && fileAnh.ContentLength > 0)
                {
                    string fileName = DateTime.Now.Ticks + "_" + Path.GetFileName(fileAnh.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                    string directory = Server.MapPath("~/Images/");
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                    fileAnh.SaveAs(path);
                    sanPham.HinhAnh = fileName;
                }

                sanPham.TrangThai = sanPham.TrangThai ?? true;
                db.San_Pham.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoai = new SelectList(db.Loai_San_Pham, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // ===================== EDIT =====================
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            San_Pham sanPham = db.San_Pham.Find(id);
            if (sanPham == null) return HttpNotFound();
            ViewBag.MaLoai = new SelectList(db.Loai_San_Pham, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(San_Pham sanPham, HttpPostedFileBase fileAnh)
        {
            ValidateProductInput(sanPham, fileAnh);
            if (ModelState.IsValid)
            {
                var itemDB = db.San_Pham.AsNoTracking().FirstOrDefault(s => s.MaSP == sanPham.MaSP);
                if (itemDB == null) return HttpNotFound();

                if (fileAnh != null && fileAnh.ContentLength > 0)
                {
                    string fileName = DateTime.Now.Ticks + "_" + Path.GetFileName(fileAnh.FileName);
                    string directory = Server.MapPath("~/Images/");
                    Directory.CreateDirectory(directory);
                    string path = Path.Combine(directory, fileName);
                    fileAnh.SaveAs(path);
                    sanPham.HinhAnh = fileName;
                }
                else
                {
                    sanPham.HinhAnh = itemDB.HinhAnh;
                }

                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoai = new SelectList(db.Loai_San_Pham, "MaLoai", "TenLoai", sanPham.MaLoai);
            return View(sanPham);
        }

        // ===================== DELETE (TRANG XÁC NHẬN - QUAN TRỌNG) =====================
       
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sanPham = db.San_Pham.Include(s => s.Loai_San_Pham).FirstOrDefault(s => s.MaSP == id);
            if (sanPham == null) return HttpNotFound();
            return View(sanPham);
        }

        // ===================== DELETE CONFIRMED (XÓA MỀM) =====================
        [HttpPost, ActionName("Delete")] // Đồng bộ tên để Form Post từ trang Delete gọi được
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var sanPham = db.San_Pham.Find(id);
            if (sanPham != null)
            {
                sanPham.TrangThai = false; // Ngừng bán
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ===================== DELETE FOREVER (XÓA VĨNH VIỄN) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        // ===================== DELETE PERMANENT (XÓA VĨNH VIỄN) =====================
        // Đổi tên từ DeleteForever thành DeletePermanent để khớp với Link trên giao diện
        // Đảm bảo hàm này nằm trong class SanPhamAdmin_65134070Controller
        public ActionResult DeletePermanent(int id)
        {
            var sanPham = db.San_Pham.Find(id);
            if (sanPham != null)
            {
                try
                {
                    db.San_Pham.Remove(sanPham);
                    db.SaveChanges();
                    TempData["Success"] = "Đã xóa vĩnh viễn sản phẩm!";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Sản phẩm đã có đơn hàng, không thể xóa vĩnh viễn!";
                }
            }
            return RedirectToAction("Index");
        }

        // ===================== RESTORE =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(int id)
        {
            var sanPham = db.San_Pham.Find(id);
            if (sanPham != null)
            {
                sanPham.TrangThai = true;
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private void ValidateProductInput(San_Pham sanPham, HttpPostedFileBase fileAnh)
        {
            if (string.IsNullOrWhiteSpace(sanPham.TenSP) || sanPham.TenSP.Length > 150)
                ModelState.AddModelError("", "Tên sản phẩm phải có từ 1 đến 150 ký tự.");
            if (!sanPham.Gia.HasValue || sanPham.Gia < 0 ||
                !sanPham.SoLuongTon.HasValue || sanPham.SoLuongTon < 0)
                ModelState.AddModelError("", "Giá và số lượng tồn phải là số không âm.");
            if (!db.Loai_San_Pham.Any(l => l.MaLoai == sanPham.MaLoai))
                ModelState.AddModelError("", "Vui lòng chọn loại sản phẩm hợp lệ.");

            if (fileAnh != null && fileAnh.ContentLength > 0)
            {
                string extension = Path.GetExtension(fileAnh.FileName).ToLowerInvariant();
                string[] allowed = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                if (!allowed.Contains(extension) || fileAnh.ContentLength > 4 * 1024 * 1024)
                    ModelState.AddModelError("", "Chỉ chọn ảnh JPG, PNG, GIF hoặc WebP tối đa 4 MB.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}