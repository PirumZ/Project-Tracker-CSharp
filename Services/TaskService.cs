using Project_Tracker_C_.Models;
using Project_Tracker_C_.Data;

namespace Project_Tracker_C_.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context) 
        {
            _context = context;
        }

        public List<TaskItem> GetAll() 
        {
            return _context.Tasks.ToList();
        }

        public TaskItem Create(TaskItem task) 
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        public TaskItem? GetById(int id) 
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == id);
        }

        public TaskItem? Update(int id, TaskItem updatedTask) 
        {
            var task = GetById(id);
            if (task == null) { return null; }

            task.Title = updatedTask.Title;
            task.IsCompleted = updatedTask.IsCompleted;

            _context.SaveChanges();

            return task;
        }

        public bool Delete(int id) 
        {
            TaskItem task = GetById(id);
            if (task == null) { return false; }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return true;
        }
    }
}
