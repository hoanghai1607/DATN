using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WebProjectManager.Common.Authentication;
using WebProjectManager.Common.Helper;
using WebProjectManager.Common.ViewModel;
using WebProjectManager.Models.EF;
using WebProjectManager.Models.Entities;

namespace WebProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly EFCoreDbContext _context;

        private string accountSid = "";
        private string authToken = "";

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager,
            IConfiguration configuration, EFCoreDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, model.Password);
                if (!isCorrect)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var jwtToken = GenerateJwtToken(existingUser.Email, existingUser);
                return Ok(new AuthResult()
                {
                    Success = true,
                    Token = jwtToken.Result
                });


            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<object> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var emailExists = await _userManager.FindByEmailAsync(model.Email);

                if (emailExists != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Email already in use"
                            },
                        Success = false
                    });
                }

                var phoneNumberExist = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);

                if (phoneNumberExist != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Phone Number already in use"
                            },
                        Success = false
                    });
                }


                var newId = Guid.NewGuid();
                Random _code = new Random();
                var user = new User
                {
                    Id = newId,
                    Lastname = model.Lastname,
                    Firstname = model.Firstname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = false,
                    Gender = model.Gender,
                    UserName = model.Email,
                    IsVerification = true,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(user.Email, user);
                    return Ok(new AuthResult()
                    {
                        Success = true,
                        Token = jwtToken.Result
                    });
                    
                }
                else
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = result.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }


            }

            return BadRequest();
        }

        [HttpGet("")]
        public async Task<ActionResult<User>> GetProfile()
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var data = await _context.Users.FindAsync(Guid.Parse(userId));

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);

        }

        [HttpPut("")]
        public async Task<ActionResult<User>> PutProfile(UserViewModel model)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var data = await _context.Users.FindAsync(Guid.Parse(userId));

            if (data == null)
            {
                return NotFound();
            }
            if (model.Lastname != null && model.Lastname != "")
            {
                data.Lastname = model.Lastname;
            }
            if (model.Firstname != null && model.Firstname != "")
            {
                data.Firstname = model.Firstname;
            }
            if (model.DateOfBirth != null)
            {
                data.DateOfBirth = model.DateOfBirth;
            }
            if (model.Address != null && model.Address != "")
            {
                data.Address = model.Address;
            }
            if (model.Gender != null)
            {
                data.Gender = model.Gender;
            }
            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(data);

        }

        [HttpGet("FindUser/{phoneNumber}")]
        public async Task<ActionResult<User>> GetUser(string phoneNumber)
        {
            var userRequest = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

            if (userRequest == null)
            {
                return BadRequest();
            }
            return Ok(userRequest);
        }

        [HttpGet("AccountVerification")]
        public async Task<ActionResult<AccountVerification>> AccountVerification()
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;

            System.Diagnostics.Debug.WriteLine(userId);

            var userRequest = await _context.Users.FindAsync(Guid.Parse(userId));

            System.Diagnostics.Debug.WriteLine(userRequest);

            if (userRequest == null)
            {
                return NotFound();
            }

            if (userRequest.PhoneNumberConfirmed == false)
            {
                var checkExists = _context.AccountVerifications.Where(x => x.IdUser == Guid.Parse(userId)).ToList();
                Random _code = new Random();
                string code = _code.Next(100000, 999999).ToString();
                if (checkExists.Count == 0)
                {
                    var verifi = new AccountVerification
                    {
                        Id = Guid.NewGuid(),
                        IdUser = Guid.Parse(userId),
                        CreateOn = DateTime.Now,
                        ExpiryOn = DateTime.Now.AddMinutes(10),
                        VerificationWith = true,
                        Code = code,
                    };
                    _context.AccountVerifications.Add(verifi);
                    await _context.SaveChangesAsync();

                    String _phoneNumber = userRequest.PhoneNumber.Substring(1);
                    _phoneNumber = "+84" + _phoneNumber;
                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Mã xác thực tài khoản của bạn là " + code,
                        from: new Twilio.Types.PhoneNumber("+19344515623"),
                        to: new Twilio.Types.PhoneNumber(_phoneNumber)
                    );
                    Content(message.Sid);
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Đã gửi mã",
                            },
                        Success = true
                    });
                }
                if (checkExists.Count == 1)
                {
                    var data = checkExists.FirstOrDefault();
                    data.CreateOn = DateTime.Now;
                    data.ExpiryOn = DateTime.Now.AddMinutes(10);
                    data.Code = code;
                    _context.Entry(data).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    String _phoneNumber = userRequest.PhoneNumber.Substring(1);
                    _phoneNumber = "+84" + _phoneNumber;
                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Mã xác thực tài khoản của bạn là " + code,
                        from: new Twilio.Types.PhoneNumber("+19344515623"),
                        to: new Twilio.Types.PhoneNumber(_phoneNumber)
                    );
                    Content(message.Sid);
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Đã gửi mã",
                            },
                        Success = true
                    });
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost("AccountVerification")]
        public async Task<ActionResult<AccountVerification>> CheckVerification(Verification model)
        {
            if (ModelState.IsValid)
            {
                string tokenString = Request.Headers["Authorization"].ToString();
                var infoFromToken = Auths.GetInfoFromToken(tokenString);
                var userId = infoFromToken.Result.UserId;
                var checkExists = _context.AccountVerifications.Where(x => x.IdUser == Guid.Parse(userId)).FirstOrDefault();
                if (checkExists != null)
                {
                    if (checkExists.Code!=model.Code)
                    {
                        return BadRequest();
                    }
                    if (checkExists.ExpiryOn < DateTime.Now)
                    {
                        return BadRequest();
                    }
                    var userRequest = await _context.Users.FindAsync(Guid.Parse(userId));
                    userRequest.PhoneNumberConfirmed = true;
                    userRequest.UpdatedOn = DateTime.UtcNow;
                    _context.Entry(userRequest).State = EntityState.Modified;
                    _context.Remove(checkExists);
                    await _context.SaveChangesAsync();
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Xác thực số điện thoại thành công",
                            },
                        Success = true
                    });
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("ForgotPasswordCode")]
        public async Task<ActionResult<AccountVerification>> ForgotPassword(ForgotPassword model)
        {

            var userRequest = await _userManager.FindByEmailAsync(model.Email);

            if (userRequest.PhoneNumberConfirmed == true)
            {
                var checkExists = _context.AccountVerifications.Where(x => x.IdUser == userRequest.Id).ToList();
                Random _code = new Random();
                string code = _code.Next(100000, 999999).ToString();
                if (checkExists.Count == 0)
                {
                    var verifi = new AccountVerification
                    {
                        Id = Guid.NewGuid(),
                        IdUser = userRequest.Id,
                        CreateOn = DateTime.Now,
                        ExpiryOn = DateTime.Now.AddMinutes(10),
                        VerificationWith = true,
                        Code = code,
                    };
                    _context.AccountVerifications.Add(verifi);
                    await _context.SaveChangesAsync();

                    String _phoneNumber = userRequest.PhoneNumber.Substring(1);
                    _phoneNumber = "+84" + _phoneNumber;
                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Mã xác thực tài khoản của bạn là " + code,
                        from: new Twilio.Types.PhoneNumber("+19344515623"),
                        to: new Twilio.Types.PhoneNumber(_phoneNumber)
                    );
                    Content(message.Sid);
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Đã gửi mã",
                            },
                        Success = true
                    });
                }
                if (checkExists.Count == 1)
                {
                    var data = checkExists.FirstOrDefault();
                    data.CreateOn = DateTime.Now;
                    data.ExpiryOn = DateTime.Now.AddMinutes(10);
                    data.Code = code;
                    _context.Entry(data).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    String _phoneNumber = userRequest.PhoneNumber.Substring(1);
                    _phoneNumber = "+84" + _phoneNumber;
                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Mã xác thực tài khoản của bạn là " + code,
                        from: new Twilio.Types.PhoneNumber("+19344515623"),
                        to: new Twilio.Types.PhoneNumber(_phoneNumber)
                    );
                    Content(message.Sid);
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Đã gửi mã",
                            },
                        Success = true
                    });
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult<AccountVerification>> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                string tokenString = Request.Headers["Authorization"].ToString();
                var infoFromToken = Auths.GetInfoFromToken(tokenString);
                var userId = infoFromToken.Result.UserId;
                var userExists = await _userManager.FindByIdAsync(userId);
                var isCorrect = await _userManager.CheckPasswordAsync(userExists, model.CurrentPassword);
                if (!isCorrect)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Current password is not correct"
                            },
                        Success = false
                    });
                }
                if (model.NewPassword != model.CheckPassword)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "New password and re-enter password do not match"
                            },
                        Success = false
                    });
                }
                await _userManager.ChangePasswordAsync(userExists,model.CurrentPassword,model.NewPassword);
                return Ok(new AuthResult()
                {
                    Errors = new List<string>() {
                                "Đã đổi mật khẩu thành công",
                            },
                    Success = true
                });
                
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<AccountVerification>> ForgotPassword(VerifiPassword model)
        {
            if (ModelState.IsValid)
            {
                var userRequest = await _userManager.FindByEmailAsync(model.Email);
                var checkExists = _context.AccountVerifications.Where(x => x.IdUser == userRequest.Id).FirstOrDefault();
                if (checkExists != null)
                {
                    if (checkExists.Code != model.Code)
                    {
                        return BadRequest();
                    }
                    if (checkExists.ExpiryOn < DateTime.Now)
                    {
                        return BadRequest();
                    }

                    var userExists = await _userManager.FindByIdAsync(userRequest.Id.ToString());
                    string code = await _userManager.GeneratePasswordResetTokenAsync(userExists);
                    string passwordNew = RandomPassword.RandomString(10);
                    await _userManager.ResetPasswordAsync(userExists, code, passwordNew);
                    _context.Remove(checkExists);
                    await _context.SaveChangesAsync();
                    String _phoneNumber = userExists.PhoneNumber.Substring(1);
                    _phoneNumber = "+84" + _phoneNumber;
                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Mật khẩu mới của bạn là : " + passwordNew,
                        from: new Twilio.Types.PhoneNumber("+19344515623"),
                        to: new Twilio.Types.PhoneNumber(_phoneNumber)
                    );
                    Content(message.Sid);
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Đã đổi mật khẩu thành công",
                            },
                        Success = true
                    });
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("UploadImage")]
        public async Task<ActionResult<User>> PutImageProduct(IFormFile file)
        {
            Console.WriteLine("upload file!");
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var avatars = UploadImage.UploadImageFile(file);
            User getUser = await _context.Users.FindAsync(Guid.Parse(userId));  
            getUser.UrlAvatar = avatars.ToString();
            _context.Entry(getUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(getUser);
        }

        private Task<string> GenerateJwtToken(string email, User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JwtExpireHours"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return System.Threading.Tasks.Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<User>> GetUserProfile(Guid id)
        {
            string tokenString = Request.Headers["Authorization"].ToString();
            var infoFromToken = Auths.GetInfoFromToken(tokenString);
            var userId = infoFromToken.Result.UserId;
            var data = await _context.Users.FindAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);

        }
    }
}
