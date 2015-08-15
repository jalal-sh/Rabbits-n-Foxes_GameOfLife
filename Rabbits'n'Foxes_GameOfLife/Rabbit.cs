using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Rabbit
    {
        /// <summary>
        /// Age of The Rabbit (Represented as Days)
        /// </summary>
        public int Age { get; private set; }
        /// <summary>
        /// Time (in Days) between each 2 Multiplication Seasons
        /// </summary>
        public static int MultiplicationInterval { get; set; }
        /// <summary>
        /// Expected Age of Rabbits
        /// </summary>
        public static int MedianAge { get; set; }
        
    }
}
