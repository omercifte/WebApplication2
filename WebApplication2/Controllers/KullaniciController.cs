using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class KullaniciController : Controller
    {
        BlogDB db = new BlogDB();

        // GET: Kullanici
        public ActionResult Index()
        {
            return View();
        }

        // GET: Kullanici/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Kullanici model)

        {
            try
            {
                var varmi = db.Kullanici.Where(i => i.KullaniciAdi == model.KullaniciAdi).SingleOrDefault();
                if(varmi==null)
                {
                    return View();
                }
                if  (varmi.Sifre == model.Sifre)
                {
                    Session["username"] = model.KullaniciAdi;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Kullanici/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kullanici/Create
        [HttpPost]
        public ActionResult Create(Kullanici model )
        {
            try
            {
                var varmi = db.Kullanici.Where(i => i.KullaniciAdi == model.KullaniciAdi).SingleOrDefault();

                if(varmi!=null)
                {
                    return View();
                }

                if (string.IsNullOrEmpty(model.Sifre))
                {
                    return View();
                }
                db.Kullanici.Add(model);
                db.SaveChanges();

                Session["username"] = model.KullaniciAdi;

                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["usernam"] = null;
            return RedirectToAction("Index", "Home");
        }
        // GET: Kullanici/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Kullanici/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Kullanici/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kullanici/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
