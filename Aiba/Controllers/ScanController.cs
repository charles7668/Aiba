using Aiba.Enums;
using Aiba.Model;
using Aiba.Model.RequestParams;
using Aiba.Plugin.Scanner;
using Aiba.Repository;
using Aiba.Scanners;
using Aiba.TaskManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScanController : Controller
    {
        public ScanController(ScannerFactory scannerFactory, ILogger<ScanController> logger,
            UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, ITaskManager taskManager)
        {
            _scannerFactory = scannerFactory;
            _logger = logger;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _taskManager = taskManager;
        }

        private readonly ILogger<ScanController> _logger;
        private readonly ScannerFactory _scannerFactory;
        private readonly ITaskManager _taskManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        [HttpGet("scanner")]
        public Task<ActionResult<IEnumerable<string>>> GetScanners([FromQuery] int flagNumber)
        {
            var flag = (MediaTypeFlag)flagNumber;
            return Task.FromResult<ActionResult<IEnumerable<string>>>(Ok(_scannerFactory.GetScannersByMediaType(flag)
                .Select(x => x.Name)));
        }

        [HttpGet("status")]
        public Task<ActionResult<bool>> CheckTaskStatus([FromQuery(Name = "library")] string libraryName)
        {
            string? userId = _userManager.GetUserId(User);
            if (userId == null)
                return Task.FromResult<ActionResult<bool>>(Unauthorized());
            string taskName = _taskManager.GenerateTaskName(userId, libraryName);
            bool isRunning = _taskManager.CheckTaskRunning(taskName);
            return Task.FromResult<ActionResult<bool>>(Ok(isRunning));
        }

        [HttpPost("mediaInfos")]
        public async Task<IActionResult> ScanMediaInfos(ScanMediaInfoRequest request)
        {
            string? userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();
            _logger.LogInformation("Scanning media infos by user {User} , Library : {LibraryName}", userId,
                request.LibraryName);
            LibraryInfo libraryInfo = await _unitOfWork.GetLibraryInfo(userId, request.LibraryName);
            string scannerName = libraryInfo.ScannerName;
            IScanner? scanner = _scannerFactory.GetScanner(scannerName);
            if (scanner == null)
            {
                _logger.LogError("Scanner {ScannerName} not found", scannerName);
                return BadRequest("Scanner not found");
            }

            string taskName = _taskManager.GenerateTaskName(userId, libraryInfo.Name);
            if (_taskManager.CheckTaskRunning(taskName))
                return Ok();
            CancellationTokenSource cancellationTokenSource = _taskManager.CreateCancellationTokenSource(taskName);

            _taskManager.EnqueueTask(taskName,
                () => TaskExecutor.ScanningTask(userId, libraryInfo, scannerName,
                    cancellationTokenSource.Token));
            return Ok(new Dictionary<string, string>
            {
                { "taskName", taskName }
            });
        }
    }
}