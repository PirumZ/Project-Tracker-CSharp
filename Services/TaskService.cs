using Project_Tracker_C_.Models;

namespace Project_Tracker_C_.Services
{
    public class TaskService
    {
        private static List<TaskItem> _tasks = new();

        public List<TaskItem> GetAll() => _tasks;

        public TaskItem Create(TaskItem task) 
        {
            task.Id = _tasks.Count + 1;
            _tasks.Add(task);
            return task;
        }

        public TaskItem? GetById(int id) 
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public TaskItem? Update(int id, TaskItem updatedTask) 
        {
            TaskItem task = GetById(id);
            if (task == null)
            {
                return null;
            }
            else 
            {
                task.Title = updatedTask.Title;
                task.IsCompleted = updatedTask.IsCompleted;
                return task;
            }
        }

        public bool Delete(int id) 
        {
            TaskItem task = GetById(id);
            if (task == null)
            {
                return false;
            }
            else 
            {
                _tasks.Remove(task);
                return true;
            }
        }
    }
}
