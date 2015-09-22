using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;


namespace GameOfLife
{
    public class GameGrid

    {
        #region Properties

        /// <summary>
        /// Total Count of Foxes on the grid
        /// </summary>
        public int TotalFoxesCount { get; private set; }

        /// <summary>
        /// Total Count of Rabbits on the grid
        /// </summary>
        public int TotalRabbitsCount { get; private set; }

        /// <summary>
        /// Size of the Grid
        /// </summary>
        public Tuple<int, int> Size { get; set; }


        /// <summary>
        /// Number Of Days since The Simulation Begun
        /// </summary>
        public int Date { get; private set; }

        /// <summary>
        /// The Cells in The Grid
        /// </summary>
        public List<List<GameCell>> Cells { get; private set; }
        #endregion      

        #region Methods
        /// <summary>
        /// Possible Destination of The Animal which currently resides at Cell <paramref name="c"/> 
        /// </summary>
        /// <param name="c">The Cell in which the Rabbit is Currently</param>
        /// <param name="trDist"> The Distance the Animal can Travel</param>
        /// <returns>
        /// Index of possible Destinations
        /// </returns>
        private List<Tuple<int, int>> possibleDest(Tuple<int, int> c, int trDist)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            result.Add(Tuple.Create(c.Item1, c.Item2));
            for (int i = 0; i < trDist; i++)
            {
                if (c.Item1 + i < Size.Item1)
                {
                    result.Add(Tuple.Create(c.Item1 + i, c.Item2));
                    if (c.Item2 + i < Size.Item2)
                        result.Add(Tuple.Create(c.Item1 + i, c.Item2 + i));
                    if (c.Item2 - i >= 0)
                        result.Add(Tuple.Create(c.Item1 + i, c.Item2 - i));

                }
                if (c.Item2 + i < Size.Item2)
                    result.Add(Tuple.Create(c.Item1, c.Item2 + i));
                if (c.Item1 - i >= 0)
                {
                    result.Add(Tuple.Create(c.Item1 - i, c.Item2));
                    if (c.Item2 + i < Size.Item2)
                        result.Add(Tuple.Create(c.Item1 - i, c.Item2 + i));
                    if (c.Item2 - i >= 0)
                        result.Add(Tuple.Create(c.Item1 - i, c.Item2 - i));
                }
                if (c.Item2 - i >= 0)
                    result.Add(Tuple.Create(c.Item1, c.Item2 - i));


            }
            return result;
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
        /// Perform a Simulation of the Grid for <paramref name="time"/> amount of time
        /// using <paramref name="threads"/> Thread (Parallel if <paramref name="threads"/> is 1
        /// </summary>
        /// <param name="time">number of days of the simulation</param>
        /// <param name="threads">number of thread to use in Simulation</param>
        public void GridSim(int time, uint threads)
        {
            if (threads == 0)
                throw new ArgumentOutOfRangeException(nameof(threads), 0, "Number of threads Can't be Zero");

            Barrier barrier = new Barrier((int)threads);
            ThreadPool.SetMaxThreads((int)threads, (int)threads);
            ThreadParam.bar = barrier;

            for (int i = 0; i < time; i++)
            {
                GridOneDaySim_Parallel(threads);
            }

        }

        /// <summary>
        /// Parameters of the Threaded Method
        /// </summary>
        private class ThreadParam
        {
            public static Barrier bar;
            public Tuple<int, int> start;
            public int count;
            public ThreadParam(int c, int si, int sj)
            {
                start = new Tuple<int, int>(si, sj);
                count = c;
            }
        }

        /// <summary>
        /// Process a part of the Grid
        /// </summary>
        /// <param name="o">ThreadParam object to Provide needed Data</param>
        private void PartialGridSimulation(object o)
        {
            Tuple<int, int> start = ((ThreadParam)o).start;
            int count = ((ThreadParam)o).count;
            for (int i = start.Item1, j = start.Item2; count > 0; count--)
            {
                if (j >= Size.Item2)
                {
                    i++;
                    j = 0;
                }
                int oldRabbitCount = Cells[i][j].RabbitsCount;
                int oldFoxesCount = Cells[i][j].FoxesCount;
                Cells[i][j].oneDayCourse(Date);
                lock (this as object)
                {
                    TotalFoxesCount += Cells[i][j].FoxesCount - oldFoxesCount;
                    TotalRabbitsCount += Cells[i][j].RabbitsCount - oldRabbitCount;
                }


            }
            ThreadParam.bar.SignalAndWait();
        }

        /// <summary>
        /// Perform a parallel one day sim of the Grid using <paramref name="threadCount"/> Threads
        /// </summary>
        /// <param name="threadCount">Number of threads to use in the simulation process</param>
        private void GridOneDaySim_Parallel(uint threadCount)
        {
            int si, sj;
            int perThread = (int)(Size.Item1 * Size.Item2 / threadCount);
            for (int i = 0; i < threadCount - 1; i++)
            {
                si = (perThread * i) % Size.Item1;
                sj = (perThread * i) / Size.Item1;
                ThreadPool.QueueUserWorkItem(PartialGridSimulation, new ThreadParam(perThread, si, sj));
            }
            si = (perThread * ((int)threadCount - 1)) % Size.Item1;
            sj = (perThread * ((int)threadCount - 1)) / Size.Item1;
            int count = perThread + Size.Item1 * Size.Item2 % (int)threadCount;
            ThreadPool.QueueUserWorkItem(PartialGridSimulation, new ThreadParam(count, si, sj));
            MoveAnimals();
        }

