using BusinessLayer.Concrete;
using BusinessLayer.ValidationsRules;
using DataAccessLayer.Entity_Framework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Results;

namespace MvcProject.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCategoryList()
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
            //cm.CategoryAddBL(p);
            CategoryValidatior categoryValidator = new CategoryValidatior();
            ValidationResult results = categoryValidator.Validate(p);
            if (results.IsValid)
            {
                cm.CategoryAdd(p);
                return RedirectToAction("GetCategoryList");
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
    }
}