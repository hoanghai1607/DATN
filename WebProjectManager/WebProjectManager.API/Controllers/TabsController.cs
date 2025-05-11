using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectManager.Common.Authentication;
using WebProjectManager.Common.ViewModel;
using WebProjectManager.Models.EF;
using WebProjectManager.Models.Entities;

namespace WebProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TabsController : Controller
    {
        private readonly EFCoreDbContext _context;

        public TabsController(EFCoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Tab>>> Get(Guid id)
        {
            var data = await _context.Tabs.Where(x => x.ProjectId==id)
                .Include(x => x.Cards.OrderBy(c=>c.Order))
                    .ThenInclude(it => it.Tasks.OrderBy(d=>d.Order))
                        

                .ToListAsync();
            return Ok(data);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Tab>> Post(TabViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            Tab createItem = new Tab()
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Parse(userId),
                ProjectId = id,
                Name = model.Name,
 //               Description = model.Description,
                CreatedOn = DateTime.Now,
                TimeExpiry = model.TimeExpiry,
                IsActive = model.IsActive,
                Order = model.Order,
            };
            _context.Tabs.Add(createItem);
            await _context.SaveChangesAsync();
            return Ok(createItem);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Tab>> Put(TabViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.Tabs.FirstOrDefault(x => x.Id == id);
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
            var project = _context.Tabs.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            var lsCard = _context.Cards.Where(x=>x.TabId == id).ToList();
            foreach (var item in lsCard)
            {
                var lsTask = _context.Tasks.Where(x => x.CardId == item.Id).ToList();
                foreach (var items in lsTask)
                {
                    _context.Remove(items);
                }
                _context.Remove(item);
            }
            _context.Remove(project);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Id/{idTab}")]
        public ActionResult<Tab> GetById(Guid idTab)
        {
            var project = _context.Tabs.FirstOrDefault(x => x.Id == idTab);
            if (project == null)
            {
                return BadRequest();
            }
            return Ok(project);
        }
        [HttpPut("Name/{id}")]
        public async Task<ActionResult<Tab>> PutName(UpdateNameViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.Tabs.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            if (model.Name != null && model.Name != "")
            {
                project.Name = model.Name;
            }
            //if (model.Description != null && model.Description != "")
            //{
            //    project.Description = model.Description;
            //}
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(project);
        }
    }
}
