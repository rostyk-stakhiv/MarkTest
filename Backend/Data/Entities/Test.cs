using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Test:BaseClass
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Durability { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public List<ThemeCount> ThemeCount { get; set; }
        public List<Question> Questions { get; set; }
        public double Mark { get; set; }
        public double MaxMark { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public int NumberOfAttempts { get; set; }
        public int DificultyLevel { get; set; }
    }
}
