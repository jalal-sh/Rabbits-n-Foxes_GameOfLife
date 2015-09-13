using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Rabbit : Animal
    {
        #region Static Members

        /// <summary>
        /// The Max Distance a Rabbit May Travel
        /// </summary>
        public static int TravellingDistance { get; private set; }

        /// <summary>
        /// Time (in Days) between each 2 Multiplication Seasons
        /// </summary>
        public static int MultiplicationInterval { get; private set; }

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
        /// Ratio of Rabbits that travel to neighbouring Cells
        /// </summary>
        public static float RateOfTravel { get; private set; }
        #endregion

        /// <summary>
        /// Initialize The Static Members in Class Rabbits
        /// </summary>
        /// <param name="MultiInterval">Time (in Days) between each 2 Multiplication Seasons</param>
        /// <param name="Median">Expected Age of Rabbits based on Vegetation Level</param>
        /// <param name="BRate">The number of Rabbits born per couple based on vegetation level
        /// <param name="trRate"> Ratio Of Rabbits which travel to neighbouring Cells</param>
        /// <param name="trDist">the Distance a Rabbit Can travel</param>
        /// And the Number of rabbits in the current context</param>
        public static void init(int MultiInterval, Distribution<float, int> Median, Distribution<int, float, int> BRate, float trRate, int trDist)
        {
            MultiplicationInterval = MultiInterval;
            MedianAge = Median;
            BirthRate = BRate;
            RateOfTravel = trRate;
            TravellingDistance = trDist;
        }

        public override object Clone()
        {
            return new Rabbit(this);
        }

        public Rabbit() : base()
        {

        }

        public Rabbit(Rabbit r) : base(r)
        {

        }
    }
}