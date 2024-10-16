using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Aiba.TaskManager
{
    public class TaskManager(ILogger<TaskManager> logger) : ITaskManager
    {
        private static readonly ConcurrentDictionary<string, string?> _RunningTasks = new();
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _CancellationTokenSources = new();

        public void EnqueueTask(string taskName, Expression<Action> task)
        {
            if (CheckTaskRunning(taskName))
                return;
            string? taskId = BackgroundJob.Enqueue(task);
            logger.LogInformation("Task {TaskName} with id {TaskId} is pending", taskName, taskId);
            _RunningTasks[taskName] = taskId;
        }

        public bool CheckTaskRunning(string taskName)
        {
            if (!_RunningTasks.TryGetValue(taskName, out string? jobId))
                return false;
            IMonitoringApi? monitor = JobStorage.Current.GetMonitoringApi();
            IEnumerable<KeyValuePair<string, ProcessingJobDto>> processingJobs =
                monitor.ProcessingJobs(0, int.MaxValue).Where(j => j.Key == jobId);

            if (processingJobs.Any())
            {
                return true;
            }

            _RunningTasks.Remove(taskName, out _);
            _CancellationTokenSources.Remove(taskName, out _);
            return false;
        }

        public CancellationTokenSource CreateCancellationTokenSource(string taskName)
        {
            _CancellationTokenSources[taskName] = new CancellationTokenSource();
            return _CancellationTokenSources[taskName];
        }

        public void CancelTask(string taskName)
        {
            if (_CancellationTokenSources.TryGetValue(taskName, out CancellationTokenSource? cancellationTokenSource))
            {
                cancellationTokenSource.Cancel();
            }
        }

        public string GenerateTaskName(string userId, string libraryName, string action)
        {
            return "userId:" + userId + ";library:" + libraryName + ";action:" + action;
        }
    }
}