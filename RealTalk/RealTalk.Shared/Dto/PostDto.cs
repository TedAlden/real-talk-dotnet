using System;
using System.Collections.Generic;
using System.Text;

namespace RealTalk.Shared.Dto
{
    public class PostDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public UserDto? User { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? Content { get; set; }
    }
}
