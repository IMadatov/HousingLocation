﻿using HousingLocation.Models;

namespace HousingLocation.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserLastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
