﻿using System;

namespace SqlDAL.Domain
{
    public class Like
    {
        public int Id { get; set; }

        public Member Member { get; set; }

        public Opinion Opinion { get; set; }

        public DateTime Dob { get; set; }
    }
}
