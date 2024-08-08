using Aiba.Controllers;
using Aiba.Entities;
using Aiba.Model;
using Aiba.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace Aiba.Tests.ControllerTests
{
    [TestClass]
    public class LibraryTest
    {
        private readonly List<LibraryEntity> _testLibraryEntities =
        [
            new LibraryEntity
            {
                Id = 0,
                Name = "TestLibrary1",
                Path = "TestPath1",
                UserId = "TestUser1"
            },
            new LibraryEntity
            {
                Id = 1,
                Name = "TestLibrary2",
                Path = "TestPath2",
                UserId = "TestUser1"
            },
            new LibraryEntity
            {
                Id = 2,
                Name = "TestLibrary3",
                Path = "TestPath3",
                UserId = "TestUser3"
            }
        ];

        private bool _fakeAddLibraryThrowError;
        private bool _fakeDeleteLibraryThrowError;

        private string _fakeSignInUserId = "TestUser01";

        private UserManager<IdentityUser> CreateMockUserManager()
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
            userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns((ClaimsPrincipal _) =>
                {
                    return _testLibraryEntities.Any(x => x.UserId == _fakeSignInUserId) ? _fakeSignInUserId : null;
                });

            return userManagerMock.Object;
        }

        private ILogger<LibraryController> GetMockLogger()
        {
            return new Mock<ILogger<LibraryController>>().Object;
        }

        private IUnitOfWork GetMockUnitOfWork()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.GetLibraryInfosByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    IEnumerable<LibraryInfo> result = _testLibraryEntities.Where(x => x.UserId == id).Select(x =>
                        new LibraryInfo
                        {
                            Name = x.Name,
                            Path = x.Path,
                            Type = x.Type
                        });
                    return result;
                });
            mockUnitOfWork.Setup(x => x.AddLibraryInfoByUserIdAsync(It.IsAny<string>(), It.IsAny<LibraryInfo>()))
                .Callback((string _, LibraryInfo _) =>
                {
                    if (_fakeAddLibraryThrowError)
                        throw new Exception("test");
                });
            mockUnitOfWork.Setup(x => x.RemoveLibraryByUserIdAsync(It.IsAny<string>(), It.IsAny<LibraryInfo>()))
                .Callback((string _, LibraryInfo _) =>
                {
                    if (_fakeDeleteLibraryThrowError)
                        throw new Exception("test");
                });
            return mockUnitOfWork.Object;
        }

        private SignInManager<IdentityUser> GetSinInManager()
        {
            var signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                CreateMockUserManager(),
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                null!,
                null!,
                null!,
                null!
            );
            return signInManagerMock.Object;
        }

        [TestMethod]
        public async Task TestAddLibrary()
        {
            IUnitOfWork unitOfWork = GetMockUnitOfWork();
            ILogger<LibraryController> logger = GetMockLogger();
            SignInManager<IdentityUser> signInManager = GetSinInManager();
            var controller = new LibraryController(unitOfWork, logger, signInManager);
            // test with invalid user
            _fakeSignInUserId = "InvalidUser";
            IActionResult result = await controller.AddLibrary(new LibraryInfo()
            {
                Path = AppDomain.CurrentDomain.BaseDirectory
            });
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            // test internal server error
            _fakeSignInUserId = "TestUser1";
            _fakeAddLibraryThrowError = true;
            result = await controller.AddLibrary(new LibraryInfo()
            {
                Path = AppDomain.CurrentDomain.BaseDirectory
            });
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual((result as ObjectResult)?.StatusCode, 500);

            // test success
            _fakeSignInUserId = "TestUser1";
            _fakeAddLibraryThrowError = false;
            result = await controller.AddLibrary(new LibraryInfo()
            {
                Path = AppDomain.CurrentDomain.BaseDirectory
            });
            Assert.IsInstanceOfType(result, typeof(OkResult));

            // test directory not exist
            _fakeSignInUserId = "TestUser1";
            _fakeAddLibraryThrowError = false;
            result = await controller.AddLibrary(new LibraryInfo
            {
                Path = "InvalidPath"
            });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task TestGetLibraries()
        {
            IUnitOfWork unitOfWork = GetMockUnitOfWork();
            ILogger<LibraryController> logger = GetMockLogger();
            SignInManager<IdentityUser> signInManager = GetSinInManager();
            var controller = new LibraryController(unitOfWork, logger, signInManager);
            // test with invalid user
            _fakeSignInUserId = "InvalidUser";
            ActionResult<IEnumerable<LibraryInfo>> result = await controller.GetLibraries();
            Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedResult));

            // test with valid user "TestUser1"
            _fakeSignInUserId = "TestUser1";
            var expect = _testLibraryEntities.Where(x => x.UserId == _fakeSignInUserId).Select(x => new LibraryInfo
            {
                Name = x.Name,
                Path = x.Path,
                Type = x.Type
            }).ToList();
            result = await controller.GetLibraries();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            List<LibraryInfo> resultList =
                ((result.Result as OkObjectResult)?.Value as IEnumerable<LibraryInfo>)?.ToList() ??
                new List<LibraryInfo>();
            Assert.AreEqual(resultList.Count, expect.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i].Name, resultList[i].Name);
                Assert.AreEqual(expect[i].Path, resultList[i].Path);
                Assert.AreEqual(expect[i].Type, resultList[i].Type);
            }

            // test with valid user "TestUser1"
            _fakeSignInUserId = "TestUser3";
            expect = _testLibraryEntities.Where(x => x.UserId == _fakeSignInUserId).Select(x => new LibraryInfo
            {
                Name = x.Name,
                Path = x.Path,
                Type = x.Type
            }).ToList();
            result = await controller.GetLibraries();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            resultList =
                ((result.Result as OkObjectResult)?.Value as IEnumerable<LibraryInfo>)?.ToList() ??
                new List<LibraryInfo>();
            Assert.AreEqual(resultList.Count, expect.Count);
            for (int i = 0; i < expect.Count; i++)
            {
                Assert.AreEqual(expect[i].Name, resultList[i].Name);
                Assert.AreEqual(expect[i].Path, resultList[i].Path);
                Assert.AreEqual(expect[i].Type, resultList[i].Type);
            }
        }

        [TestMethod]
        public async Task TestRemoveLibrary()
        {
            IUnitOfWork unitOfWork = GetMockUnitOfWork();
            ILogger<LibraryController> logger = GetMockLogger();
            SignInManager<IdentityUser> signInManager = GetSinInManager();
            var controller = new LibraryController(unitOfWork, logger, signInManager);
            // test with invalid user
            _fakeSignInUserId = "InvalidUser";
            IActionResult result = await controller.RemoveLibrary(new LibraryInfo());
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            // test with internal server error
            _fakeSignInUserId = "TestUser1";
            _fakeDeleteLibraryThrowError = true;
            result = await controller.RemoveLibrary(new LibraryInfo());
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual((result as ObjectResult)?.StatusCode, 500);

            // test success
            _fakeSignInUserId = "TestUser1";
            _fakeDeleteLibraryThrowError = false;
            result = await controller.RemoveLibrary(new LibraryInfo());
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}