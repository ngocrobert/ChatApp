using AutoMapper;
using DatingApp2.Data;
using DatingApp2.DTOs;
using DatingApp2.Entities;
using DatingApp2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp2.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUers()
        {
            //var users = await _userRepository.GetUsersAsync();

            //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            //return Ok(usersToReturn);

            var users = await _userRepository.GetMembersAsync();
            return Ok(users);


        }

        
        //[HttpGet("{id}")]
        
        //public async Task<ActionResult<AppUser>> GetUser(int id)
        //{
        //    var user = await _userRepository.GetUserByIdAsync(id);

        //    return user;
        //}

        [HttpGet("{username}")]

        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //var user = await _userRepository.GetUserByUsernameAsync(username);
            //return _mapper.Map<MemberDto>(user);

            //thay the code tren
            return await _userRepository.GetMemberAsync(username);
        }

        //[HttpPost]
        //public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        //{

        //    if(await UserExists(registerDto.UserName))
        //    {
        //        return BadRequest("Username is taken");
        //    }

        //    using var hmac = new HMACSHA512();
        //    var user = new AppUser
        //    {
        //        UserName = registerDto.UserName.ToLower(),
        //        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        //        PasswordSalt = hmac.Key
        //    };

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}

        //private async Task<bool> UserExists(string username)
        //{
        //    return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        //}
    }
}
