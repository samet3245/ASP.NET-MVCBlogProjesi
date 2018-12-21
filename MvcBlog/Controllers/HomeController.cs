using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcBlog.Models;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace MvcBlog.Controllers
{
    public class HomeController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Home
        public ActionResult Index(int Page=1) //Varsayılan olarak 1. parametreye gitsin.
        {
            var makale = db.Makales.OrderByDescending(m => m.MakaleId).ToPagedList(Page,5); //Kaçlı sıra
            return View(makale);
        }

        public ActionResult BlogAra(string Ara ,int Page=1)
        {
            var makale = db.Makales.OrderByDescending(m => m.MakaleId).Where(m => m.Baslik.Contains(Ara)).ToPagedList(Page,5);
            return View(makale);
        }

        public ActionResult SonYorumlar()
        {
            var yorums = db.Yorums.OrderByDescending(y => y.Tarih).Take(5);
            return View(yorums);
        }

        public ActionResult PopulerMakaleler()
        {
            var makales = db.Makales.OrderByDescending(y => y.Okunma).Take(5);
            return View(makales);
        }

        public ActionResult KategoriMakale(int id,int Page=1)
        {
            var makaleler = db.Makales.Where(m => m.Kategori.KategoriId == id).OrderByDescending(m=>m.MakaleId).ToPagedList(Page, 5);
            return View(makaleler);
        }

        public ActionResult MakaleDetay(int id)
        {
            var makale = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        public ActionResult Hakkimizda()
        {
            return View();
        }

        public ActionResult Iletisim()
        {
            return View();
        }

        public ActionResult KategoriPartial()
        {
            return View(db.Kategoris.ToList());
        }

        public JsonResult YorumYap(string yorum,int Makaleid)
        {
            var uyeid = Session["uyeid"];
            if(yorum != null)
            {
                db.Yorums.Add(new Yorum { UyeId = Convert.ToInt32(uyeid), MakaleId = Makaleid, Icerik = yorum, Tarih = DateTime.Now });
                db.SaveChanges();
            }
            return Json(false,JsonRequestBehavior.AllowGet);
        }

        public ActionResult YorumSil(int id)
        {
            var uyeid = Session["uyeid"];
            var yorum = db.Yorums.Where(u => u.YorumId == id).SingleOrDefault();
            var makale = db.Makales.Where(m => m.MakaleId == yorum.MakaleId).SingleOrDefault();
            if (yorum.UyeId == Convert.ToInt32(uyeid))
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.MakaleId });
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult OkunmaArttir(int Makaleid)
        {
            var makales = db.Makales.Where(m => m.MakaleId == Makaleid).SingleOrDefault();
            makales.Okunma++;
            db.SaveChanges();
            return View();
        }
    }
}