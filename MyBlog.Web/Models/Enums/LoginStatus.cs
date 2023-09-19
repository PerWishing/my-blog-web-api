﻿namespace MyBlog.Web.Models.Enums
{
    public enum LoginStatus
    {
        UserNotFound = 0,
        Succeeded = 1,
        WrongPassword = 2,
        IsBlocked = 3
    }
}
