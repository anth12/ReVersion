﻿using System;

namespace ReVersion.Services.Subversion.Requests
{
    public class ListRepositoriesRequest
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
