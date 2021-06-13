using RestWithASPNET.Hypermedia;
using RestWithASPNET.Hypermedia.Abstract;
using System;
using System.Collections.Generic;

namespace RestWithASPNET.Data.VO
{
    public class UserVO
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
