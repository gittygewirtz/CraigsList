﻿using CraigsList.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CraigsList.Models
{
    public class IndexViewModel
    {
        public List<Post> Posts { get; set; }
        public bool LoggedIn { get; set; }
        public int UserId { get; set; }
    }
}
