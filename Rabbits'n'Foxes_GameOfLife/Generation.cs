using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Generation<T> where T :Animal
    {
        public int Age => Animals[0].Age;
        public List<T> Animals { get; set; }
        public Generation(List<T> a)
        {
            Animals = new List<T>(a);
        }
    }
}
