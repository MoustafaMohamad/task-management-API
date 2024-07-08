using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_management_API.DBContext;
using task_management_API.DTO;
using task_management_API.Models;
using Task = task_management_API.Models.Task;

namespace task_management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly TaskMangerContext _context;


        public TeamMemberController(TaskMangerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamMembers()
        {
            var teamMembersList = await _context.TeamMembers
               .Include(tm => tm.Tasks)
               .Select(member => new TeamMemberDto
               {
                   MemberId = member.MemberId,
                   Name = member.Name,
                   Email = member.Email,
                   Tasks = member.Tasks.Select(task => new TaskDto
                   {
                       TaskId = task.TaskId,
                       Name = task.Name,
                       Description = task.Description,
                       Status = task.Status,
                       StartDate = task.StartDate,
                       EndDate = task.EndDate,
                       MemberId = task.MemberId
                   }).ToList()
               })
               .ToListAsync();

            return Ok(teamMembersList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamMember(int id)
        {
            var teamMember = await _context.TeamMembers
                .Include(tm => tm.Tasks)
                .Where(tm => tm.MemberId == id)
                .Select(member => new TeamMemberDto
                {
                    MemberId = member.MemberId,
                    Name = member.Name,
                    Email = member.Email,
                    Tasks = member.Tasks.Select(task => new TaskDto
                    {
                        TaskId = task.TaskId,
                        Name = task.Name,
                        Description = task.Description,
                        Status = task.Status,
                        StartDate = task.StartDate,
                        EndDate = task.EndDate,
                        MemberId = task.MemberId
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (teamMember == null)
            {
                return NotFound();
            }

            return Ok(teamMember);
        }


        [HttpPost]
        public async Task<IActionResult> PostTeamMember(MemberDto MemberDto)
        {
            var teamMember = new TeamMember
            {
                Name = MemberDto.Name,
                Email = MemberDto.Email,
                Tasks = null
            };

            _context.TeamMembers.Add(teamMember);
            await _context.SaveChangesAsync();

            MemberDto.MemberId = teamMember.MemberId;

            return CreatedAtAction("GetTeamMember", new { id = teamMember.MemberId }, MemberDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamMember(int id, MemberDto teamMemberDto)
        {
            
            var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.MemberId == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            teamMember.Name = teamMemberDto.Name;
            teamMember.Email = teamMemberDto.Email;
          

            _context.Entry(teamMember).State = EntityState.Modified;

            
                await _context.SaveChangesAsync();
          

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamMember(int id)
        {
            var member = await _context.TeamMembers.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.TeamMembers.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
