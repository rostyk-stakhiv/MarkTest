using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ThemeCountModel:BaseModel
    {
        public int Count { get; set; }
        public string Theme { get; set; }
        public int TestId { get; set; }
    }
}
