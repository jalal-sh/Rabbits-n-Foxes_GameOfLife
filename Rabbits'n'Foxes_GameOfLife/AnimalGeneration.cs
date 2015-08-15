using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class AnimalGeneration
    {
        public int Age { get; private set; }
        public int SinceLastMultiplication { get; private set; }
        public static int MultiplicationInterval { get; private set; }
             
    }
}
