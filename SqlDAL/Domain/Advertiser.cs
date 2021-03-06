﻿using System;
using System.Collections.Generic;

namespace SqlDAL.Domain
{
    public class Advertiser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime Dob { get; set; }
    }
}
