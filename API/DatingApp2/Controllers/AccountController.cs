using AutoMapper;
using DatingApp2.Data;
using DatingApp2.DTOs;
using DatingApp2.Entities;
using DatingApp2.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp2.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        //private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        // POST: api/account/register?username=dave&password=pwd
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.UserName))
            {
                return BadRequest("Username is taken");
            }

            var user = _mapper.Map<AppUser>(registerDto);

            //dung Identity thay the
            //using var hmac = new HMACSHA512();

            //var user = new AppUser
            //{
            user.UserName = registerDto.UserName.ToLower();

            // dung Identity thay the
            //user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            //    user.PasswordSalt = hmac.Key;

            //};

            //_context.Users.Add(user);
            //await   _context.SaveChangesAsync();
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);
            
            return new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            //Dung Identity thay the
            //using var hmac = new HMACSHA512(user.PasswordSalt);

            //var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //for(int i=0; i<computedHash.Length; i++)
            //{
            //    if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            //}

            //return user;

            return new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

        }

        private async Task<bool> UserExists(string username)
        {
            //return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());

        }
    }
}
