using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using Helpers;
using PagedList;
using TongkhosonVn.Models;
using TongkhosonVn.ViewModel;

namespace TongkhosonVn.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataEntities _db = new DataEntities();
        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];

        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                TradeMarks = _db.TradeMarks.Where(x => x.Active && x.FlagHome && x.Avatar != null).ToList(),
                Banners = _db.Banners.Where(x => x.Active && x.Location == LocationEnum.HeroSlide).OrderBy(a => a.DisplayOrder).ToList(),
                LibraryImage = _db.LibraryImages.FirstOrDefault(x => x.Active && x.FlagHome),
                FeedBacks = _db.FeedBacks.Where(x => x.Active && x.FlagHome && x.Avatar != null).ToList()
            };
            return View(model);
        }

        [Route("thuong-hieu/{name}-{tradeMarkId}")]
        public ActionResult ListProduct(int? page, int? productCategoryId, int tradeMarkId)
        {
            var tradeMark = _db.TradeMarks.Find(tradeMarkId);
            if (tradeMark == null || !tradeMark.Active)
            {
                return RedirectToAction("Index");
            }

            var products = _db.Products.Where(x => x.Active && x.TradeMarkID == tradeMarkId);
            var pageNumber = page ?? 1;
            var model = new ProductViewModel
            {
                TradeMark = tradeMark
            };
            if (productCategoryId.HasValue)
            {
                products = products.Where(x => x.ProductCategoryID == productCategoryId);
                model.ProductCategory = _db.Table_ProductCategory.Find(productCategoryId);
            }
            model.Products = products.OrderBy(a => a.DisplayOrder).ToPagedList(pageNumber, 12);
            return View(model);
        }
        [Route("san-pham/{name}-{productCategoryId}", Order = 1)]
        public ActionResult ProductCategory(int? page, int? tradeMarkId, int productCategoryId = 0)
        {
            var category = _db.Table_ProductCategory.Find(productCategoryId);
            if (category == null || !category.Active)
            {
                return RedirectToAction("Index");
            }
            var products = _db.Products.Where(x => x.Active && x.ProductCategoryID == productCategoryId);
            var pageNumber = page ?? 1;

            var model = new ProductViewModel
            {
                ProductCategory = category
            };
            if (tradeMarkId.HasValue)
            {
                products = products.Where(a => a.TradeMarkID == tradeMarkId);
                model.TradeMark = _db.TradeMarks.Find(tradeMarkId);
            }

            model.Products = products.OrderBy(a => a.DisplayOrder).ToPagedList(pageNumber, 12);

            return View(model);
        }
        [Route("san-pham/{name}-{id}.html", Order = 0)]
        public ActionResult ProductDetail(long id)
        {
            if (id == 0) return HttpNotFound();
            var product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (product == null) return HttpNotFound();
            return View(product);
        }
        [Route("tim-kiem")]
        [HttpGet]
        public ActionResult ProductSearch(int? page, int? productCategoryId, string productName)
        {
            var products = _db.Products.Where(x => x.Active).AsQueryable();
            if (productCategoryId.HasValue)
            {
                products = products.Where(a => a.ProductCategoryID == productCategoryId);
            }
            if (!string.IsNullOrEmpty(productName))
            {
                products = products.Where(a => a.Name.Contains(productName));
            }
            ViewBag.Keyword = productName;
            ViewBag.ProductCategoryId = productCategoryId;
            var pageNumber = page ?? 1;
            return View(products.OrderBy(a=>a.DisplayOrder).ToPagedList(pageNumber, 12));
        }

        [Route("mau-son")]
        public ActionResult TableColorPaint(int? page)
        {
            return View(_db.TradeMarks.Where(x => x.Active).OrderBy(x => x.DisplayOrder).ToPagedList(page ?? 1, 12));
        }
        [Route("blog/{url}", Order = 1)]
        public ActionResult ListPost(int? page, string url)
        {
            var category = _db.Table_PostCategory.SingleOrDefault(a => a.Active && a.Url == url);
            if (category == null)
            {
                return RedirectToAction("ListPost");
            }
            var posts = _db.Posts.Where(x => x.Active && (x.PostCategoryID == category.Id || x.PostCategory.ParentCategoryId == category.Id)).OrderByDescending(x => x.CreateDate).ToPagedList(page ?? 1, 12);

            var model = new ListPostViewModel
            {
                Posts = posts,
                PostCategory = category,
                ChildCategories = _db.Table_PostCategory.Where(a => a.ParentCategoryId == category.Id)
            };
            return View(model);
        }
        [Route("blog/{name}-{id}.html", Order = 0)]
        public ActionResult PostDetail(int id)
        {
            var post = _db.Posts.SingleOrDefault(x => x.Id == id);
            if (post == null || !post.Active)
            {
                return RedirectToAction("Index");
            }

            var model = new PostDetailViewModel
            {
                Post = post,
                Posts = _db.Posts.Where(a => a.Active && a.PostCategoryID == post.PostCategoryID && a.CreateDate < post.CreateDate).OrderByDescending(a => a.CreateDate).Take(3)
            };

            return View(model);
        }
        [Route("lien-he")]
        [HttpGet]
        public ActionResult ContactView(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }
        [Route("lien-he")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ContactView(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _db.Contacts.Add(contact);
                _db.SaveChanges();

                SendEmailForContact(contact);

                return RedirectToAction("ContactView", new { result = 1 });
            }
            return View(contact);
        }
        [Route("bao-gia-son")]
        public ActionResult PaintPrice()
        {
            var model = new PaintColor
            {
                tradeMarks = _db.TradeMarks.ToList(),
                Posts = _db.Posts.Where(x => x.Active && x.PostCategory.CategoryName.Contains("Màu sơn")).ToList()
            };
            return View(model);
        }
        [Route("huong-dan-mua-hang")]
        public ActionResult PaymentView()
        {
            return View();
        }
        [Route("dich-vu")]
        public ActionResult ServiceView()
        {
            return View(_db.Posts.Where(x => x.Active && x.PostCategory.CategoryName.Contains("dịch vụ")).ToList());
        }
        [HttpPost]
        public bool Subscribe(string email)
        {
            if (ModelState.IsValid)
            {
                _db.ReceiveEmails.Add(new ReceiveEmail { Email = email });
                _db.SaveChanges();

                //SendEmailForReceiveEmail(email);
                return true;
            }

            return false;
        }
        [Route("dat-hang/{name}-{id}")]
        [HttpGet]
        public ActionResult OrderProduct(long id)
        {
            Product product = _db.Products.SingleOrDefault(x => x.Id == id);
            if (product == null) return HttpNotFound();
            OrderViewModel model = new OrderViewModel
            {
                Product = product,
                Member = new Member(),
                Quantity = 1,
                ProductID = id
            };
            return View(model);
        }
        [HttpPost]
        public JsonResult OrderProduct(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = _db.Products.SingleOrDefault(x => x.Id == model.ProductID);
                model.Member.Active = true;
                _db.Members.Add(model.Member);
                _db.SaveChanges();
                Order order = new Order
                {
                    MemberID = model.Member.Id,
                    Price = product.Price * model.Quantity,
                    CreateDate = DateTime.Now,
                    ProductID = product.Id,
                    Status = false,
                    ShipDate = DateTime.Now.AddDays(10),
                    Quantity = model.Quantity,

                };
                product.Quantity -= model.Quantity;
                _db.Orders.Add(order);
                _db.SaveChanges();
                order.OrderCode = DateTime.Now.ToString("yyyyMMddHHmm") + "C" + order.ID;
                _db.SaveChanges();

                SendEmailForCustomer(order);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [Route("khach-hang-tieu-bieu")]
        public ActionResult TypicalCustomerView()
        {
            return View(_db.TypicalCustomers.Where(x => x.Active && x.Avatar != null).ToList());
        }
        [Route("thu-vien-anh")]
        public ActionResult AlbumImageView()
        {
            return View(_db.LibraryImages.Where(x => x.Active).OrderBy(a=>a.DisplayOrder).ToList());
        }
        [Route("thu-vien-anh/{name}-{id}")]
        public ActionResult AlbumDetail(int id = 0)
        {
            var album = _db.LibraryImages.Find(id);
            if (album == null)
            {
                return RedirectToAction("AlbumImageView");
            }

            var model = new AlumbDetailViewModel
            {
                Album = album,
                Albums = _db.LibraryImages.Where(x => x.Active && album.Id < id).OrderBy(a => a.DisplayOrder).Take(3)
            };

            return View(model);
        }

        #region function Send Email
        private void SendEmailForCustomer(Order order)
        {
            var sb = "<p style='font-size:16px'>Thông tin đơn hàng gửi từ website " + Request.Url?.Host + "</p>";
            sb += "<p>Mã đơn hàng: <strong>" + order.OrderCode + "</strong></p>";
            sb += "<p>Họ và tên: <strong>" + order.Member.FullName + "</strong></p>";
            sb += "<p>Địa chỉ: <strong>" + order.Member.Address + "</strong></p>";
            sb += "<p>Email: <strong>" + order.Member.Email + "</strong></p>";
            sb += "<p>Điện thoại: <strong>" + order.Member.Phone + "</strong></p>";
            sb += "<p>Ngày đặt hàng: <strong>" + order.CreateDate.ToString("dd-MM-yyyy HH:ss") + "</strong></p>";
            sb += "<p>Ngày giao hàng: <strong>" + order.ShipDate.ToString("dd-MM-yyyy") + "</strong></p>";
            sb += "<p>Hình thức giao hàng: <strong>Giao hàng</strong></p>";
            sb += "<p>Thông tin đơn hàng</p>";
            sb += "<table border='1' cellpadding='10' style='border:1px #ccc solid;border-collapse: collapse'>" +
                  "<tr>" +
                  "<th>Ảnh sản phẩm</th>" +
                  "<th>Tên sản phẩm</th>" +
                  "<th>Số lượng</th>" +
                  "<th>Giá tiền</th>" +
                  "<th>Thành tiền</th>" +
                  "</tr>";
            string str = order.Product.Images.Split(';')[0];
            sb += "<tr>" +
                      "<td><img src = 'https://" + Request.Url?.Host + "/images/Products/" + str + "' class='w100' /></td>" +
                      "<td>" + order.Product.Name;

            sb += "</td>" +
                  "<td style='text-align:center'>" + order.Quantity + "</td>" +
                  "<td style='text-align:center'>" + Convert.ToDecimal(order.Product.Price).ToString("N0") + "</td>" +
                  "<td style='text-align:center'>" + Convert.ToDecimal(order.Price).ToString("N0") + " đ</td>" +
                  "</tr>";

            sb += "<tr><td colspan='5' style='text-align:right'><strong>Tổng tiền: " + Convert.ToDecimal(order.Price).ToString("N0") + " đ</strong></td></tr>";
            sb += "</table>";
            sb += "<p>Cảm ơn bạn đã tin tưởng và mua hàng của chúng tôi.</p>";
            Task.Run(() => HtmlHelpers.SendEmail("gmail", "[" + order.OrderCode + "] Đơn đặt hàng từ website Sơn skey", sb, "SonSkey.webPaint@gmail.com", Email, Email, Password, "Đặt Hàng Online", order.Member.Email, "SonSkey.webPaint@gmail.com"));
        }
        private void SendEmailForReceiveEmail(string email)
        {
            var sb = "<p style='font-size:16px'>Thông tin được gửi gửi từ website " + Request.Url?.Host + "</p>";
            sb += "<p>Đến: <strong>" + email + "</strong></p>";
            sb += "<p>Cảm ơn bạn đã tin tưởng chúng tôi</p>";
            Task.Run(() => HtmlHelpers.SendEmail("gmail", "Thông tin được gửi từ website Sơn skey", sb, "SonSkey.webPaint@gmail.com", Email, Email, Password, "Nhận thông tin", email, "SonSkey.webPaint@gmail.com"));
        }
        private void SendEmailForContact(Contact contact)
        {
            var sb = "<p style='font-size:16px'>Thông tin được gửi gửi từ website " + Request.Url?.Host + "</p>";
            sb += "<p>Đến: <strong>" + contact.Email + "</strong></p>";
            sb += "<p>Cảm ơn bạn đã để liên hệ lại cho chúng tôi</p>";
            Task.Run(() => HtmlHelpers.SendEmail("gmail", "Thông tin được gửi từ website Sơn skey", sb, "SonSkey.webPaint@gmail.com", Email, Email, Password, "Thông tin liên hệ", contact.Email, "SonSkey.webPaint@gmail.com"));
        }
        #endregion

        #region Partial
        [ChildActionOnly]
        public PartialViewResult HeaderPartial()
        {
            var model = new HeaderViewModel
            {
                ProductCategories = _db.Table_ProductCategory.Where(x => x.Active && x.ShowMenuVertical).OrderBy(a => a.DisplayOrder),
                PostCategories = _db.Table_PostCategory.Where(a => a.Active).OrderBy(a => a.DisplayOrder).ToList()
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult FooterPartial()
        {
            return PartialView(_db.Table_PostCategory.Where(x => x.Active && x.TypeCategory == TypeCategory.Footer).OrderBy(x => x.DisplayOrder).ToList());
        }
        [ChildActionOnly]
        public PartialViewResult ListTradeMarkPartial()
        {
            return PartialView(_db.TradeMarks.ToList());
        }

        [ChildActionOnly]
        public PartialViewResult ProductCardPartial(Product product)
        {
            return PartialView(product);
        }
        [ChildActionOnly]
        public PartialViewResult ActiveProductForCategoryPartial(TradeMark tradeMark)
        {
            return PartialView(tradeMark);
        }
        [ChildActionOnly]
        public PartialViewResult MenuMobilePartial()
        {
            return PartialView(_db.TradeMarks.Where(x => x.Active).ToList());
        }
        [ChildActionOnly]
        public PartialViewResult ProductDetailPartial(Product product)
        {
            return PartialView(product);
        }
        [ChildActionOnly]
        public PartialViewResult MenuXLPartial()
        {
            var model = new MenuXLViewModel
            {
                TradeMarks = _db.TradeMarks.Where(x => x.Active).ToList(),
                Posts = _db.Posts.Where(x => x.Active && x.FlagHome).OrderBy(x => x.DisPlayOrder).Take(5).ToList()
            };
            return PartialView(model);
        }

        public PartialViewResult LasterPost()
        {
            return PartialView(_db.Posts.Where(x => x.Active && x.FlagHome).Take(15).ToList());
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}