using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Generation<T> where T :Animal, new()
    {
        public Nullable<int> Age => Animals?[0]?.Age;
        public List<T> Animals { get; set; }
        public Generation(int num)
        {
            Animals = new List<T>();
            for (int i = 0; i < num; i++)
                Animals.Add(new T());
        }

    }
}
