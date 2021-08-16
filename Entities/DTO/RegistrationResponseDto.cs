﻿using System.Collections.Generic;

namespace Entities.DTO
{
    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public string UserId { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
