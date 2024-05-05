//using FullDB.Data;
//using GuitArtistsWeb.Models;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using static GuitArtistsWeb.Helpers.ClaimUpdateHelper;

//namespace GuitArtistsWeb.Controllers
//{
//    public class ExistingLoginController : Controller
//    {
//        private readonly AppDbContext _context;

//        public ExistingLoginController(AppDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index()
//        {
//            if (TempData["ExistingLogin"] == null || !(bool)TempData["ExistingLogin"])
//                return RedirectToAction("Index", "PageNotFound");

//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> IndexAsync(ExistingLoginModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
//                user.Login = model.Login;
//                _context.SaveChanges();

//                var identity = (ClaimsIdentity)User.Identity;
//                ClaimsPrincipal newPrincipal = new ClaimsPrincipal(GetNewIdentity(identity, ClaimTypes.Name, model.Login));
//                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);

//                return RedirectToAction("Index", "Home");
//            }

//            return View("~/Views/ExistingLogin/index.cshtml", model);
//        }
//    }
//}
