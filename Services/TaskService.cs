using Project_Tracker_C_.Models;
using Project_Tracker_C_.Data;
using Project_Tracker_C_.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Project_Tracker_C_.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<List<TaskReadDto>> GetAll(bool? completed) 
        {
            var query = _context.Tasks.AsQueryable();

            if (completed.HasValue) 
            {
                query = query.Where(t => t.IsCompleted == completed.Value);
            }

            var tasks = await query.ToListAsync();

            return tasks.Select(t => new TaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted
            }).ToList();
        }

        public async Task<TaskReadDto> Create(TaskCreateDto dto) 
        {
            var task = new TaskItem
            { Title = dto.Title, IsCompleted = false };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new TaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<TaskItem?> GetById(int id) 
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        

        public async Task<TaskReadDto?> Toggle(int id) 
        {
            var task = await GetById(id);
            if (task == null) { return null; }

            task.IsCompleted = !task.IsCompleted;

            await _context.SaveChangesAsync();

            return new TaskReadDto 
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<TaskReadDto?> UpdateTitle(int id, UpdateTitleDto dto) 
        {
            var task = await GetById(id);
            if (task == null) { return null; }

            task.Title = dto.Title;

            await _context.SaveChangesAsync();

            return new TaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<bool> Delete(int id) 
        {
            TaskItem? task = await GetById(id);
            if (task == null) { return false; }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
