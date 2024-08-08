using Aiba.Model;
using Aiba.Repository;
using Hangfire;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Aiba.TaskManager
{
    public class TaskManager(ILogger<TaskManager> logger) : ITaskManager
    {
        private static readonly Dictionary<string, string?> RunningTasks = new();
        private static readonly Dictionary<string, CancellationTokenSource> CancellationTokenSources = new();

        public void EnqueueTask(string taskName, Expression<Action> task)
        {
            if (CheckTaskRunning(taskName))
                return;
            string? taskId = BackgroundJob.Enqueue(task);
            logger.LogInformation("Task {TaskName} with id {TaskId} is pending", taskName, taskId);
            RunningTasks.Add(taskName, taskId);
        }

        public bool CheckTaskRunning(string taskName)
        {
            if (!RunningTasks.TryGetValue(taskName, out string? jobId))
                return false;
            IMonitoringApi? monitor = JobStorage.Current.GetMonitoringApi();
            IEnumerable<KeyValuePair<string, ProcessingJobDto>> processingJobs =
                monitor.ProcessingJobs(0, int.MaxValue).Where(j => j.Key == jobId);

            if (processingJobs.Any())
            {
                return true;
            }

            RunningTasks.Remove(taskName);
            CancellationTokenSources.Remove(taskName);
            return false;
        }

        public CancellationTokenSource CreateCancellationTokenSource(string taskName)
        {
            CancellationTokenSources[taskName] = new CancellationTokenSource();
            return CancellationTokenSources[taskName];
        }

        public void CancelTask(string taskName)
        {
            if (CancellationTokenSources.TryGetValue(taskName, out CancellationTokenSource? cancellationTokenSource))
            {
                cancellationTokenSource.Cancel();
            }
        }
    }
}