        /// <summary>
        /// Performs a one Day simulation of the Overall Grid
        /// </summary>
        public void GridOneDaySim()
        {

            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    int oldRabbitCount = Cells[i][j].RabbitsCount;
                    int oldFoxesCount = Cells[i][j].FoxesCount;
                    Cells[i][j].oneDayCourse(Date);
                    TotalFoxesCount += Cells[i][j].FoxesCount - oldFoxesCount;
                    TotalRabbitsCount += Cells[i][j].RabbitsCount - oldRabbitCount;
                }

            }
            MoveAnimals();
            Date++;
        }

        /// <summary>
        /// Displace animals according to the rules
        /// </summary>
        private void MoveAnimals()
        {
            List<List<GameCell>> result = new List<List<GameCell>>();
            for (int i = 0; i < Cells.Count; i++)
            {
                result.Add(new List<GameCell>());
                for (int j = 0; j < Cells[i].Count; j++)
                    result[i].Add(new GameCell());
            }
            Random random = new Random();
            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    int numToTravel = (int)(Cells[i][j].RabbitsCount * Rabbit.RateOfTravel);
                    int alreadyTravelled = 0;
                    List<Tuple<int, int>> possibleDests = possibleDest(Tuple.Create(i, j), Rabbit.TravellingDistance);
                    List<float> rates = getRates(possibleDests.Count);
                    if (alreadyTravelled == numToTravel)
                        continue;
                    for (int dest = 0; dest < possibleDests.Count; dest++)
                    {
                        int travellersTothisDest = (int)(numToTravel * rates[dest]);
                        List<Generation<Rabbit>> destination = new List<Generation<Rabbit>>();
                        if (Cells[i][j].FoxesGenerations.Count == 0)
                            continue;
                        int gen = random.Next(Cells[i][j].RabbitsGenerations.Count);
                        int fromThisGen = random.Next(Math.Min(Cells[i][j].RabbitsGenerations[gen].Count, numToTravel - alreadyTravelled));
                        Generation<Rabbit> resGen = new Generation<Rabbit>(0);
                        for (int rab = 0; rab < fromThisGen; rab++)
                        {
                            int index = random.Next(Cells[i][j].RabbitsGenerations[gen].Count - 1);
                            Rabbit cur = Cells[i][j].RabbitsGenerations[gen][index];
                            resGen.Animals.Add(cur);
                            Cells[i][j].RabbitsGenerations[gen].Animals.RemoveAt(index);
                            alreadyTravelled++;
                        }
                        if (fromThisGen != 0)
                            destination.Add(resGen);
                        result[possibleDests[dest].Item1][possibleDests[dest].Item2].Merge(destination);
                    }
                }
            }
            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < Cells[i].Count; j++)
                {
                    int numToTravel = (int)(Cells[i][j].FoxesCount * Fox.RateOfTravel);
                    int alreadyTravelled = 0;
                    List<Tuple<int, int>> possibleDests = possibleDest(Tuple.Create(i, j), Fox.TravellingDistance);
                    List<float> rates = getRates(possibleDests.Count);
                    if (alreadyTravelled == numToTravel)
                        continue;
                    for (int dest = 0; dest < possibleDests.Count; dest++)
                    {

                        int travellersTothisDest = (int)(numToTravel * rates[dest]);
                        List<Generation<Fox>> destination = new List<Generation<Fox>>();
                        if (Cells[i][j].FoxesGenerations.Count == 0)
                            continue;
                        int gen = random.Next(Cells[i][j].FoxesGenerations.Count);

                        int fromThisGen = random.Next(Math.Min(Cells[i][j].FoxesGenerations[gen].Count, numToTravel - alreadyTravelled));
                        Generation<Fox> resGen = new Generation<Fox>(0);
                        for (int fox = 0; fox < fromThisGen; fox++)
                        {
                            int index = random.Next(Cells[i][j].FoxesGenerations[gen].Count - 1);
                            Fox cur = Cells[i][j].FoxesGenerations[gen][index];
                            resGen.Animals.Add(cur);
                            Cells[i][j].FoxesGenerations[gen].Animals.RemoveAt(index);
                            alreadyTravelled++;
                        }
                        if (fromThisGen != 0)
                            destination.Add(resGen);
                        result[possibleDests[dest].Item1][possibleDests[dest].Item2].Merge(destination);
                    }
                }
            }
            for (int i = 0; i < Size.Item1; i++)
            {
                for (int j = 0; j < Size.Item2; j++)
                {
                    result[i][j].CopyMerge(Cells[i][j]);

                }
            }
            Cells = result;

        }

        #endregion
        public GameGrid(List<List<GameCell>> contents)
        {
            Size = new Tuple<int, int>(contents.Count, contents[0].Count);
            foreach (List<GameCell> l in contents)
                foreach (GameCell gCell in l)
                {
                    TotalFoxesCount += gCell.FoxesCount;
                    TotalRabbitsCount += gCell.RabbitsCount;
                }
            Cells = contents;
        }
        public GameGrid(Tuple<int, int> size)

        {
            object o = new object();
            lock (o)
            {

            }


            Cells = new List<List<GameCell>>();
            Date = 0;
            Size = size;
            for (int i = 0; i < size.Item1; i++)
            {
                Cells.Add(new List<GameCell>());
            }
        }
    }
}
