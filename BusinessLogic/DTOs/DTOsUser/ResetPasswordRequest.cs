﻿namespace BusinessLogic.DTOs.DTOsUser
{
    public class ResetPasswordRequest
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}
