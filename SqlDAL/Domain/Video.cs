using System;

namespace SqlDAL.Domain
{
    public class Video
    {
        public long Id { get; set; }
        public Advertiser Advertiser { get; set; }
        public string Url { get; set; }
        public DateTime Dob { get; set; }
     }
}
