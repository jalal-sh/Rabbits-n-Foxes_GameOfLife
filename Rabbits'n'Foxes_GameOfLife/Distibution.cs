using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Distribution<TKey,TValue> where TKey : IComparable
    {

    }
    public class Distribution<TKey1, TKey2, TValue> where TKey1 : IComparable where TKey2 : IComparable
    {
        private List<TKey2> horizentalKeys;
        Distribution<TKey1, TValue> vals;
        public Distribution<TKey2,TValue> this[TKey2 k]
        {
            get
            {

            }
            set
            {

            }
        }

    }
}