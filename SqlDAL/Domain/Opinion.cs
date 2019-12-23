using System;

namespace SqlDAL.Domain
{
    // Give an opinion on a topic by a User
    public class Opinion
    {
        public int Id { get; set; }
        public Topic Topic { get; set; }
        public string TheOpinion { get; set; }
        public User User { get; set; }
        public DateTime Dob { get; set; }
    }
}
