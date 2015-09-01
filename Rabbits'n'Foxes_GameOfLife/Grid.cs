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
            if (c.Item1 + 1 != Size.Item1)
            {
                result.Add(Tuple.Create(c.Item1 + 1, c.Item2));
                if (c.Item2 + 1 != Size.Item2)
                    result.Add(Tuple.Create(c.Item1 + 1, c.Item2 + 1));
                if (c.Item2 != 0)
                    result.Add(Tuple.Create(c.Item1 + 1, c.Item2 - 1));

            }
            if (c.Item2 + 1 != Size.Item2)
                result.Add(Tuple.Create(c.Item1, c.Item2 + 1));
            if (c.Item1 != 0)
            {
                result.Add(Tuple.Create(c.Item1 - 1, c.Item2));
                if (c.Item2 + 1 != Size.Item2)
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
        /// <summary>
        /// a List of random floats that sum up to 1
        /// </summary>
        /// <param name="num">number of elements in the list</param>
        /// <returns></returns>
        List<float> getRates(int num)
        {
            List<float> probabs = new List<float>();
            List<int> rands = new List<int>();
            int sum = 0;
            Random randGen = new Random();
            for (int k = 0; k < num; k++)
            {
                int curRand = randGen.Next();
                sum += curRand;
                rands.Add(curRand);
            }
            foreach (int k in rands)
                probabs.Add((float)k / sum);
            return probabs;
        }
        /// <summary>
        /// Number Of Days since The Simulation Begun
        /// </summary>
        public int Date { get; private set; }
        /// <summary>
        /// Performs a one Day simulation of the Overall Grid
        /// </summary>
        public void GridOneDaySim()
        {

            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    Cells[i][j].oneDayCourse(Date);
                }
            }
            List<List<Cell>> result = new List<List<Cell>>();
            for (int i = 0; i < Cells.Count; i++)
            {
                result.Add(new List<Cell>());
                for (int j = 0; j < Cells[i].Count; j++)
                    result[i].Add(new Cell());
            }
            Random random = new Random();
            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    int numToTravel = (int)(Cells[i][j].RabbitsCount * Rabbit.RateOfTravel);
                    List<Tuple<int, int>> possibleDests = possibleRabbitDest(Tuple.Create(i, j));
                    List<float> rates = getRates(possibleDests.Count);
                    for (int dest = 0; dest < possibleDests.Count; dest++)
                    {
                        int travellersTothisDest = (int)(numToTravel * rates[dest]);
                        List<Generation<Rabbit>> destination = new List<Generation<Rabbit>>();
                        List<float> genRates = getRates(Cells[i][j].RabbitsGenerations.Count);
                        for (int gen = 0; gen < Cells[i][j].RabbitsGenerations.Count; gen++)
                        {
                            int fromThisGen = (int)(travellersTothisDest * genRates[gen]);
                            Generation<Rabbit> resGen = new Generation<Rabbit>(0);
                            for (int rab = 0; rab < fromThisGen; rab++)
                            {
                                int index = random.Next(Cells[i][j].RabbitsGenerations[rab].Count);
                                Rabbit cur = Cells[i][j].RabbitsGenerations[gen][index];
                                resGen.Animals.Add(cur);
                                Cells[i][j].RabbitsGenerations[rab].Animals.RemoveAt(index);
                            }
                            destination.Add(resGen);
                        }
                        result[possibleDests[dest].Item1][possibleDests[dest].Item2].Merge(destination);
                    }
                }
            }
            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    int numToTravel = (int)(Cells[i][j].FoxesCount * Fox.RateOfTravel);
                    List<Tuple<int, int>> possibleDests = possibleFoxDest(Tuple.Create(i, j));
                    List<float> rates = getRates(possibleDests.Count);
                    for (int dest = 0; dest < possibleDests.Count; dest++)
                    {
                        int travellersTothisDest = (int)(numToTravel * rates[dest]);
                        List<Generation<Fox>> destination = new List<Generation<Fox>>();
                        List<float> genRates = getRates(Cells[i][j].FoxesGenerations.Count);
                        for (int gen = 0; gen < Cells[i][j].FoxesGenerations.Count; gen++)
                        {
                            int fromThisGen = (int)(travellersTothisDest * genRates[gen]);
                            Generation<Fox> resGen = new Generation<Fox>(0);
                            for (int fox = 0; fox < fromThisGen; fox++)
                            {
                                int index = random.Next(Cells[i][j].FoxesGenerations[fox].Count);
                                Fox cur = Cells[i][j].FoxesGenerations[gen][index];
                                resGen.Animals.Add(cur);
                                Cells[i][j].FoxesGenerations[fox].Animals.RemoveAt(index);
                            }
                            destination.Add(resGen);
                        }
                        result[possibleDests[dest].Item1][possibleDests[dest].Item2].Merge(destination);
                    }
                    result[i][j].CopyMerge(Cells[i][j]);
                }
            }
            Date++;
        }
        public Grid(Tuple<int, int> size)
        {
            Cells = new List<List<Cell>>();
            Date = 0;
            Size = size;
            for (int i = 0; i < size.Item1; i++)
            {
                Cells.Add(new List<Cell>());
            }
        }
    }
}
