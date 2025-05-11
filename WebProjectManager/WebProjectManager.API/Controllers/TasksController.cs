using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectManager.Common.Authentication;
using WebProjectManager.Common.ViewModel;
using WebProjectManager.Models.EF;

namespace WebProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly EFCoreDbContext _context;

        public TasksController(EFCoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<WebProjectManager.Models.Entities.Task>> Get(Guid id)
        {
            var data = _context.Tasks.Where(x => x.CardId == id)
                .Include(i => i.TaskUserMember)
                    .ThenInclude(ti=>ti.MemberNavigation)
                .ToList();
            return Ok(data);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<WebProjectManager.Models.Entities.Task>> Post(TaskViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            WebProjectManager.Models.Entities.Task createItem = new WebProjectManager.Models.Entities.Task()
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Parse(userId),
                CardId= id,
                Icon = model.Icon,
                Name = model.Name,
                NumberMember = model.NumberMember,
                Comment = model.Comment,
                Type = "Chưa hoàn thành",
                CreatedOn = DateTime.Now,
                TimeExpiry = model.TimeExpiry,
                IsActive = model.IsActive,
                Order = model.Order
            };
            _context.Tasks.Add(createItem);
            await _context.SaveChangesAsync();
            return Ok(createItem);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<WebProjectManager.Models.Entities.Task>> Put(TaskViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.Tasks.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            if (model.Comment != null && model.Comment != "")
            {
                project.Comment = model.Comment;
            }
            if (model.Name != null && model.Name != "")
            {
                project.Name = model.Name;
            }
            if (model.Icon != null && model.Icon != "")
            {
                project.Icon = model.Icon;
            }
            if (model.NumberMember != null)
            {
                project.NumberMember = model.NumberMember;
            }
            if (model.IsActive != null)
            {
                project.IsActive = model.IsActive;
            }
            if (model.TimeExpiry != null)
            {
                project.TimeExpiry = model.TimeExpiry;
            }
            if (model.Order != null)
            {
                project.Order = model.Order;
            }
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(project);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.Tasks.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            _context.Remove(project);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Id/{idTask}")]
        public ActionResult<WebProjectManager.Models.Entities.Task> GetById(Guid idTask)
        {
            var project = _context.Tasks.FirstOrDefault(x => x.Id == idTask);
            if (project == null)
            {
                return BadRequest();
            }
            return Ok(project);
        }

        [HttpGet("Change/{idTask}")]
        public ActionResult<WebProjectManager.Models.Entities.Task> Change(Guid idTask)
        {
            var project = _context.Tasks.FirstOrDefault(x => x.Id == idTask);
            if (project == null)
            {
                return BadRequest();
            }
            if (project.Type == "Chưa hoàn thành")
            {
                project.Type = "Đã hoàn thành";
            } else
            {
                project.Type = "Chưa hoàn thành";
            }
            _context.Entry(project).State = EntityState.Modified;
            return Ok(project);
        }
        [HttpPut("Name/{id}")]
        public async Task<ActionResult<WebProjectManager.Models.Entities.Task>> PutName(UpdateNameViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.Tasks.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            if (model.Name != null && model.Name != "")
            {
                project.Name = model.Name;
            }
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(project);
        }
    }
}
