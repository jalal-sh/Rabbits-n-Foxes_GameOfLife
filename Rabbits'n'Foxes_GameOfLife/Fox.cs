using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Fox : Animal
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
        /// Number of Rabbits of which The Fox can't eat anymore when there's enough Cover
        /// </summary>
        public static int MaxFoodAllowedWeekly { get; private set; }
        /// <summary>
        /// Number of Rabbits of which The Fox can't eat anymore when there's NOT enough Cover
        /// </summary>
        public static int MinFoodAllowedWeekly { get; private set; }
        /// <summary>
        /// The Food Required for a fox weekly so it does NOT become hungry
        /// </summary>
        public static int RequiredWeekly { get; private set; }
        /// <summary>
        /// if Hungery the Fox would be more vulneraible to Death
        /// </summary>
        public bool Hungry => RequiredWeekly > EatenThisWeek;
        /// <summary>
        /// The Probabilty of Death by Hunger
        /// </summary>
        public static float HungerDeathPropabilty { get; private set; }
        /// <summary>
        /// Number of Rabbits The Fox have Eaten each day in the Past Week
        /// </summary>
        public int[] EatenEachDay { get; set; }
        /// <summary>
        /// Sum of all the Rabbits that the fox have eaten the past week
        /// </summary>
        public int EatenThisWeek { get; private set; }
        /// <summary>
        /// Simulates the satisfaction by eating <paramref name="num"/> Rabbits at the <paramref name="Day"/>
        /// </summary>
        /// <param name="num">number of rabbits to eat</param>
        /// <param name="Day">date on which the rabbits are being eaten</param>
        public void Eat(int num, int Day)
        {
            int x = EatenEachDay[Day % 7];
            EatenEachDay[Day % 7] = num;
            EatenThisWeek += num - x;
        }
        /// <summary>
        /// Initialize the Static Members in the Class Fox
        /// </summary>
        public static void init()
        {

        }
        public Fox() : base()
        {
            EatenEachDay = new int[7];
        }

    }
}
