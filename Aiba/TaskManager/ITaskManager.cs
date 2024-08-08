using Aiba.Model;
using System.Linq.Expressions;

namespace Aiba.TaskManager
{
    public interface ITaskManager
    {
        public void EnqueueTask(string taskName, Expression<Action> task);
        public bool CheckTaskRunning(string taskName);
        public CancellationTokenSource CreateCancellationTokenSource(string taskName);
        public void CancelTask(string taskName);
    }
}