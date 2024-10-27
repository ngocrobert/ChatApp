using DatingApp2.Entities;

namespace DatingApp2.Entities
{
    public class UserLike
    {
        public AppUser? SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public AppUser? LikeUser { get; set; }
        public int LikedUserId { get; set; }
    }
}
