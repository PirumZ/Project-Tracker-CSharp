using Microsoft.AspNetCore.Mvc;
using Project_Tracker_C_.Models;
using Project_Tracker_C_.Services;

namespace Project_Tracker_C_.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        //private static List<TaskItem> _tasks = new List<TaskItem>();
        private readonly TaskService _service = new TaskService();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public IActionResult CreateTask(TaskItem task)
        {
            return Ok(_service.Create(task));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskItem updatedTask)
        {
            var result = _service.Update(id, updatedTask);

            if (result == null)
            {
                return NotFound();
            }
            else 
            { 
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var success = _service.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            else 
            {
                return NoContent();
            }
        }


    }
}
