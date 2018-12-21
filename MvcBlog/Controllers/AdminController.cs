using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcBlog.Models;
using System.Web.Mvc;

namespace MvcBlog.Controllers
{
    public class AdminController : Controller
    {
        mvcblogDB db = new mvcblogDB();
        // GET: Admin
        public ActionResult AdminPage()
        {
            ViewBag.MakaleSayisi = db.Makales.Count();
            ViewBag.YorumSayisi = db.Yorums.Count();
            ViewBag.KategoriSayisi = db.Kategoris.Count();
            ViewBag.UyeSayisi = db.Uyes.Count();    
            return View();
        }
    }
}