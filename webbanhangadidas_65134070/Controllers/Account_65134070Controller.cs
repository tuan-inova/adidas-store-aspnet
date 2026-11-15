using System.Linq;
using System.Web.Mvc;
using webbanhangadidas_65134070.Models;

namespace webbanhangadidas_65134070.Controllers
{
    public class Account_65134070Controller : Controller
    {
        private Project_65134070Entities1 db = new Project_65134070Entities1();

        // ===== LOGIN =====
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string matKhau)
        {
            email = (email ?? "").Trim();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(matKhau) ||
                email.Length > 150 || matKhau.Length > 100)
            {
                ViewBag.Error = "Vui lòng nhập email và mật khẩu hợp lệ.";
                return View();
            }

            var user = db.Tai_Khoan
                         .FirstOrDefault(t => t.Email == email && t.MatKhau == matKhau);

            if (user == null || user.TrangThai != true)
            {
                ViewBag.Error = "Sai email hoặc mật khẩu";
                return View();
            }

            Session["MaTK"] = user.MaTK;
            Session["HoTen"] = user.HoTen;
            Session["VaiTro"] = user.VaiTro;

            return RedirectToAction("Index", "Home");
        }

        // ===== LOGOUT =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ===== REGISTER =====
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string hoTen, string email, string matKhau)
        {
            hoTen = (hoTen ?? "").Trim();
            email = (email ?? "").Trim();
            if (string.IsNullOrWhiteSpace(hoTen) || hoTen.Length > 150 ||
                string.IsNullOrWhiteSpace(email) || email.Length > 150 ||
                string.IsNullOrWhiteSpace(matKhau) || matKhau.Length > 100)
            {
                ViewBag.Error = "Vui lòng nhập họ tên, email và mật khẩu hợp lệ.";
                return View();
            }

            try
            {
                if (new System.Net.Mail.MailAddress(email).Address != email)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                ViewBag.Error = "Email không đúng định dạng.";
                return View();
            }

            var check = db.Tai_Khoan.FirstOrDefault(t => t.Email == email);
            if (check != null)
            {
                ViewBag.Error = "Email đã tồn tại";
                return View();
            }

            Tai_Khoan tk = new Tai_Khoan
            {
                HoTen = hoTen,
                Email = email,
                MatKhau = matKhau,
                VaiTro = "NguoiDung",
                TrangThai = true
            };

            db.Tai_Khoan.Add(tk);
            db.SaveChanges();

            Session["MaTK"] = tk.MaTK;
            Session["HoTen"] = tk.HoTen;
            Session["VaiTro"] = tk.VaiTro;

            return RedirectToAction("Index", "Home");
        }
    }
}
