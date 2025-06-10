namespace RuWitter1.Server.Models
{
    public class MessageDTO
    {
        public int chatId {  get; set; }
        public string? Content { get; set; }
        public IFormFile? File { get; set; }
    }
}
