using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Helpers;
using PagedList;
using TongkhosonVn.Models;
using TongkhosonVn.ViewModel;

namespace TongkhosonVn.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly DataEntities _db = new DataEntities();

        public ActionResult Index()
        {
            return View();
        }

        #region Image Processing Function
        private string AddImageToFolder(HttpPostedFileBase picture, string imgPath)
        {
            var imgFileName = DateTime.Now.ToFileTimeUtc() + "." + HtmlHelpers.GetExt(picture.FileName);

            if (System.IO.File.Exists(Server.MapPath(imgPath + "/" + imgFileName)))
            {
                System.IO.File.Delete(Server.MapPath(imgPath + "/" + imgFileName));
            }
            picture.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
            return imgFileName;
        }
        private string AddResizeImageToFolder(HttpPostedFileBase picture, string imgPath, int width, int height)
        {
            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(picture.FileName);

            if (System.IO.File.Exists(Server.MapPath(imgPath + "/" + imgFileName)))
            {
                System.IO.File.Delete(Server.MapPath(imgPath + "/" + imgFileName));
            }
            var newImage = Image.FromStream(picture.InputStream);
            var fixSizeImage = HtmlHelpers.FixedSize(newImage, width, height, false);
            HtmlHelpers.SaveJpeg(Server.MapPath(Path.Combine(imgPath, imgFileName)), fixSizeImage, 90);
            return imgFileName;
        }
        public bool CheckExtension(string extension, string extensions)
        {
            var result = false;

            var reg = new Regex(extensions, RegexOptions.Compiled);

            var m = reg.Match(extension.ToLower());
            if (m.Success)
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Banner
        [HttpGet]
        public ActionResult ListBanner()
        {
            return View(_db.Banners.ToList());
        }
        [HttpGet]
        public ActionResult AddOrUpdateBanner(int? id)
        {
            Banner banner = _db.Banners.SingleOrDefault(x => x.Id == id);
            if (id > 0 && banner == null) return HttpNotFound();
            return View(banner);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdateBanner(Banner banner, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {
                if (!UploadImageBanner(banner, picture))
                {
                    ModelState.AddModelError("", @"Hãy chọn ảnh");
                    return View(banner);
                }
                _db.Banners.AddOrUpdate(banner);
                _db.SaveChanges();
                return RedirectToAction(nameof(ListBanner));
            }
            return View(banner);
        }
        private bool CheckImageBanner(Banner banner, HttpPostedFileBase picture)
        {
            if (banner.Id == 0 && picture == null) return false;

            if (banner.Id > 0)
            {
                Banner banner1 = _db.Banners.SingleOrDefault(x => x.Id == banner.Id);
                if (banner1.Image == null && picture == null) return false;
                if (banner1.Image != null)
                {
                    banner.Image = banner1.Image;
                }
            }
            return true;

        }

        private bool UploadImageBanner(Banner banner, HttpPostedFileBase picture)
        {
            if (!CheckImageBanner(banner, picture)) return false;
            if (picture != null)
            {
                var imgPath = "/Images/Banners/" + DateTime.Now.ToString("yyyy/MM/dd");
                HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                if (banner.Image != null)
                {
                    if (banner.Image.Length > 0)
                    {
                        HtmlHelpers.DeleteFile(Server.MapPath("/Images/Banners/" + banner.Image));
                    }
                }
                banner.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + AddResizeImageToFolder(picture, imgPath, banner.Width, banner.Height);
            }
            return true;
        }
        [HttpPost]
        public JsonResult DeleteBanner(int id)
        {
            Banner banner = _db.Banners.SingleOrDefault(x => x.Id == id);
            if (banner == null && id > 0) return Json(false, JsonRequestBehavior.AllowGet);
            if (banner.Image != null)
            {
                HtmlHelpers.DeleteFile(Server.MapPath("/Images/Banners/" + banner.Image));
            }
            _db.Banners.Remove(banner);
            _db.SaveChanges();
            return Json(true);
        }
        //[HttpPost]
        //public JsonResult DeleteImageBanner(int id)
        //{
        //    Banner banner = _db.Banners.SingleOrDefault(x => x.Id == id);
        //    if (banner == null && id > 0) return Json(false, JsonRequestBehavior.AllowGet);
        //    if (banner.Image != null)
        //    {
        //        HtmlHelpers.DeleteFile(Server.MapPath("/Images/Banners/" + banner.Image));
        //    }
        //    banner.Image = null;
        //    _db.SaveChanges();
        //    return Json(true);
        //}

        #endregion

        #region Config
        public ActionResult ListConfigSite()
        {
            return View(_db.ConfigSites.ToList());
        }


        [HttpGet]
        public ActionResult AddOrUpdateConfig(int? id)
        {
            ConfigSite configSite = _db.ConfigSites.SingleOrDefault(x => x.Id == id);
            if (configSite == null && id > 0) return HttpNotFound();
            return View(configSite);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateConfig(ConfigSite configSite, HttpPostedFileBase pictureTop, HttpPostedFileBase pictureBottom)
        {
            if (ModelState.IsValid)
            {

                if (!UploadImageConfig(configSite, pictureTop, pictureBottom))
                {
                    ModelState.AddModelError("", @"Hãy chọn logo");
                    return View(configSite);
                }

                _db.ConfigSites.AddOrUpdate(configSite);
                _db.SaveChanges();

                HttpContext.Application["ConfigSite"] = configSite;

                return RedirectToAction(nameof(ListConfigSite));
            }
            return View(configSite);
        }
        private bool CheckConfig(ConfigSite configSite, HttpPostedFileBase pictureTop, HttpPostedFileBase pictureBottom)
        {
            if (configSite.Id == 0 && (pictureTop == null || pictureBottom == null)) return false;

            if (configSite.Id > 0)
            {
                ConfigSite configSite1 = _db.ConfigSites.SingleOrDefault(x => x.Id == configSite.Id);

                if ((configSite1.LogoBottom == null || configSite1.LogoTop == null) && (pictureTop == null || pictureBottom == null)) return false;
                if (configSite1.LogoBottom != null && configSite1.LogoTop != null)
                {
                    configSite.LogoBottom = configSite1.LogoBottom;
                    configSite.LogoTop = configSite1.LogoTop;
                }
            }
            return true;

        }
        private bool UploadImageConfig(ConfigSite configSite, HttpPostedFileBase pictureTop, HttpPostedFileBase pictureBottom)
        {
            if (!CheckConfig(configSite, pictureTop, pictureBottom)) return false;
            if (pictureTop != null || pictureBottom != null)
            {
                var imgPath = "/Images/ConfigSites/" + DateTime.Now.ToString("yyyy/MM/dd");
                HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                configSite.LogoTop = DateTime.Now.ToString("yyyy/MM/dd") + "/" + AddImageToFolder(pictureTop, imgPath);
                configSite.LogoBottom = DateTime.Now.ToString("yyyy/MM/dd") + "/" + AddImageToFolder(pictureBottom, imgPath);
            }
            return true;
        }
        [HttpPost]
        public JsonResult DeleteCofig(int id)
        {
            ConfigSite configSite = _db.ConfigSites.SingleOrDefault(x => x.Id == id);
            if (configSite == null && id > 0) return Json(false, JsonRequestBehavior.AllowGet);
            if (configSite.LogoTop != null)
            {
                HtmlHelpers.DeleteFile(Server.MapPath("/Images/ConfigSite/" + configSite.LogoTop));
            }
            if (configSite.LogoBottom != null)
            {
                HtmlHelpers.DeleteFile(Server.MapPath("/Images/ConfigSite/" + configSite.LogoBottom));
            }
            _db.ConfigSites.Remove(configSite);
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Contact
        public ActionResult ListContact()
        {
            return View(_db.Contacts.ToList());
        }
        [HttpGet]
        public PartialViewResult ContactDetail(int? id)
        {
            Contact contact = _db.Contacts.SingleOrDefault(x => x.Id == id);
            if (contact == null && id > 0) return null;
            return PartialView(contact);
        }
        [HttpPost]
        public JsonResult DeleteContact(int? id)
        {
            Contact contact = _db.Contacts.SingleOrDefault(x => x.Id == id);
            if (contact == null && id > 0) return Json(false, JsonRequestBehavior.AllowGet);
            _db.Contacts.Remove(contact);
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Subcribe
        public ActionResult ListSubcribe()
        {
            return View(_db.ReceiveEmails.ToList());
        }
        [HttpPost]
        public bool DeleteSubcribe(int id = 0)
        {
            var contact = _db.ReceiveEmails.Find(id);
            if (contact == null)
            {
                return false;
            }
            _db.ReceiveEmails.Remove(contact);
            _db.SaveChanges();
            return true;
        }
        #endregion

        #region Member
        public ActionResult ListMember()
        {
            return View(_db.Members.ToList());
        }
        [HttpGet]
        public PartialViewResult MemberDetail(int id)
        {
            Member member = _db.Members.SingleOrDefault(x => x.Id == id);
            return PartialView(member);
        }
        [HttpPost]
        public JsonResult DeleteMember(int? id)
        {
            Member member = _db.Members.SingleOrDefault(x => x.Id == id);
            if (member == null) return Json(false, JsonRequestBehavior.AllowGet);
            List<Order> orders = _db.Orders.Where(x => x.MemberID == id).ToList();
            _db.Orders.RemoveRange(orders);
            _db.Members.Remove(member);
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FeedBack
        public ActionResult ListFeedBack()
        {
            return View(_db.FeedBacks.ToList());
        }
        [HttpGet]
        public ActionResult AddOrUpdateFeedBack(int? id)
        {
            FeedBack feedBack = _db.FeedBacks.SingleOrDefault(x => x.ID == id);
            if (feedBack == null && id > 0) return HttpNotFound();
            return View(feedBack);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateFeedBack(FeedBack feedBack, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {
                if (!UploadImageFeedBack(feedBack, picture))
                {
                    ModelState.AddModelError("", @"Hãy chọn ảnh");
                    return View(feedBack);
                }
                _db.FeedBacks.AddOrUpdate(feedBack);
                _db.SaveChanges();
                return RedirectToAction(nameof(ListFeedBack));
            }
            return View(feedBack);
        }
        private bool CheckImageFeedBack(FeedBack feedBack, HttpPostedFileBase picture)
        {
            if (feedBack.ID == 0 && picture == null) return false;

            if (feedBack.ID > 0)
            {
                FeedBack feedBack1 = _db.FeedBacks.SingleOrDefault(x => x.ID == feedBack.ID);
                if (feedBack1.Avatar == null && picture == null) return false;
                if (feedBack1.Avatar != null)
                {
                    feedBack.Avatar = feedBack1.Avatar;
                }
            }
            return true;
        }
        private bool UploadImageFeedBack(FeedBack feedBack, HttpPostedFileBase picture)
        {
            bool res = CheckImageFeedBack(feedBack, picture);
            if (res == false) return false;
            if (picture != null)
            {
                var imgPath = "/Images/FeedBacks/" + DateTime.Now.ToString("yyyy/MM/dd");
                HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                if (feedBack.Avatar != null)
                {
                    HtmlHelpers.DeleteFile(Server.MapPath("/Images/FeedBacks/" + feedBack.Avatar));
                }
                feedBack.Avatar = DateTime.Now.ToString("yyyy/MM/dd") + "/" + AddImageToFolder(picture, imgPath);
            }
            return true;
        }

        public JsonResult DeleteImageFeedBack(int? id)
        {
            FeedBack feedBack = _db.FeedBacks.SingleOrDefault(x => x.ID == id);
            if (feedBack == null && id > 0) return Json(false, JsonRequestBehavior.AllowGet);
            if (feedBack.Avatar != null)
            {
                HtmlHelpers.DeleteFile(Server.MapPath("/Images/FeedBacks/" + feedBack.Avatar));
            }
            feedBack.Avatar = null;
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteFeedBack(int? id)
        {
            FeedBack feedBack = _db.FeedBacks.SingleOrDefault(x => x.ID == id);
            if (feedBack == null && id > 0) return Json(false, JsonRequestBehavior.AllowGet);
            if (feedBack.Avatar != null)
            {
                HtmlHelpers.DeleteFile(Server.MapPath("/Images/FeedBacks/" + feedBack.Avatar));
            }
            _db.FeedBacks.Remove(feedBack);
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Post
        public ActionResult ListPost(int? page, string name, TypeCategory? type, int? catId, int? childId)
        {
            var posts = _db.Posts.AsQueryable();

            if (type.HasValue)
            {
                posts = posts.Where(a => a.PostCategory.TypeCategory == type);
            }

            if (childId.HasValue)
            {
                posts = posts.Where(a => a.PostCategoryID == childId || a.PostCategory.ParentCategoryId == childId);
            }
            else if (catId.HasValue)
            {
                posts = posts.Where(a => a.PostCategoryID == catId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                posts = posts.Where(a => a.Subject.Contains(name));
            }

            var pageNumber = page ?? 1;
            var model = new ListPostAdminViewModel
            {
                Posts = posts.OrderByDescending(a => a.CreateDate).ToPagedList(pageNumber, 20),
                CatId = catId,
                ChildId = childId,
                Type = type,
                Name = name
            };

            if (type.HasValue)
            {
                model.CategoryList = new SelectList(_db.Table_PostCategory.Where(a => a.Active && a.TypeCategory == type).OrderBy(a => a.DisplayOrder), "Id", "Name");
            }
            if (catId.HasValue)
            {
                model.ChildList = new SelectList(_db.Table_PostCategory.Where(a => a.Active && a.ParentCategoryId == catId).OrderBy(a => a.DisplayOrder), "Id", "Name");
            }
            return View(model);
        }
        #region Add-Update post
        [HttpGet]
        public ActionResult AddOrUpdatePost(int? id)
        {
            var post = _db.Posts.Find(id);
            if (id > 0 && post == null) return HttpNotFound();

            var model = new AddPostViewModel
            {
                CategoryList =
                    new SelectList(_db.Table_PostCategory.Where(a => a.Active && a.TypeCategory == TypeCategory.News && a.ParentCategoryId == null)
                            .OrderBy(a => a.DisplayOrder), "Id", "CategoryName"),
                Post = post
            };

            if (post != null)
            {
                model.TypeCategory = post.PostCategory.TypeCategory;
                model.CategoryId = post.PostCategoryID;
                model.CategoryList =
                    new SelectList(_db.Table_PostCategory.Where(a => a.Active && a.TypeCategory == post.PostCategory.TypeCategory && a.ParentCategoryId == null)
                        .OrderBy(a => a.DisplayOrder), "Id", "CategoryName");
            }
            if (post?.PostCategory.ParentCategoryId != null)
            {
                model.CategoryId = (int)post.PostCategory.ParentCategoryId;
                model.ChildId = post.PostCategoryID;
                model.ChildList = new SelectList(_db.Table_PostCategory.Where(a => a.Active && a.ParentCategoryId == post.PostCategory.ParentCategoryId)
                    .OrderBy(a => a.DisplayOrder), "Id", "CategoryName");
            }
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdatePost(AddPostViewModel model, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {
                if (!UploadImagePost(model.Post, picture))
                {
                    ModelState.AddModelError("", @"Hãy chọn ảnh!");
                }
                else
                {
                    model.Post.PostCategoryID = model.ChildId ?? model.CategoryId;
                    model.Post.Url = HtmlHelpers.ConvertToUnSign(null, model.Post.Subject);
                    _db.Posts.AddOrUpdate(model.Post);
                    _db.SaveChanges();
                    return RedirectToAction("ListPost");
                }
            }
            if (model.Post != null)
            {
                model.CategoryList =
                    new SelectList(_db.Table_PostCategory.Where(a => a.Active && a.TypeCategory == model.Post.PostCategory.TypeCategory && a.ParentCategoryId == null)
                        .OrderBy(a => a.DisplayOrder), "Id", "CategoryName");
            }
            if (model.CategoryId > 0)
            {
                model.ChildList = new SelectList(_db.Table_PostCategory.Where(a => a.Active && a.ParentCategoryId == model.CategoryId)
                    .OrderBy(a => a.DisplayOrder), "Id", "CategoryName");
            }
            return View(model);
        }

        private bool CheckPost(Post post, HttpPostedFileBase picture)
        {
            if (post.Id == 0 && picture == null) return false;

            if (post.Id > 0)
            {
                Post post1 = _db.Posts.SingleOrDefault(x => x.Id == post.Id);
                if (post1.Image == null && picture == null) return false;

                if (post1.Image != null)
                {
                    post.Image = post1.Image;
                }
            }
            return true;
        }

        private bool UploadImagePost(Post post, HttpPostedFileBase picture)
        {
            if (!CheckPost(post, picture)) return false;
            if (picture != null)
            {
                var imgPath = "/Images/Posts/" + DateTime.Now.ToString("yyyy/MM/dd");
                HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                if (post.Image != null)
                {
                    HtmlHelpers.DeleteFile(Server.MapPath("/Images/Posts/" + post.Image));
                }
                post.Image = DateTime.Now.ToString("yyyy/MM/dd") + "/" + AddImageToFolder(picture, imgPath);
            }
            return true;
        }

        //public ActionResult DeletePost(int id)
        //{
        //    Post post = _db.Posts.SingleOrDefault(x => x.Id == id);
        //    if (id > 0 && post == null) return Json(false, JsonRequestBehavior.AllowGet);
        //    _db.Posts.Remove(post);
        //    _db.SaveChanges();
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region delete-deleteImage post
        public JsonResult DeletePost(int id)
        {
            Post post = _db.Posts.SingleOrDefault(x => x.Id == id);
            if (id > 0 && post == null) return Json(false, JsonRequestBehavior.AllowGet);
            if (post.Image != null)
            {
                HtmlHelpers.DeleteFile(Server.MapPath("/Images/Posts/" + post.Image));
            }
            _db.Posts.Remove(post);
            _db.SaveChanges();
            return Json(true);
        }
        public JsonResult DeleteImagePost(int id)
        {
            Post post = _db.Posts.SingleOrDefault(x => x.Id == id);
            if (id > 0 && post == null) return Json(false, JsonRequestBehavior.AllowGet);
            if (post.Image != null)
            {
                HtmlHelpers.DeleteFile(Server.MapPath("/Images/Posts/" + post.Image));
            }
            post.Image = "";
            _db.SaveChanges();
            return Json(true);
        }
        #endregion

        #endregion

        #region PostCategory
        public ActionResult ListPostCategory()
        {
            return View(_db.Table_PostCategory.OrderBy(a => a.TypeCategory).ThenBy(a => a.DisplayOrder).ToList());
        }
        [HttpGet]
        public ActionResult AddOrUpDatePostCategory(int? id)
        {
            var postCategory = _db.Table_PostCategory.SingleOrDefault(a => a.Id == id);
            if (id > 0 && postCategory == null) return HttpNotFound();
            ViewBag.ParentList = new SelectList(_db.Table_PostCategory.Where(x => x.Active && x.ParentCategoryId == null).OrderBy(x => x.DisplayOrder).ToList(), "Id", "CategoryName");
            return View(postCategory);
        }
        [HttpPost]
        public ActionResult AddOrUpDatePostCategory(PostCategory postCategory)
        {
            if (ModelState.IsValid)
            {
                postCategory.Url = HtmlHelpers.ConvertToUnSign(null, postCategory.CategoryName);
                _db.Table_PostCategory.AddOrUpdate(postCategory);
                _db.SaveChanges();
                return RedirectToAction(nameof(ListPostCategory));
            }
            ViewBag.ParentList = new SelectList(_db.Table_PostCategory.Where(x => x.Active && x.ParentCategory == null).OrderBy(x => x.DisplayOrder).ToList(), "Id", "CategoryName");
            return View(postCategory);
        }
        [HttpPost]
        public JsonResult DeletePostCategory(int catId)
        {
            var postCategory = _db.Table_PostCategory.SingleOrDefault(a => a.Id == catId);
            if (postCategory == null) return Json(false);
            _db.Table_PostCategory.Remove(postCategory);
            _db.SaveChanges();
            return Json(true);
        }

        public JsonResult GetCategoryByType(TypeCategory type)
        {
            var categories = _db.Table_PostCategory.Where(a => a.Active && a.ParentCategoryId == null && a.TypeCategory == type).OrderBy(a => a.DisplayOrder);
            return Json(categories.Select(a => new { a.Id, a.CategoryName }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategoryByParent(int? parentId)
        {
            var categories = _db.Table_PostCategory.Where(a => a.Active && a.ParentCategoryId == parentId).OrderBy(a => a.DisplayOrder);
            return Json(categories.Select(a => new { a.Id, a.CategoryName }), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Product
        [HttpGet]
        public ActionResult ListProduct(int? page)
        {
            return View(_db.Products.OrderBy(x => x.TradeMark.DisplayOrder).ToPagedList(page ?? 1, 15));
        }
        [HttpGet]
        public ActionResult AddOrUpdateProduct(int? id)
        {
            Product product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (id > 0 && product == null) return HttpNotFound();
            ViewBag.ProductCategories = _db.Table_ProductCategory.ToList();
            ViewBag.ListTradeMark = _db.TradeMarks.ToList();
            return View(product);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateProduct(Product product, List<HttpPostedFileBase> pictures)
        {

            if (ModelState.IsValid)
            {
                if (!UploadImageProduct(product, pictures))
                {
                    ModelState.AddModelError("", @"Hãy chọn ảnh và chỉ tối đa 10 tấm, đúng định dạng jpg,png,jpeg,gif");
                    ViewBag.ProductCategories = _db.Table_ProductCategory.ToList();
                    ViewBag.ListTradeMark = _db.TradeMarks.ToList();
                    return View(product);
                }

                product.Url = HtmlHelpers.ConvertToUnSign(null, product.Name);
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction(nameof(ListProduct));
            }
            ViewBag.ProductCategories = _db.Table_ProductCategory.ToList();
            ViewBag.ListTradeMark = _db.TradeMarks.ToList();
            return View(product);
        }
        private bool CheckImageProduct(Product product, List<HttpPostedFileBase> pictures)
        {
            if (product.Id == 0 && pictures.Count() == 0) return false;
            if (product.Id > 0)
            {
                Product product1 = _db.Products.Find(product.Id);
                if (product1.Images == null && pictures.Count() == 0) return false;
                if (product1.Images != null)
                {
                    product.Images = product1.Images;
                }
            }
            return true;
        }
        private bool UploadImageProduct(Product product, List<HttpPostedFileBase> pictures)
        {
            if (!CheckImageProduct(product, pictures)) return false;
            if (product.Id > 0)
            {
                if (product.Images != null)
                {
                    string[] str = product.Images.Split(';');
                    if (str.Length + pictures.Count() > 10) return false;
                }
            }
            if (pictures.Count() > 10) return false;
            foreach (HttpPostedFileBase picture in pictures)
            {
                if (picture != null)
                {
                    if (picture.ContentLength <= (4000 * 1024) && CheckExtension(Path.GetExtension(picture.FileName), ".jpg|.png|.gif|.jpeg"))
                    {
                        var imgPath = "/Images/Products/" + DateTime.Now.ToString("yyyy/MM/dd");
                        HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                        product.Images += DateTime.Now.ToString("yyyy/MM/dd") + "/" + AddImageToFolder(picture, imgPath) + ";";
                    }
                }
            }
            return true;
        }
        [HttpPost]
        public JsonResult DeleteProduct(int? id)
        {
            Product product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (id > 0 && product == null) return Json(false, JsonRequestBehavior.AllowGet);
            if (product.Images != null)
            {
                string[] strs = product.Images.Split(';');
                foreach (string str in strs)
                {
                    if (System.IO.File.Exists(Server.MapPath("/Images/Products/" + str)))
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/Products/" + str));
                    }
                }
            }
            _db.Products.Remove(product);
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult DeleteImageProduct(int productId, int locationImage)
        {
            try
            {
                Product product = _db.Products.SingleOrDefault(x => x.Id == productId);
                if (productId > 0 && product == null) return PartialView();
                if (product.Images != null)
                {
                    string[] strs = product.Images.Split(';');
                    product.Images = "";
                    for (int i = 0; i < strs.Length - 1; i++)
                    {
                        if (i == locationImage)
                        {
                            //if (System.IO.File.Exists(Server.MapPath("/Images/Products/" + strs[i])))
                            //{
                            //    System.IO.File.Delete(Server.MapPath("/Images/Products/" + strs[i]));
                            //}
                        }
                        else
                        {
                            product.Images += strs[i] + ";";
                        }
                    }
                }
                _db.SaveChanges();
                return PartialView(product);
            }
            catch(Exception ex)
            {
                return PartialView(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult CheckFlagHomeProduct(int id)
        {
            Product product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (product.FlagHome == false) product.FlagHome = true;
            else product.FlagHome = false;
            return Json(true);
        }
        [HttpPost]
        public JsonResult CheckActiveProduct(int id)
        {
            Product product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (product.Active == false) product.Active = true;
            else product.Active = false;
            return Json(true);
        }
        #endregion

        #region Productcategory
        [HttpGet]
        public ActionResult ListProductCategory()
        {
            return View(_db.Table_ProductCategory.ToList());
        }
        [HttpGet]
        public ActionResult AddOrUpdateProductCategory(int? id)
        {
            ProductCategory productCategory = _db.Table_ProductCategory.SingleOrDefault(x => x.ID == id);
            if (id > 0 && productCategory == null) return HttpNotFound();
            ViewBag.TradeMark = _db.TradeMarks.ToList();
            return View(productCategory);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateProductCategory(ProductCategory productCategory, List<int> TradeMarkIds)
        {
            if (ModelState.IsValid)
            {
                productCategory.Url = HtmlHelpers.ConvertToUnSign(null, productCategory.Name);

                _db.Table_ProductCategory.AddOrUpdate(productCategory);
                _db.SaveChanges();
                if (TradeMarkIds!=null)
                {
                    AddTagForProductCategory(productCategory.ID, TradeMarkIds);
                }
                return RedirectToAction(nameof(ListProductCategory));
            }
            return View(productCategory);
        }
        private void AddTagForProductCategory(int id, List<int> TradeMarkIds)
        {
            List<TagProductCategory> tagProductCategories = _db.Tag_ProductCategry_TradeMarks.Where(x => x.ProductCategoryID == id).ToList();
            if (tagProductCategories.Count > 0)
            {
                _db.Tag_ProductCategry_TradeMarks.RemoveRange(tagProductCategories);
                _db.SaveChanges();
            }
            foreach (int TradeMarkID in TradeMarkIds)
            {
                TradeMark tradeMark = _db.TradeMarks.SingleOrDefault(x => x.Id == TradeMarkID);
                if (tradeMark != null)
                {
                    TagProductCategory newTag = new TagProductCategory
                    {
                        TradeMarkID = tradeMark.Id,
                        ProductCategoryID = id
                    };
                    _db.Tag_ProductCategry_TradeMarks.Add(newTag);
                }
            }
            _db.SaveChanges();
        }

        [HttpPost]
        public JsonResult DeleteProductCategory(int? id)
        {
            ProductCategory productCategory = _db.Table_ProductCategory.SingleOrDefault(x => x.ID == id);
            if (id > 0 && productCategory == null) return Json(false, JsonRequestBehavior.AllowGet);
            List<Product> products = _db.Products.Where(x => x.ProductCategoryID == id).ToList();
            _db.Products.RemoveRange(products);
            List<TagProductCategory> tags = _db.Tag_ProductCategry_TradeMarks.Where(x => x.ProductCategoryID == productCategory.ID).ToList();
            _db.Tag_ProductCategry_TradeMarks.RemoveRange(tags);
            _db.Table_ProductCategory.Remove(productCategory);
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckFlagHomeProductCategory(int id)
        {
            ProductCategory productCategory = _db.Table_ProductCategory.SingleOrDefault(x => x.ID == id);
            if (productCategory.FlagHome) productCategory.FlagHome = false;
            else productCategory.FlagHome = true;
            _db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult CheckMenuVerticalProductCategory(int id)
        {
            ProductCategory productCategory = _db.Table_ProductCategory.SingleOrDefault(x => x.ID == id);
            if (productCategory.ShowMenuVertical) productCategory.FlagHome = false;
            else productCategory.ShowMenuVertical = true;
            _db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult CheckActiveProductCategory(int id)
        {
            ProductCategory productCategory = _db.Table_ProductCategory.SingleOrDefault(x => x.ID == id);
            if (productCategory.Active) productCategory.Active = false;
            else productCategory.Active = true;
            _db.SaveChanges();
            return Json(false);
        }


        #endregion

        #region TradeMark
        public ActionResult ListTradeMark()
        {
            return View(_db.TradeMarks.ToList());
        }
        [HttpGet]
        public ActionResult AddOrUpdateTradeMark(int? id)
        {
            TradeMark tradeMark = _db.TradeMarks.SingleOrDefault(x => x.Id == id);
            if (tradeMark == null && id > 0) return HttpNotFound();
            ViewBag.ProductCategor = _db.Table_ProductCategory.ToList();
            return View(tradeMark);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateTradeMark(TradeMark tradeMark, HttpPostedFileBase picture, List<int> ProductCategoryID)
        {
            if (ModelState.IsValid)
            {
                if (!UploadeImageTradeMark(tradeMark, picture))
                {
                    ViewBag.ProductCategor = _db.Table_ProductCategory.ToList();
                    ModelState.AddModelError("", @"Hãy chọn ảnh");
                    return View(tradeMark);
                }
                _db.TradeMarks.AddOrUpdate(tradeMark);
                _db.SaveChanges();
                AddTagForTrandeMark(tradeMark.Id, ProductCategoryID);
                return RedirectToAction(nameof(ListTradeMark));
            }
            ViewBag.ProductCategor = _db.Table_ProductCategory.ToList();
            return View(tradeMark);
        }
        private bool CheckImageTradeMark(TradeMark tradeMark, HttpPostedFileBase picture)
        {
            if (tradeMark.Id == 0 && picture == null) return false;
            if (tradeMark.Id > 0)
            {
                TradeMark tradeMark1 = _db.TradeMarks.SingleOrDefault(x => x.Id == tradeMark.Id);
                if (tradeMark1.Avatar != null) tradeMark.Avatar = tradeMark1.Avatar;
            }
            return true;
        }
        private bool UploadeImageTradeMark(TradeMark tradeMark, HttpPostedFileBase picture)
        {
            if (!CheckImageTradeMark(tradeMark, picture)) return false;

            if (picture != null)
            {
                var imgPath = "/Images/TradeMarks/" + DateTime.Now.ToString("yyyy/MM/dd");
                HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                tradeMark.Avatar = DateTime.Now.ToString("yyyy/MM/dd") + "/" + AddImageToFolder(picture, imgPath);
            }
            return true;
        }
        private void AddTagForTrandeMark(int id, List<int> ProductCategoryId)
        {
            List<TagProductCategory> tagProductCategories = _db.Tag_ProductCategry_TradeMarks.Where(x => x.TradeMarkID == id).ToList();
            if (tagProductCategories.Count() > 0)
            {
                _db.Tag_ProductCategry_TradeMarks.RemoveRange(tagProductCategories);
                _db.SaveChanges();
            }
            foreach (int catID in ProductCategoryId)
            {
                ProductCategory productCategory = _db.Table_ProductCategory.SingleOrDefault(x => x.ID == catID);
                if (productCategory != null)
                {
                    TagProductCategory newTag = new TagProductCategory
                    {
                        TradeMarkID = id,
                        ProductCategoryID = productCategory.ID
                    };
                    _db.Tag_ProductCategry_TradeMarks.Add(newTag);
                }
            }
            _db.SaveChanges();
        }

        [HttpPost]
        public JsonResult DeleteTradeMark(int id)
        {
            TradeMark tradeMark = _db.TradeMarks.SingleOrDefault(x => x.Id == id);
            if (tradeMark == null && id > 0) return Json(false, JsonRequestBehavior.AllowGet);
            if (tradeMark.Avatar != null)
            {
                if (System.IO.File.Exists(Server.MapPath("/Images/TradeMarks/" + tradeMark.Avatar)))
                {
                    System.IO.File.Delete(Server.MapPath("/Images/TradeMarkss/" + tradeMark.Avatar));
                }
            }
            List<Product> products = _db.Products.Where(x => x.TradeMarkID == tradeMark.Id).ToList();
            foreach (Product product in products)
            {
                product.TradeMarkID = null;
            }
            List<TagProductCategory> tags = _db.Tag_ProductCategry_TradeMarks.Where(x => x.TradeMarkID == tradeMark.Id).ToList();
            _db.Tag_ProductCategry_TradeMarks.RemoveRange(tags);
            _db.TradeMarks.Remove(tradeMark);
            _db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckFlagHomeTradeMark(int id)
        {
            TradeMark tradeMark = _db.TradeMarks.SingleOrDefault(x => x.Id == id);
            if (tradeMark.FlagHome)
            {
                tradeMark.FlagHome = false;
            }
            else
            {
                tradeMark.FlagHome = true;
            }
            _db.SaveChanges();
            return Json(false);
        }
        [HttpPost]
        public JsonResult CheckActiveTradeMark(int id)
        {
            TradeMark tradeMark = _db.TradeMarks.SingleOrDefault(x => x.Id == id);
            if (tradeMark.Active)
            {
                tradeMark.Active = false;
            }
            else
            {
                tradeMark.Active = true;
            }
            _db.SaveChanges();
            return Json(false);
        }
        #endregion

        #region Typical Customer
        public ActionResult ListTypicalCustomer()
        {
            return View(_db.TypicalCustomers.ToList());
        }
        [HttpGet]
        public ActionResult AddOrUpdateTypicalCustomer(int? id)
        {
            TypicalCustomer customer = _db.TypicalCustomers.SingleOrDefault(x => x.Id == id);
            if (id > 0 && customer == null) return HttpNotFound();
            return View(customer);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdateTypicalCustomer(TypicalCustomer customer, HttpPostedFileBase picture)
        {
            if (ModelState.IsValid)
            {
                if (!UploadeImageTypicalCustomer(customer, picture))
                {
                    ModelState.AddModelError("", @"Hãy chọn ảnh");
                    return View(customer);
                }
                _db.TypicalCustomers.AddOrUpdate(customer);
                _db.SaveChanges();
                return RedirectToAction("ListTypicalCustomer");
            }
            return View(customer);
        }
        private bool CheckImageTypicalCustomer(TypicalCustomer customer, HttpPostedFileBase picture)
        {
            if (customer.Id == 0 && picture == null) return false;
            if (customer.Id > 0)
            {
                TypicalCustomer customer1 = _db.TypicalCustomers.SingleOrDefault(x => x.Id == customer.Id);
                if (customer1.Avatar == null && picture == null) return false;
                if (customer1.Avatar != null) customer.Avatar = customer1.Avatar;
            }
            return true;
        }
        private bool UploadeImageTypicalCustomer(TypicalCustomer customer, HttpPostedFileBase picture)
        {
            if (!CheckImageTypicalCustomer(customer, picture)) return false;
            if (picture != null)
            {
                var imgPath = "/Images/TypicalCustomers/" + DateTime.Now.ToString("yyyy/MM/dd");
                HtmlHelpers.CreateFolder(Server.MapPath(imgPath));

                customer.Avatar = DateTime.Now.ToString("yyyy/MM/dd") + "/" + AddImageToFolder(picture, imgPath);
            }
            return true;
        }
        [HttpPost]
        public JsonResult DeleteTypicalCustomer(int id)
        {
            TypicalCustomer customer = _db.TypicalCustomers.SingleOrDefault(x => x.Id == id);
            if (id > 0 && customer == null) return Json(false, JsonRequestBehavior.AllowGet);
            if (customer.Avatar != null)
            {
                if (System.IO.File.Exists(Server.MapPath("/Images/TypicalCustomers/" + customer.Avatar)))
                {
                    System.IO.File.Delete(Server.MapPath("/Images/TypicalCustomers/" + customer.Avatar));
                }
            }
            _db.TypicalCustomers.Remove(customer);
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckActiveTypicalCustomer(int id)
        {
            TypicalCustomer customer = _db.TypicalCustomers.SingleOrDefault(x => x.Id == id);
            if (customer.Active == false) customer.Active = true;
            else customer.Active = false;
            _db.SaveChanges();
            return Json(true);
        }
        #endregion

        #region Library Image
        public ActionResult ListAlbumImage()
        {
            return View(_db.LibraryImages.OrderBy(x => x.DisplayOrder).ToList());
        }
        [HttpGet]
        public ActionResult AddOrUpdateItemForAlbum(int? id)
        {
            var model = _db.LibraryImages.Find(id);
            if (id > 0 && model == null) return HttpNotFound();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrUpdateItemForAlbum(LibraryImage model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var images = fc["Pictures"];
                model.Avatar = images;
                _db.LibraryImages.AddOrUpdate(model);
                _db.SaveChanges();
                return RedirectToAction("ListAlbumImage");
            }
            return View(model);
        }
        [HttpPost]
        public bool DeleteAlbum(int id)
        {
            var album = _db.LibraryImages.Find(id);
            if (album == null)
            {
                return false;
            }

            _db.LibraryImages.Remove(album);
            _db.SaveChanges();
            return true;
        }

        [HttpPost]
        public JsonResult CheckActiveItemAlbum(int id)
        {
            LibraryImage model = _db.LibraryImages.SingleOrDefault(x => x.Id == id);
            if (model.Active == false) model.Active = true;
            else model.Active = false;
            _db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult CheckFlagHomeItemAlbum(int id)
        {
            LibraryImage model = _db.LibraryImages.SingleOrDefault(x => x.Id == id);
            if (model.FlagHome == false) model.FlagHome = true;
            else model.FlagHome = false;
            _db.SaveChanges();
            return Json(true);
        }
        #endregion

        #region UserRole
        #region Login-logout
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LoginAction()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoginAction(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userRole = _db.UserRoles.FirstOrDefault(x => x.UserName == model.UserName);
                if (userRole == null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập không chính xác.");
                    return View(model);
                }

                if (!HtmlHelpers.VerifyHash(model.PassWord, "SHA256", userRole.PassWord))
                {
                    ModelState.AddModelError("", @"Mật khẩu không chính xác.");
                    return View(model);
                }
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult LogoutAction()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction(nameof(LoginAction));
        }
        #endregion
        private UserRole GetUserRole()
        {
            var ticket = Request.Cookies[FormsAuthentication.FormsCookieName];
            var authenTicket = FormsAuthentication.Decrypt(ticket.Value);
            UserRole UserRole = _db.UserRoles.Where(x => x.UserName == authenTicket.Name).FirstOrDefault();
            return UserRole;
        }

        public ActionResult ListUserRole()
        {
            return View(_db.UserRoles.ToList());
        }

        #region Add-update-delete
        [HttpGet]
        public ActionResult AddOrUpdateUserRole(int? id)
        {
            UserRole UserRole = _db.UserRoles.SingleOrDefault(x => x.Id == id);
            if (id > 0 && UserRole == null) return HttpNotFound();
            if (UserRole != null)
            {
                if (UserRole.UserName == "Admin" || UserRole.UserName == "admin")
                {
                    return RedirectToAction(nameof(ListUserRole));
                }
            }
            return View(UserRole);
        }

        [HttpPost]
        public ActionResult AddOrUpdateUserRole(UserRole UserRole, string confimPass)
        {
            if (UserRole.UserName == "Admin" || UserRole.UserName == "admin")
            {
                ModelState.AddModelError("", @"Không được lấy tên là admin hoặc Admin");
                return View(UserRole);
            }
            if (ModelState.IsValid)
            {
                if (UserRole.PassWord != confimPass)
                {
                    ModelState.AddModelError("", @"Mật khẩu xác nhận không khớp nhau");
                    return View(UserRole);
                }
                UserRole.PassWord = HtmlHelpers.ComputeHash(UserRole.PassWord, "SHA256", null);
                _db.UserRoles.AddOrUpdate(UserRole);
                _db.SaveChanges();
                return RedirectToAction(nameof(ListUserRole));

            }
            return View(UserRole);
        }
        [HttpPost]
        public JsonResult DeleteUserRole(int id)
        {
            UserRole UserRole = _db.UserRoles.SingleOrDefault(x => x.Id == id);
            if (UserRole == null || UserRole.UserName == "Admin")
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            _db.UserRoles.Remove(UserRole);
            _db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Chang PassWord

        [HttpGet]
        public ActionResult ChangePassWord()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassWord(ChangPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserRole UserRole = GetUserRole();
                if (!HtmlHelpers.VerifyHash(model.OldPassWord, "SHA256", UserRole.PassWord))
                {
                    ModelState.AddModelError("", @"Mật khẩu cũ không chính xác!");
                    return View(model);
                }
                if (model.NewPassWord != model.ConfimPassword)
                {
                    ModelState.AddModelError("", @"Mật khẩu mới không khớp!");
                    return View(model);
                }
                UserRole.PassWord = HtmlHelpers.ComputeHash(model.NewPassWord, "SHA256", null);
                _db.UserRoles.AddOrUpdate(UserRole);
                _db.SaveChanges();

                return RedirectToAction(nameof(LoginAction));
            }
            return View(model);
        }
        #endregion
        #endregion

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}