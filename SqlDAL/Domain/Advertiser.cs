using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    public class Advertiser
    {
        public int Id { get; set; }
        public Member User { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public bool IsAccepted { get; set; }
        public List<Video> Videos { get; set; }
    }
}
