using Microsoft.AspNetCore.Mvc;
using Project_Tracker_C_.Dtos;
using Project_Tracker_C_.Models;
using Project_Tracker_C_.Services;
using System.Diagnostics.Eventing.Reader;

namespace Project_Tracker_C_.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _service;

        public TasksController(TaskService service) 
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = await _service.GetAll();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            var task = await _service.GetById(id);
            if (task == null) { return NotFound(); }
            else { return Ok(task); }
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
        {
            var result = await _service.Update(id, dto);

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
        public async Task<IActionResult> DeleteTask(int id)
        {
            var success = await _service.Delete(id);
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
