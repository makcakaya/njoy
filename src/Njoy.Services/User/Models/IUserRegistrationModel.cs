﻿namespace Njoy.Services
{
    public interface IUserRegistrationModel
    {
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string PasswordConfirm { get; set; }
    }
}