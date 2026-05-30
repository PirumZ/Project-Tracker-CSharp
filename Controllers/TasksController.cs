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
        public async Task<IActionResult> Get(bool? completed)
        {
            var tasks = await _service.GetAll(completed);
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
            if (!ModelState.IsValid) 
            {
                return ValidationProblem(ModelState);
            }

            return Ok(await _service.Create(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
        {
            if (!ModelState.IsValid) 
            {
                return ValidationProblem(ModelState);
            }

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

        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult<TaskReadDto>> ToggleTask(int id) 
        {
            var updatedTask = await _service.Toggle(id);

            if (updatedTask == null) { return NotFound(); }

            return Ok(updatedTask);
        }

//[HttpPatch("{id}/title")]
  //      public async Task<ActionResult<TaskReadDto>> UpdateTitle(int id, UpdateTitleDto dto)




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
