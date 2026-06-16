using System;
using System.Collections.Generic;
using System.Text;

namespace RealTalk.Shared.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
    }
}
