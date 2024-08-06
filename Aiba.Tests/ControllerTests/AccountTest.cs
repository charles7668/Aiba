using Aiba.Controllers;
using Aiba.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Aiba.Tests.ControllerTests
{
    [TestClass]
    public class AccountTest
    {
        private bool _fakeAddToRoleResultFlag = true;
        private IdentityError[] _fakeCreateErrors = [];
        private bool _fakeLoginFlag;
        private bool _fakeRoleExistFlag;
        private SignInResult? _fakeSignInResult;

        private ILogger<AccountController> CreateLogger()
        {
            return new Mock<ILogger<AccountController>>().Object;
        }

        private RoleManager<IdentityRole> CreateRoleManager()
        {
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object, Array.Empty<IRoleValidator<IdentityRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object
            );
            roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(new IdentityResult());
            roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync((string _) => _fakeRoleExistFlag);
            return roleManagerMock.Object;
        }

        private SignInManager<IdentityUser> CreateSignInManager(UserManager<IdentityUser> userManager)
        {
            var signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                userManager,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                null!,
                null!,
                null!,
                null!
            );
            signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync((IdentityUser user, string password, bool _, bool _) =>
                {
                    if (user.UserName == "test01" && password == "abc123" && _fakeSignInResult == null)
                        return SignInResult.Success;
                    return _fakeSignInResult ?? SignInResult.Failed;
                });
            signInManagerMock.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => _fakeLoginFlag);

            return signInManagerMock.Object;
        }

        private UserManager<IdentityUser> CreateUserManager()
        {
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<IdentityUser>>().Object, Array.Empty<IUserValidator<IdentityUser>>(),
                Array.Empty<IPasswordValidator<IdentityUser>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<IdentityUser>>>().Object
            );
            var fakeUsers = new List<IdentityUser>
            {
                new()
                {
                    UserName = "exist01",
                    Email = "exist@test.com"
                }
            };
            userManagerMock.Setup(x => x.Users).Returns(fakeUsers.AsQueryable());

            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync((IdentityUser _, string _) =>
                    _fakeAddToRoleResultFlag ? IdentityResult.Success : IdentityResult.Failed());
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync((IdentityUser _, string _) => _fakeCreateErrors.Length == 0
                    ? IdentityResult.Success
                    : IdentityResult.Failed(_fakeCreateErrors));
            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string userName) =>
                {
                    if (userName == "test01")
                        return new IdentityUser
                        {
                            UserName = userName
                        };
                    return null;
                });
            return userManagerMock.Object;
        }

        [TestMethod]
        public async Task TestLogin()
        {
            _fakeSignInResult = null;
            _fakeLoginFlag = false;
            UserManager<IdentityUser> userManager = CreateUserManager();
            SignInManager<IdentityUser> signInManager = CreateSignInManager(userManager);
            RoleManager<IdentityRole> roleManager = CreateRoleManager();
            ILogger<AccountController> logger = CreateLogger();
            var controller = new AccountController(userManager, signInManager, roleManager, logger);
            // test model input error
            controller.ModelState.AddModelError("test-err", "test error");
            IActionResult result = await controller.Login(new LoginInfo
            {
                UserName = "test02",
                Password = "abc123"
            });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            // test user not found
            controller.ModelState.Clear();
            result = await controller.Login(new LoginInfo
            {
                UserName = "test02",
                Password = "abc123"
            });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            // test password wrong
            controller.ModelState.Clear();
            result = await controller.Login(new LoginInfo
            {
                UserName = "test01",
                Password = "abc122"
            });
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));

            var successLoginInfo = new LoginInfo
            {
                UserName = "test01",
                Password = "abc123"
            };
            // test success
            controller.ModelState.Clear();
            result = await controller.Login(successLoginInfo);
            Assert.IsInstanceOfType(result, typeof(OkResult));

            // test already sign in
            _fakeLoginFlag = true;
            result = await controller.Login(successLoginInfo);
            Assert.IsInstanceOfType(result, typeof(OkResult));

            _fakeLoginFlag = false;
            // test different signin result
            _fakeSignInResult = SignInResult.LockedOut;
            result = await controller.Login(successLoginInfo);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
            _fakeSignInResult = SignInResult.TwoFactorRequired;
            result = await controller.Login(successLoginInfo);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
            _fakeSignInResult = SignInResult.NotAllowed;
            result = await controller.Login(successLoginInfo);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task TestLogout()
        {
            UserManager<IdentityUser> userManager = CreateUserManager();
            SignInManager<IdentityUser> signInManager = CreateSignInManager(userManager);
            RoleManager<IdentityRole> roleManager = CreateRoleManager();
            ILogger<AccountController> logger = CreateLogger();
            var controller = new AccountController(userManager, signInManager, roleManager, logger);
            IActionResult result = await controller.Logout();
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task TestRegister()
        {
            _fakeCreateErrors = [];
            _fakeRoleExistFlag = true;
            _fakeAddToRoleResultFlag = true;
            _fakeLoginFlag = false;
            UserManager<IdentityUser> userManager = CreateUserManager();
            SignInManager<IdentityUser> signInManager = CreateSignInManager(userManager);
            RoleManager<IdentityRole> roleManager = CreateRoleManager();
            ILogger<AccountController> logger = CreateLogger();
            var controller = new AccountController(userManager, signInManager, roleManager, logger);

            // failed case
            var validateModel = new RegisterInfo
            {
                UserName = "test01",
                Email = "a@a.com",
                Password = "123456",
                ConfirmPassword = "123458"
            };
            var validateResultList = new List<ValidationResult>();
            bool validateResult = Validator.TryValidateObject(validateModel, new ValidationContext(validateModel),
                validateResultList, true);
            Assert.AreEqual(validateResult, false);
            controller.ModelState.AddModelError("test-err", "test error");

            IActionResult result = await controller.Register(validateModel);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            // success case
            controller.ModelState.Clear();
            validateModel = new RegisterInfo
            {
                UserName = "test01",
                Email = "a@a.com",
                Password = "abc123",
                ConfirmPassword = "abc123"
            };
            controller.ModelState.Clear();
            validateResultList = new List<ValidationResult>();
            validateResult = Validator.TryValidateObject(validateModel, new ValidationContext(validateModel),
                validateResultList, true);
            Assert.AreEqual(validateResult, true);

            result = await controller.Register(validateModel);
            Assert.IsInstanceOfType(result, typeof(OkResult));

            // test user exist case
            controller.ModelState.Clear();
            result = await controller.Register(new RegisterInfo
            {
                UserName = "exist01"
            });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            // test email exist
            result = await controller.Register(new RegisterInfo
            {
                UserName = "test01",
                Email = "exist@test.com"
            });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            // test add to role failed
            _fakeAddToRoleResultFlag = false;
            result = await controller.Register(new RegisterInfo
            {
                UserName = "test01"
            });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            // test role not exist
            _fakeRoleExistFlag = false;
            result = await controller.Register(new RegisterInfo
            {
                UserName = "test01"
            });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            // test create error
            _fakeCreateErrors =
            [
                new IdentityError
                {
                    Code = "test",
                    Description = "test"
                }
            ];
            result = await controller.Register(new RegisterInfo
            {
                UserName = "test01"
            });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task TestStatus()
        {
            UserManager<IdentityUser> userManager = CreateUserManager();
            SignInManager<IdentityUser> signInManager = CreateSignInManager(userManager);
            RoleManager<IdentityRole> roleManager = CreateRoleManager();
            ILogger<AccountController> logger = CreateLogger();
            var controller = new AccountController(userManager, signInManager, roleManager, logger);

            // test not login
            _fakeLoginFlag = false;
            IActionResult result = await controller.Status();
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            // test login
            _fakeLoginFlag = true;
            result = await controller.Status();
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}