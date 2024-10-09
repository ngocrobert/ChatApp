using API.DTOs;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using DatingApp2.Data;
using DatingApp2.DTOs;
using DatingApp2.Entities;
using DatingApp2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        //[AllowAnonymous]
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

        [HttpGet("{username}", Name ="GetUser")]

        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //var user = await _userRepository.GetUserByUsernameAsync(username);
            //return _mapper.Map<MemberDto>(user);

            //thay the code tren
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
            {
                // return _mapper.Map<PhotoDto>(photo);

                // trả về statuCode = 201, thiết lập location(url) tại header của photo trỏ đến username
                return CreatedAtRoute("GetUser", new {username = user.UserName} ,_mapper.Map<PhotoDto>(photo));
                
            }    

            return BadRequest("Problem adding photo!");
        }

        /// <summary>
        /// Phương thức Update ảnh chính (avt)
        /// </summary>
        /// <param name="photoId">ID ảnh</param>
        /// <returns>NoContent</returns>
        /// Created: 02/10/2024
        [HttpPut("set-main-photo/{photoId}")] 
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
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
