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
    public class CardsController : ControllerBase
    {
        private readonly EFCoreDbContext _context;

        public CardsController(EFCoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Card>> Get(Guid id)
        {
            var data = _context.Cards.Where(x => x.TabId == id)
                .Include(i=>i.CardUserMembers)
                .ToList();
            return Ok(data);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Card>> Post(CardViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            
            Card createItem = new Card()
            {
                Id = Guid.NewGuid(),
                CreatedBy = Guid.Parse(userId),
                TabId = id,
                Name = model.Name,
                NumberMember = model.NumberMember,
                CreatedOn = DateTime.Now,
                TimeExpiry = model.TimeExpiry,
                IsActive = model.IsActive,
                Order = model.Order,
            };
            _context.Cards.Add(createItem);
            await _context.SaveChangesAsync();
            return Ok(createItem);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Card>> Put(CardViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.Cards.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            if (model.Name != null && model.Name != "")
            {
                project.Name = model.Name;
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
            var project = _context.Cards.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            var lsTask = _context.Tasks.Where(x => x.CardId == id).ToList();
            foreach (var item in lsTask)
            {
                _context.Remove(item);
            }
            _context.Remove(project);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Id/{idCard}")]
        public ActionResult<Card> GetById(Guid idCard)
        {
            var project = _context.Cards.FirstOrDefault(x => x.Id == idCard);
            if (project == null)
            {
                return BadRequest();
            }
            return Ok(project);
        }
        [HttpPut("UploadImage/{id}")]
        public async Task<ActionResult<Card>> UploadImageCard(IFormFile file, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var images = UploadImage.UploadImageFile(file);
            var project = _context.Cards.FirstOrDefault(x => x.Id == id);
            project.Image = images.ToString();
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(project);
        }
        [HttpPut("Name/{id}")]
        public async Task<ActionResult<Card>> PutName(UpdateNameViewModel model, Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var project = _context.Cards.FirstOrDefault(x => x.Id == id);
            if (project == null)
            {
                return BadRequest();
            }
            if (model.Name != null && model.Name != "")
            {
                project.Name = model.Name;
            }
            if (model.Description != null && model.Description != "")
            {
                project.Description = model.Description;
            }
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(project);
        }

        [HttpPut("Move/{idcard}/{idtabnew}/{idtabold}/{number}")]
        public async Task<ActionResult<Card>> PutMove(Guid idcard, Guid idtabnew,Guid idtabold, int number)
        {
            var card = _context.Cards.FirstOrDefault(x => x.Id == idcard);
            if (card == null)
            {
                return BadRequest();
            }
            var lsCardOld = _context.Cards.Where(x=>x.TabId == idtabold && x.Id!= idcard).OrderBy(o=>o.Order).ToList();
            var lsCardNew = _context.Cards.Where(x => x.TabId == idtabnew).OrderBy(o => o.Order).ToList();

            foreach (var item in lsCardOld)
            {
                if (item.Order > card.Order)
                {
                    item.Order = item.Order - 1;
                }
            }
            foreach (var item in lsCardNew)
            {
                if (item.Order >= number)
                {
                    item.Order = item.Order + 1;
                }
            }
            card.TabId = idtabnew;
            card.Order = number;
            _context.Entry(card).State = EntityState.Modified;
            foreach (var item in lsCardOld)
            {

                _context.Entry(item).State = EntityState.Modified;
            }
            foreach (var item in lsCardNew)
            {

                _context.Entry(item).State = EntityState.Modified;
            }
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(card);
        }

        [HttpPut("MoveOrder/{id}/{number}")]
        public async Task<ActionResult<Card>> PutMove(Guid id, int number)
        {
            var card = _context.Cards.FirstOrDefault(x => x.Id == id);
            var lsCard = await _context.Cards.Where(x=>x.TabId==card.TabId && x.Id != id).OrderBy(o=>o.Order).ToListAsync();
            if (card == null)
            {
                return BadRequest();
            }
            //Tăng số - Đi xuống
            if (number > card.Order)
            {
                card.Order = number;
                foreach (var item in lsCard)
                {
                    if (item.Order <= card.Order)
                    {
                        item.Order = item.Order - 1;
                    }
                    
                }
            } else
            {
                if (number < card.Order)
                {
                    //Giảm số - Đi lên
                    card.Order = number;
                    foreach (var item in lsCard)
                    {
                        
                        if (item.Order >= card.Order)
                        {
                            item.Order = item.Order + 1;
                        }
                    }
                }
            }
            _context.Entry(card).State = EntityState.Modified;
            foreach (var item in lsCard)
            {

                _context.Entry(item).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return Ok(card);
        }
    }
}
