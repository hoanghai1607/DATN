using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectManager.Common.Authentication;
using WebProjectManager.Common.Helper;
using WebProjectManager.Common.ViewModel;
using WebProjectManager.Models.EF;
using WebProjectManager.Models.Entities;

namespace WebProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : Controller
    {
        private readonly EFCoreDbContext _context;

        public ProjectsController(EFCoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Project>>> Get()
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var data = _context.Projects.Where(x => x.CreatedBy == Guid.Parse(userId)).ToList();
            var projects = _context.MemberProjects.Where(x=>x.IdUser == Guid.Parse(userId)).ToList();
            foreach (var item in projects)
            {
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == item.ProjectId);
                data.Add(project);
            }
            return Ok(data);
        }
        [HttpGet("UserProject")]
        public async Task<ActionResult<IEnumerable<Project>>> GetUserProject()
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var data = _context.Projects.Where(x => x.CreatedBy == Guid.Parse(userId)).ToList();
            return Ok(data);
        }

        [HttpPost("")]
        public async Task<ActionResult<Project>> Post(ProjectViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            Project createItem = new Project()
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Parse(userId),
                Name = model.Name,
                NumberMember = model.NumberMember,
                Background = model.Background,
                CreatedOn = DateTime.Now,
                TimeExpiry = model.TimeExpiry,
                IsActive = model.IsActive,
                
            };
            _context.Projects.Add(createItem);
            await _context.SaveChangesAsync();
            return Ok(createItem);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> Put(ProjectViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.Projects.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            if (model.Name != null && model.Name != "")
            {
                project.Name = model.Name;
            }
            if (model.IsActive != null)
            {
                project.IsActive = model.IsActive;
            }
            if (model.TimeExpiry != null)
            {
                project.TimeExpiry = model.TimeExpiry;
            }
            if (model.Background != null)
            {
                project.Background = model.Background;
            }
            if (model.NumberMember != null)
            {
                project.NumberMember = model.NumberMember;
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
            var project = _context.Projects.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            if (project.CreatedBy != Guid.Parse(userId))
            {
                return BadRequest();
            }
            _context.Remove(project);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<Project> GetById(Guid id)
        {
            var project = _context.Projects.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            return Ok(project);
        }

        [HttpPut("UploadImage/{id}")]
        public async Task<ActionResult<Project>> UploadImageProject(IFormFile file, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var images = UploadImage.UploadImageFile(file);
            var project = _context.Projects.FirstOrDefault(x => x.Id == id);
            project.Image = images.ToString();
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(project);
        }
        [HttpGet("GetName/{name}")]
        public ActionResult<List<Project>> GetName(string name)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var data = _context.Projects.Where(x => x.CreatedBy == Guid.Parse(userId)).ToList();
            var projects = _context.MemberProjects.Where(x => x.IdUser == Guid.Parse(userId)).ToList();
            List<Project> projectData = new List<Project>();
            foreach (var item in projects)
            {
                var project = from c in _context.Projects
                              where EF.Functions.Like(c.Name, "%" + name + "%") && c.Id == item.ProjectId
                              select c;
                if (project.FirstOrDefault() != null)
                {
                    projectData.Add(project.FirstOrDefault());
                }
            }
            foreach (var item in data)
            {
                var projectOwner = from c in _context.Projects
                                   where EF.Functions.Like(c.Name, "%" + name + "%") && c.Id == item.Id
                                   select c;
                if (projectOwner.FirstOrDefault() != null)
                {
                    projectData.Add(projectOwner.FirstOrDefault());
                }
            }

            return Ok(projectData);
        }
    }
}
