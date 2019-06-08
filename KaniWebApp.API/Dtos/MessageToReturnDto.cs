using System;

namespace KaniWebApp.API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderNickname { get; set; }
        public string SenderImageUrl { get; set; }
        public int RecipientId { get; set; }
        public string RecipientNickname { get; set; }
        public string RecipientImageUrl { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime SentDate { get; set; }

    }
}