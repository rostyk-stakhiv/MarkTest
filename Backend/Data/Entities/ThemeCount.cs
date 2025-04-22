using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class ThemeCount:BaseClass
    {
        public int Count { get; set; }
        public string Theme { get; set; }
        public int TestId { get; set; }
    }
}
