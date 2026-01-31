using CourseService.Interface;
using CourseService.Models;
using CourseWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        // Constructor 注入 Service
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: 註冊頁
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        // POST: 註冊
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ViewModel → UserModel
            var user = new UserModel
            {
                UserName = model.Username,
                Email = model.Email,
                Pwd = model.Password // 明碼，Service 會 hash
            };

            // 呼叫 Service 註冊
            var result = await _userService.UserRegisterAsync(user);

            if (!result)
            {
                // Email 已存在
                ModelState.AddModelError("", "此 Email 已被使用，請換一個。");
                return View(model);
            }

            // 註冊成功 → 導向登入頁
            TempData["SuccessMsg"] = "註冊成功！請登入。";
            return RedirectToAction("Login");
        }

        // GET: 登入頁
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //登入
                var user = await _userService.UserSignAsync(model.UserName, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("system", "帳號或密碼錯誤");
                    return View(model);
                }
                Claim[] claims = new[] {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal));

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpGet]
        public IActionResult ChangePwd()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePwd(ChangePwdViewModel changePwdViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result = await _userService.UserPwdUpdateAsync(
                    new UserPwdReqModel
                    {
                        UserId = Guid.Parse(currentUserId),
                        OldPwd = changePwdViewModel.OldPassword!,
                        NewPwd = changePwdViewModel.Password!
                    });

                if (!result)
                {
                    ModelState.AddModelError("system", "修改密碼失敗");
                    return View(changePwdViewModel);
                }
                return RedirectToAction("Logout");

            }
            return View(changePwdViewModel);


        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ChangeInfo()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var member = await _userService.FindUserAsync(userId);

            var vm = new UserChgInfoViewModel() { Name = member.UserName, Mobile = member.Mobile };
            return View(vm);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeInfo(UserChgInfoViewModel userChgInfoViewModel)
        {
            if (ModelState.IsValid) {
                var result = await _userService.UserInfoUpdateAsync(new UserInfoReqModel()
                {
                    UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Name = userChgInfoViewModel.Name,
                    Mobile = userChgInfoViewModel.Mobile

                });
                ViewBag.Result = result ? "更新成功" : "更新失敗";
            }

        return View(userChgInfoViewModel);
        }
}
    
    }
    

