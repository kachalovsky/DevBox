using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevBox.Models
{
    public class AddInfo
    {
        public Question Question { get; set; }
        public Answers Answer { get; set; }
        public IEnumerable<Tips> Tips { get; set; }
        public IEnumerable<Categories> Categories { get; set; }
        public int PuzzleId { get; set; }
        public int Num { get; set; }
        public int All { get; set; }
    }
}