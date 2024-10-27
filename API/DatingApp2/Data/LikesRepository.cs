using API.DTOs;
using API.Helpers;
using API.Interfaces;
using DatingApp2.Data;
using DatingApp2.Entities;
using DatingApp2.Extensions;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Lấy thông tin Like giữa 2 ng dùng
        /// </summary>
        /// <param name="sourceUserId"></param>
        /// <param name="likedUserId"></param>
        /// <returns></returns>
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        /// <summary>
        /// Lấy danh sách người dùng mà 1 người đã like hoặc đã được like
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            // tìm tất cả người dùng mà userid đã like
            if(likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikeUser);
            }

            // tìm tất cả người dùng đã like userid
            if(likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedByUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
