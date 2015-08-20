using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Generation<T> : IComparable where T : Animal, new()
    {
        public int? Age => Animals?[0]?.Age;
        public List<T> Animals { get; set; }
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

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
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
