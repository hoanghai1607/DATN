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
    public class CardUserMemberController : ControllerBase
    {
        private readonly EFCoreDbContext _context;

        public CardUserMemberController(EFCoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("{cardId}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(Guid cardId)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var dataMember = _context.CardUserMembers.Where(x => x.CardId == cardId).ToList();
            List<User> users = new List<User>();
            foreach (var member in dataMember)
            {
                var data = await _context.Users.FindAsync(member.Member);
                users.Add(data);
            }
            return Ok(users);
        }

        [HttpPost("{cardId}/{userId}")]
        public async Task<ActionResult<CardUserMember>> Post(Guid cardId, Guid userId)
        {
           
            var checkMember = _context.CardUserMembers.Where(x => x.Member == userId && x.CardId == cardId).ToList();
            
            if (checkMember.Count > 0)
            {
                return BadRequest();
            }
            CardUserMember createItem = new CardUserMember()
            {
                Id = Guid.NewGuid(),
                Member = userId,
                CardId = cardId
            };
            _context.CardUserMembers.Add(createItem);
            await _context.SaveChangesAsync();
            return Ok(createItem);
        }

        [HttpDelete("{cardId}/{userId}")]
        public async Task<IActionResult> Delete(Guid cardId,Guid userId)
        {
            var checkMember = _context.CardUserMembers.Where(x => x.Member == userId && x.CardId == cardId).ToList();

            if (checkMember.Count == 0)
            {
                return BadRequest();
            }
            _context.CardUserMembers.Remove(checkMember.FirstOrDefault());
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
