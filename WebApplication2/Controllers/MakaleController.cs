using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class MakaleController : YetkiliController
    {
        
        BlogDB db = new BlogDB();
        public ActionResult Index()
        {
            var makaleler = db.Makale.ToList();
            return View(makaleler);
        }

        // GET: Makale/Details/5
        public ActionResult Details(int id)
        {
            var makale = db.Makale.Where(i => i.id == id).SingleOrDefault();
            return View(makale);
        }

        public ActionResult KisiMakaleListele()
        {

            var kullaniciadi = Session["username"].ToString();
            var makaleler = db.Kullanici.Where(a => a.KullaniciAdi == kullaniciadi).SingleOrDefault().Makale.ToList();
            return View(makaleler);
        }

        // GET: Makale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Katagori, "KategoriId", "KategoriAdi");
            return View();
        }

        // POST: Makale/Create
        [HttpPost]
        public ActionResult Create(Makale model)
        {
            try
            {
                string kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanici.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
                model.KullaniciId = kullanici.id;
                model.Tarih = DateTime.Now;
                db.Makale.Add(model);
                db.SaveChanges();


                return RedirectToAction("Index","Kullanici");
            }
            catch
            {
                return View();
            }
        }

        // GET: Makale/Edit/5
        public ActionResult Edit(int id)
        {
            string kullaniciadi = Session["username"].ToString();
            var makale = db.Makale.Where(i => i.id==id).SingleOrDefault();
            if(makale==null)
            {
                return HttpNotFound();

            }
            if(makale.Kullanici.KullaniciAdi==kullaniciadi)
            {
                ViewBag.KategoriId = new SelectList(db.Katagori, "KategoriId", "KategoriAdi");
                return View(makale);
            }
            return HttpNotFound();
            
        }

        // POST: Makale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Makale model)
        {
            try
            {
                var makale = db.Makale.Where(i => i.id == id).SingleOrDefault();
                makale.Baslik = model.Baslik;
                makale.icerik = model.icerik;
                makale.KategoriId = model.KategoriId;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

   
        
        public ActionResult Delete(int id)
        {
            try
            {
                var kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanici.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
              
                var makale = db.Makale.Where(i => i.id == id).SingleOrDefault();
                if(kullanici.id==makale.KullaniciId)
                { 
                    foreach(var i in makale.Yorum.ToList())
                    {
                        db.Yorum.Remove(i);
                      //  db.SaveChanges();
                    }
                    foreach (var i in makale.Etiket.ToList())
                    {
                        db.Etiket.Remove(i);
                      //  db.SaveChanges();
                    }
                    db.Makale.Remove(makale);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Makale silinemedi" });
            }
            catch
            {
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Makale silinemedi" });
            }
        }

        public JsonResult YorumYap(string yorum, int Makaleid)
        {
            var kullaniciadi = Session["username"].ToString();
            var kullanici = db.Kullanici.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();

            

            db.Yorum.Add(new Yorum { KullaniciId = kullanici.id, MakaleId = Makaleid, Tarih = DateTime.Now, YorumIcerik = yorum });
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult YorumDelete(int id)
        {
            try
            {
                var kullaniciadi = Session["username"].ToString();
                var kullanici = db.Kullanici.Where(i => i.KullaniciAdi == kullaniciadi).SingleOrDefault();
                var yorum = db.Yorum.Where(i => i.id == id).SingleOrDefault();
                var makale = db.Makale.Where(i => i.id == yorum.MakaleId).SingleOrDefault();
                if (yorum == null)
                {
                    return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Yorum bulunamadı" });
                }
                if (OrtakSinif.DeleteIzinYetkiVarmi(id, kullanici) || makale.KullaniciId == kullanici.id)
                {
                    db.Yorum.Remove(yorum);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Makale", new { id = yorum.MakaleId });

                }
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Yorum silinemedi" });
            }
            catch
            {
                return RedirectToAction("Hata", "Yetkili", new { yazilacak = "Yorum silinemedi" });

            }

           
           
            
        }
    }
}
