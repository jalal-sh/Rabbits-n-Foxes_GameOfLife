using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Fox:Animal
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
        /// <summary>
        /// Deterimine the Level of Vegetation above which finding rabbits becomes Hard
        /// </summary>
        public static int PlantationCoverRabbits { get; private set; }
        /// <summary>
        /// porpabilty of Rabbit being Eaten when the vegetation cover is low
        /// </summary>
        public static int MinFoodProp { get; private set; }
        /// <summary>
        /// porpabilty of Rabbit being Eaten when the vegetation cover is High
        /// </summary>
        public static int MaxFoodProp { get; private set; }
        /// <summary>
        /// Number of Rabbits of which The Fox can't eat anymore 
        /// </summary>
        public static int MaxFoodAllowedWeekly { get; private set; }
        public Fox():base()
        {

        }

    }
}
