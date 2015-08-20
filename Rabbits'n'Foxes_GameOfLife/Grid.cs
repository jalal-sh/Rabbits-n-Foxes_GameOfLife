using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Grid
    {
        /// <summary>
        /// Size of the Grid
        /// </summary>
        public Tuple<int, int> Size { get; set; }
        /// <summary>
        /// The Cells in The Grid
        /// </summary>
        public List<List<Cell>> Cells { get; private set; }
        /// <summary>
        /// Possible Destination of The Rabbit which currently resides at Cell <paramref name="c"/> 
        /// </summary>
        /// <param name="c">The Cell in which the Rabbit is Currently</param>
        /// <returns>
        /// Index of possible Destinations
        /// </returns>
        private List<Tuple<int, int>> possibleRabbitDest(Tuple<int, int> c)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            result.Add(Tuple.Create(c.Item1, c.Item2));
            if (c.Item1 != Size.Item1)
            {
                result.Add(Tuple.Create(c.Item1 + 1, c.Item2));
                if (c.Item2 != Size.Item2)
                    result.Add(Tuple.Create(c.Item1 + 1, c.Item2 + 1));
                if (c.Item2 != 0)
                    result.Add(Tuple.Create(c.Item1 + 1, c.Item2 - 1));

            }
            if (c.Item2 != Size.Item2)
                result.Add(Tuple.Create(c.Item1, c.Item2 + 1));
            if (c.Item1 != 0)
            {
                result.Add(Tuple.Create(c.Item1 - 1, c.Item2));
                if (c.Item2 != Size.Item2)
                    result.Add(Tuple.Create(c.Item1 - 1, c.Item2 + 1));
                if (c.Item2 != 0)
                    result.Add(Tuple.Create(c.Item1 - 1, c.Item2 - 1));
            }
            if (c.Item2 != 0)
                result.Add(Tuple.Create(c.Item1, c.Item2 - 1));
            return result;
        }
        /// <summary>
        /// Possible Destination of The Fox which currently resides at Cell <paramref name="c"/> 
        /// </summary>
        /// <param name="c">The Cell in which the Fox is Currently</param>
        /// <returns>
        /// Index of possible Destinations
        /// </returns>
        private List<Tuple<int, int>> possibleFoxDest(Tuple<int, int> c)
        {
            List<Tuple<int, int>> oneStep = possibleRabbitDest(c);

            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            foreach (Tuple<int, int> d in oneStep)
            {
                result.AddRange(possibleRabbitDest(d));
            }
            return result.Distinct().ToList();
        }
        public void GridOneDaySim()
        {
            
        }
    }
}
