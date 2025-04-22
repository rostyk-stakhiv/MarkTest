using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class Answer:BaseClass
    {
        public int QuestionId { get; set; }
        public List<string> Answers { get; set; }
    }
}
