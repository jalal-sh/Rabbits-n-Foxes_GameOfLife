using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Generation<T> : IComparable where T : Animal, new()
    {
        public int Count => Animals.Count;
        public T this[int k]
        {
            get
            {
                return Animals[k];
            }
            set
            {
                Animals[k] = value;
            }
        }
        public int? Age => (Animals?.Count>0)?(Animals?[0].Age):null;
        public List<T> Animals { get; set; }
        public Generation(Generation<T> c)
        {
            Animals = new List<T>();
            foreach (T t in c.Animals)
                Animals.Add((T)t.Clone());
        }
        public Generation(int num)
        {
            Animals = new List<T>();
            for (int i = 0; i < num; i++)
                Animals.Add(new T());
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            return Age == ((Generation<T>)(obj)).Age;
        }
        public int CompareTo(object obj)
        {
            if (obj is Generation<T>)
            {
                Generation<T> objT = (Generation<T>)obj;
                if (Age == objT.Age)
                    return 0;
                else if (Age < objT.Age)
                    return -1;
                else
                    return 1;
            }
            throw new ArgumentException("is not a Generation of the Same Animal ", nameof(obj));

        }
    }
}
