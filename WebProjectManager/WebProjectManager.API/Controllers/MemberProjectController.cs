using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebProjectManager.Common.Authentication;
using WebProjectManager.Common.ViewModel;
using WebProjectManager.Models.EF;
using WebProjectManager.Models.Entities;

namespace WebProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberProjectController : Controller
    {
        private readonly EFCoreDbContext _context;

        public MemberProjectController(EFCoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var dataMember = _context.MemberProjects.Where(x => x.ProjectId == id).ToList();
            List<User> users = new List<User>();
            var projectOwner = await _context.Projects.FindAsync(id);
            var owner = await _context.Users.FindAsync(projectOwner.CreatedBy);
            users.Add(owner);
            foreach (var member in dataMember)
            {
                var data = await _context.Users.FindAsync(member.IdUser);
                users.Add(data);
            }
            return Ok(users);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<MemberProject>> Post(MemberProjectViewModel model,Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var checkProject = await _context.Projects.FindAsync(id);
            if (checkProject.CreatedBy != Guid.Parse(userId))
            {
                return BadRequest();
            }
            var checkMember = _context.MemberProjects.Where(x => x.ProjectId == id && x.IdUser == model.IdUser).ToList();
            if (checkMember.Count>0)
            {
                return BadRequest();
            }
            if (checkProject.CreatedBy == model.IdUser)
            {
                return BadRequest();
            }
            MemberProject createItem = new MemberProject()
            {
                Id = Guid.NewGuid(),
                IdUser = model.IdUser,
                ProjectId = id,
                CreatedOn = DateTime.Now,
                TimeExpiry = DateTime.Now,
                IsActive = model.IsActive,
            };
            _context.MemberProjects.Add(createItem);
            await _context.SaveChangesAsync();
            return Ok(createItem);
        }
        
        [HttpDelete("{idProject}/{idUser}")]
        public async Task<IActionResult> Delete(Guid idProject, Guid idUser)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.MemberProjects.FirstOrDefault(x => x.ProjectId == idProject && x.IdUser == idUser);
            var checkProject = await _context.Projects.FindAsync(idProject);
            if (checkProject.CreatedBy != Guid.Parse(userId))
            {
                return BadRequest();
            }
            if (project == null)
            {
                return BadRequest();
            }
            _context.Remove(project);
            await _context.SaveChangesAsync();
            return Ok();
        }

        
    }
}
