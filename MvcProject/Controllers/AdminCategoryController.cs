using BusinessLayer.Concrete;
using BusinessLayer.ValidationsRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.Entity_Framework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class AdminCategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        public ActionResult Index()
        {
            var categoryValues = cm.GetList();
            return View(categoryValues);
        }
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(Category p)
        {
            CategoryValidatior categoryvalidator = new CategoryValidatior();
            ValidationResult results = categoryvalidator.Validate(p);
            if (results.IsValid)
            {
                cm.CategoryAdd(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
                return View();
        }
        public ActionResult DeleteCategory(int id)
        {
            var categoryvalue = cm.GetByID(id);
            cm.CategoryDelete(categoryvalue);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            var categoryvalue = cm.GetByID(id);
            return View(categoryvalue);
        }
        [HttpPost]
        public ActionResult EditCategory(Category p)
        {
            cm.CategoryUpdate(p);
            return RedirectToAction("Index");
        }
        public ActionResult CategoryCount()
        {
            var kategoriler = cm.GetList(); // cm = CategoryManager

            ViewBag.ToplamKategori = kategoriler.Count(); // Toplam sayıyı alıyoruz
            return View();
        }
        public ActionResult CategoryPanel()
        {
            var kategoriler = cm.GetList(); // cm = CategoryManager

            ViewBag.ToplamKategori = kategoriler.Count(); // Toplam sayıyı alıyoruz

            using (Context c = new Context())
            {
                int yazilimBaslikSayisi = c.Headings
                    .Count(h => h.Category.CategoryName == "Yazılım");

                ViewBag.YazilimBaslikSayisi = yazilimBaslikSayisi;
            }
            using (Context c = new Context())
            {
                int aIcerenYazarSayisi = c.Writers
                    .Count(w => w.WriterName.ToLower().Contains("a"));

                ViewBag.AliYazarSayisi = aIcerenYazarSayisi;
            }
            using (Context c = new Context())
            {
                var enFazlaBaslikliKategori = c.Headings
                    .GroupBy(h => h.Category.CategoryName)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault();

                ViewBag.EnFazlaBaslikliKategori = enFazlaBaslikliKategori;
            }
            using (Context c = new Context())
            {
                int aktif = c.Categories.Count(x => x.CategoryStatus == true);
                int pasif = c.Categories.Count(x => x.CategoryStatus == false);
                int fark = Math.Abs(aktif - pasif); // Mutlak değer alınır

                ViewBag.AktifPasifFark = fark;
            }

            return View();
        }


    }
}