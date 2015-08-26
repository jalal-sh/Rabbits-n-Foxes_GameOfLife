using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Animal:ICloneable
    {
        
        /// <summary>
        /// a Unique ID for each animal so We can decide whether or not 2 animals are the same Animals
        /// </summary>
        public long ID { get; private set; }
        private static long IDCounter { get; set; }
        /// <summary>
        /// Age of The Rabbit (Represented as Days)
        /// </summary>
        public int Age { get; protected set; }
        /// /// <summary>
        /// On the End of Day Rabbits Ages One More Day
        /// </summary>
        public void AgeOneDay()
        {
            Age++;
        }
        /// <summary>
        /// Defines whether the 2 animals are Actually the same one depending on thier IDs
        ///  </summary>
        /// <param name="obj"> another animal</param>
        /// <returns>
        /// True if <paramref name="obj"/> is the same animal as this
        /// false otherwise
        /// </returns>
        ///<exception cref="ArgumentException">if <paramref name="obj"/> is not an animal</exception>
        public override bool Equals(object obj)
        {
            if (obj is Animal)
            {
                Animal c = (Animal)obj;
                return c.ID == ID;
            }
            throw new ArgumentException("is not an Animal ", nameof(obj));
        }

        public virtual object Clone()
        {
            return new Animal(this);
        }

        protected Animal()
        {
            Age = 0;
            ID = IDCounter++;
        }
        public Animal(Animal cpy)
        {
            ID = cpy.ID;
            Age = cpy.Age;
        }
    }
}
