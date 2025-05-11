using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebProjectManager.Common.Authentication;
using WebProjectManager.Models.EF;
using WebProjectManager.Models.Entities;

namespace WebProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskUserMemberController : ControllerBase
    {
        private readonly EFCoreDbContext _context;

        public TaskUserMemberController(EFCoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(Guid taskId)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var dataMember = _context.TaskUserMembers.Where(x => x.TaskId == taskId).ToList();
            List<User> users = new List<User>();
            foreach (var member in dataMember)
            {
                var data = await _context.Users.FindAsync(member.Member);
                users.Add(data);
            }
            return Ok(users);
        }

        [HttpPost("{taskId}/{userId}")]
        public async Task<ActionResult<TaskUserMember>> Post(Guid taskId, Guid userId)
        {

            var checkMember = _context.TaskUserMembers.Where(x => x.Member == userId && x.TaskId == taskId).ToList();

            if (checkMember.Count > 0)
            {
                return BadRequest();
            }
            TaskUserMember createItem = new TaskUserMember()
            {
                Id = Guid.NewGuid(),
                Member = userId,
                TaskId = taskId
            };
            _context.TaskUserMembers.Add(createItem);
            await _context.SaveChangesAsync();
            return Ok(createItem);
        }

        [HttpDelete("{taskId}/{userId}")]
        public async Task<IActionResult> Delete(Guid taskUd, Guid userId)
        {
            var checkMember = _context.TaskUserMembers.Where(x => x.Member == userId && x.TaskId == taskUd).ToList();

            if (checkMember.Count == 0)
            {
                return BadRequest();
            }
            _context.TaskUserMembers.Remove(checkMember.FirstOrDefault());
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
