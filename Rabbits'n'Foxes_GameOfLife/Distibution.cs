using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    /// <summary>
    /// Distribution Table Class
    /// </summary>
    /// <typeparam name="TKey"> Type of Keys of the Table</typeparam>
    /// <typeparam name="TValue">Type of Values of the Table</typeparam>
    public class Distribution<TKey, TValue> where TKey : IComparable
    {
        private List<TKey> keys;
        private List<TValue> values;

        public TValue this[TKey k]
        {
            get
            {
                int i = keys.BinarySearch(k);
                if (i < 0)
                    i = ~i;
                if (i == keys.Count)
                    i--;
                return values[i];
            }
            set
            {
                /// Return the Place in Which the key k exists or the binary complement of the Location it's supposed to be
                int i = keys.BinarySearch(k);
                if (i < 0)
                {
                    i = ~i;
                    keys.Insert(i, k);

                    values.Insert(i, value);
                }
                else
                    values[i] = value;
            }
        }
        public Distribution()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }
        public Distribution(List<TKey> keys,List<TValue> values)
        {
            this.keys = keys;
            this.values = values;
        }

    }
    /// <summary>
    /// Two Dimensional Distribution
    /// </summary>
    /// <typeparam name="TKey1"> Type of horizental Keys of the Table</typeparam>
    /// <typeparam name="TKey2"> Type of vertical Keys of the Table</typeparam>
    /// <typeparam name="TValue"> Type of Values represented by the Table</typeparam>
    public class Distribution<TKey1, TKey2, TValue> : Distribution<TKey1, Distribution<TKey2, TValue>> where TKey1 : IComparable where TKey2 : IComparable
    {
        public Distribution() : base()
        {

        }
        
    }
}