using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class TestModel:BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Durability { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public List<ThemeCountModel> ThemeCount { get; set; }
        public List<Question> Questions { get; set; }
        public double Mark { get; set; }
        public double MaxMark { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public int NumberOfAttempts { get; set; }
        public int DificultyLevel { get; set; }
        public UserModel User { get; set; }
        public UserModel Owner { get; set; }
    }
}
