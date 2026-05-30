namespace RuWitter1.Server.Models
{
    public class PostLikeDTO
    {
        public int PostId {  get; set; }
        public int? CommentId { get; set; } = null;
    }
}
