using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.Utilities;

namespace Vidly.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        //Enabled Output cache action filter for 50 seconds 
        //When we apply an action filter it will be applied by 
        //MVC framework before and after the execution of MVC action 
        //depending on how it is implmented
        [OutputCache (Duration=50)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            throw new Exception();
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //Single file upload 
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Upload"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            ViewBag.imagelst = _context.MovieImages.ToList();
            return View();
        }

        public ActionResult MultipleFilesUpload(IEnumerable<HttpPostedFileBase> files)
        {
            if (files.Count() > 0)
            {
                try
                {
                    List<string> fileList = new List<string>();
                    //traverse through each file and save it destination folder
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string path = Path.Combine(Server.MapPath("~/Upload"),
                            Path.GetFileName(file.FileName));
                            file.SaveAs(path);
                            fileList.Add(path);
                        }
                    }
                    ViewBag.Message = "Following file(s) uploaded successfully";
                    ViewBag.FilesSaved = fileList;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            
            return View("UploadFile");

        }
        [HttpPost]
        public ActionResult UploadImageToDb(HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                try
                {
                    using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                    {
                        bytes = br.ReadBytes(postedFile.ContentLength);
                    }

                    _context.MovieImages.Add(new MovieImage
                    {
                        Name = Path.GetFileName(postedFile.FileName),
                        Description = postedFile.ContentType,
                        MovieImg = bytes
                    });
                    _context.SaveChanges();
                    ViewBag.Message = "One file saved to database";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            
            return RedirectToAction("UploadFile");
        }


    }
}