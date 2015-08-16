using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Rabbit : Animal
    {
        /// <summary>
        /// Time (in Days) between each 2 Multiplication Seasons
        /// </summary>
        public static int MultiplicationInterval { get; set; }
        /// <summary>
        /// Expected Age of Rabbits based on Vegetation Level
        /// </summary>
        public static Distribution<float, int> MedianAge { get; private set; }
        /// <summary>
        /// The number of Rabbits born per couple based on vegetation level
        /// And the Number of rabbits in the current context
        /// </summary>
        public static Distribution<int, float, int> BirthRate { get; private set; }
        public Rabbit():base()
        {

        }
    }
}
