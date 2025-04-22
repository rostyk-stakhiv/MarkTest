using System.Collections.Generic;

namespace Data.Entities
{
    public class Question:BaseClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> SuggestedAnswers { get; set; }
        public List<string> Answers { get; set; }
        public QuestionType Type { get; set; }
        public double Mark { get; set; }
        public string Theme { get; set; }
        public string Subject { get; set; }
        public User Owner { get; set; }
        public int OwnerId { get; set; }
        public bool Privacy { get; set; }
        public int DificultyLevel { get; set; }
    }
}
