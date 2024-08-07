using Aiba.Controllers;
using Aiba.MediaInfoProviders;
using Aiba.Model;
using Aiba.Model.RequestParams;
using Aiba.Plugin;
using Aiba.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Aiba.Tests.ControllerTests
{
    [TestClass]
    public class MediaInfoControllerTest
    {
        private bool _fakeIsSignIn;
        private bool _fakeThrowOnAdd;

        [TestMethod]
        public async Task TestAddMediaInfo()
        {
            var mockMediaInfoProvider = new Mock<IMediaInfoProvider>();
            mockMediaInfoProvider.Setup(x => x.ProviderName)
                .Returns("test");
            MediaProviderFactory providerFactory =
                new Mock<MediaProviderFactory>(new List<IMediaInfoProvider>
                {
                    mockMediaInfoProvider.Object
                }).Object;
            ILogger<MediaInfoController> logger = new Mock<ILogger<MediaInfoController>>().Object;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!
            );
            mockUserManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns((ClaimsPrincipal _) => _fakeIsSignIn ? "test" : null);
            mockUnitOfWork.Setup(x =>
                    x.AddMediaInfoToLibraryAsync(It.IsAny<string>(), It.IsAny<LibraryInfo>(), It.IsAny<MediaInfo>()))
                .Callback((string _, LibraryInfo _, MediaInfo _) =>
                {
                    if (_fakeThrowOnAdd)
                        throw new Exception("test");
                });
            var controller =
                new MediaInfoController(providerFactory, logger, mockUnitOfWork.Object, mockUserManager.Object);
            // test not sign in
            _fakeIsSignIn = false;
            ActionResult result = await controller.AddMediaInfoToLibrary(new AddMediaInfoRequest());
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));

            // test throw on add
            _fakeIsSignIn = true;
            _fakeThrowOnAdd = true;
            result = await controller.AddMediaInfoToLibrary(new AddMediaInfoRequest());
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            // test success
            _fakeThrowOnAdd = false;
            result = await controller.AddMediaInfoToLibrary(new AddMediaInfoRequest());
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}