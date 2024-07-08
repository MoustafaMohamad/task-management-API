using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_management_API.DBContext;
using task_management_API.DTO;
using Task = task_management_API.Models.Task;


namespace task_management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskMangerContext _context;

        public TaskController(TaskMangerContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasksList = await _context.Tasks
                .Include(task => task.TeamMember)
                .Select(task => new TaskDto
                {
                    TaskId = task.TaskId,
                    Name = task.Name,
                    Description = task.Description,
                    Status = task.Status,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    MemberId = task.MemberId
                })
                .ToListAsync();
            return Ok(tasksList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.Tasks
                .Where(t => t.TaskId == id)
                .Select(task => new TaskDto
                {
                    TaskId = task.TaskId,
                    Name = task.Name,
                    Description = task.Description,
                    Status = task.Status,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    MemberId = task.MemberId
                })
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskDto taskDto)
        {
            if (id != taskDto.TaskId)
            {
                return BadRequest();
            }
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            

            
                if (ModelState.IsValid)
                {
                    task.Name = taskDto.Name;
                    task.Description = taskDto.Description;
                    task.Status = taskDto.Status;
                    task.StartDate = taskDto.StartDate;
                    task.EndDate = taskDto.EndDate;
                    task.MemberId = taskDto.MemberId;

                    _context.Entry(task).State = EntityState.Modified;
                }
                else
                {
                    return BadRequest();
                }

                await _context.SaveChangesAsync();

            return NoContent();


        }


        [HttpPost]
        public async Task<IActionResult> PostTask(TaskDto taskDto)
        {
            if (ModelState.IsValid)
            {
                var task = new Task
                {
                    Name = taskDto.Name,
                    Description = taskDto.Description,
                    Status = taskDto.Status,
                    StartDate = taskDto.StartDate,
                    EndDate = taskDto.EndDate,
                    MemberId = taskDto.MemberId
                };
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return Ok(taskDto);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
       

    }
}
