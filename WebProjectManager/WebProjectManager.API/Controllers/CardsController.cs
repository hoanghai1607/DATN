using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjectManager.Common.Authentication;
using WebProjectManager.Common.Helper;
using WebProjectManager.Common.ViewModel;
using WebProjectManager.Models.EF;
using WebProjectManager.Models.Entities;
using WebProjectManager.Common.Services;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace WebProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly EFCoreDbContext _context;
        private readonly IEmailService _emailService;

        public CardsController(EFCoreDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Card>> Get(Guid id)
        {
            var data = _context.Cards.Where(x => x.TabId == id)
                .Include(i=>i.CardUserMembers)
                    .ThenInclude(cu => cu.MemberNavigation)
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
            if (model.TimeExpiry.HasValue)
            {
                project.TimeExpiry = model.TimeExpiry.Value;
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

        [HttpPost("Remind/{id}")]
        public async Task<IActionResult> RemindCardMembers(Guid id)
        {
            try
            {
                var card = await _context.Cards
                    .Include(c => c.CardUserMembers)
                        .ThenInclude(cu => cu.MemberNavigation)
                    .Include(c => c.Tasks)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (card == null)
                {
                    Debug.WriteLine("Card not found");
                    return NotFound("Card not found");
                }
                
                Debug.WriteLine($"Found card: {card.Name}, with {card.CardUserMembers.Count} members and {card.Tasks.Count} tasks");

                // Kiểm tra xem card đã hoàn thành chưa
                int completedTasks = card.Tasks.Count(t => t.IsActive == true);
                bool isComplete = card.Tasks.Count > 0 && completedTasks == card.Tasks.Count;
                
                Debug.WriteLine($"Card completion status: {completedTasks}/{card.Tasks.Count} tasks complete, isComplete={isComplete}");
                
                if (isComplete)
                {
                    Debug.WriteLine("Cannot send reminders for completed cards");
                    return BadRequest("Cannot send reminders for completed cards");
                }

                // Lấy ra các member
                var members = card.CardUserMembers.Select(cu => cu.MemberNavigation).ToList();
                
                if (members.Count == 0)
                {
                    Debug.WriteLine("No members assigned to this card");
                    return BadRequest("No members assigned to this card");
                }
                
                Debug.WriteLine($"Found {members.Count} members to remind");

                int successCount = 0;
                StringBuilder errorDetails = new StringBuilder();

                foreach (var member in members)
                {
                    try {
                        if (string.IsNullOrEmpty(member.Email))
                        {
                            Debug.WriteLine($"Member {member.Id} has no email address");
                            errorDetails.AppendLine($"Member {member.Id}: No email address");
                            continue;
                        }
                        
                        Debug.WriteLine($"Sending reminder to {member.Email}");

                        string subject = "Task Reminder";
                        
                        string safeCardName = WebUtility.HtmlEncode(card.Name?.Trim() ?? "Unnamed task");
                        string safeDescription = WebUtility.HtmlEncode(card.Description?.Trim() ?? "No description");
                        
                        StringBuilder body = new StringBuilder();
                        body.AppendLine("<h2>Task Reminder</h2>");
                        body.AppendLine("<p>This is a reminder that you have an incomplete task that requires your attention.</p>");
                        body.AppendLine("<p>Task details:</p>");
                        body.AppendLine("<ul>");
                        body.AppendLine($"<li><strong>Name:</strong> {safeCardName}</li>");
                        body.AppendLine($"<li><strong>Description:</strong> {safeDescription}</li>");
                        
                        body.AppendLine($"<li><strong>Progress:</strong> {completedTasks}/{card.Tasks.Count} tasks completed</li>");
                        body.AppendLine("</ul>");
                        body.AppendLine("<p>Please complete your assigned work as soon as possible.</p>");

                        await _emailService.SendEmailAsync(member.Email, subject, body.ToString());
                        successCount++;
                        Debug.WriteLine($"Successfully sent email to {member.Email}");
                    }
                    catch (Exception ex) {
                        Debug.WriteLine($"Error sending email to {member.Email}: {ex.Message}");
                        errorDetails.AppendLine($"Error sending to {member.Email}: {ex.Message}");
                    }
                }
                
                if (successCount == 0)
                {
                    return StatusCode(500, $"Failed to send any reminder emails. Details: {errorDetails}");
                }
                else if (successCount < members.Count)
                {
                    return Ok($"Sent {successCount} out of {members.Count} reminder emails. Some emails failed: {errorDetails}");
                }

                return Ok($"Successfully sent {successCount} reminder emails");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RemindCardMembers: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                return StatusCode(500, $"An error occurred while sending reminders: {ex.Message}. Inner exception: {ex.InnerException?.Message}");
            }
        }
    }
}
