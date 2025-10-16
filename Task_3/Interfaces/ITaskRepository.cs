using Task_3.Models;

namespace Task_3.Interfaces
{
    public interface ITaskRepository
    {
        IEnumerable<Tasks> GetAllTasks();
        Tasks GetTaskById(int id);
        int AddTask(Tasks task);
        bool DeleteTask(int id);
        bool UpdateTask(int id, bool isCompleted);

    }
}
