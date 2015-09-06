﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;


namespace GameOfLife
{
    public class Grid

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
        public List<List<Cell>> Cells { get; private set; }
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
            if (trDist == 1)
                return result.Distinct().ToList();
            else
            {
                List<Tuple<int, int>> newRes = new List<Tuple<int, int>>();
                foreach (Tuple<int, int> d in result)
                {
                    newRes.AddRange(possibleDest(d, trDist - 1));
                }
                return newRes.Distinct().ToList();
            }
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
            if (threads == 1)
                for (int i = 0; i < time; i++)
                {
                    GridOneDaySim();
                }
            else
            {
                Barrier barrier = new Barrier((int)threads);
                ThreadPool.SetMaxThreads((int)threads, 100);
                for (int i = 0; i < time; i++)
                {
                    GridOneDaySim_Parallel(threads, barrier);
                }
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
                    int oldRabbitCount = Cells[i][j].RabbitsCount;
                    int oldFoxesCount = Cells[i][j].FoxesCount;
                    Cells[i][j].oneDayCourse(Date);
                    lock (this as object)
                    {
                        TotalFoxesCount += Cells[i][j].FoxesCount - oldFoxesCount;
                        TotalRabbitsCount += Cells[i][j].RabbitsCount - oldRabbitCount;
                    }
                }

            }
            ThreadParam.bar.SignalAndWait();
        }

        /// <summary>
        /// Perform a parallel one day sim of the Grid using <paramref name="threadCount"/> Threads
        /// </summary>
        /// <param name="threadCount">Number of threads to use in the simulation process</param>
        private void GridOneDaySim_Parallel(uint threadCount, Barrier br)
        {
            ThreadParam.bar = br;
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
                    int alreadyTravelled = 0;
                    List<Tuple<int, int>> possibleDests = possibleDest(Tuple.Create(i, j), Rabbit.TravellingDistance);
                    List<float> rates = getRates(possibleDests.Count);
                    if (alreadyTravelled == numToTravel)
                        continue;
                    for (int dest = 0; dest < possibleDests.Count; dest++)
                    {
                        int travellersTothisDest = (int)(numToTravel * rates[dest]);
                        List<Generation<Rabbit>> destination = new List<Generation<Rabbit>>();

                        int gen = random.Next(Cells[i][j].RabbitsGenerations.Count - 1);
                        int fromThisGen = random.Next(Math.Min(Cells[i][j].RabbitsGenerations[gen].Count, numToTravel - alreadyTravelled));
                        Generation<Rabbit> resGen = new Generation<Rabbit>(0);
                        for (int rab = 0; rab < fromThisGen; rab++)
                        {
                            int index = random.Next(Cells[i][j].RabbitsGenerations[gen].Count - 1);
                            Rabbit cur = Cells[i][j].RabbitsGenerations[gen][index];
                            resGen.Animals.Add(cur);
                            Cells[i][j].RabbitsGenerations[rab].Animals.RemoveAt(index);
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
                        int gen = random.Next(Cells[i][j].FoxesGenerations.Count - 1);

                        int fromThisGen = random.Next(Math.Min(Cells[i][j].FoxesGenerations[gen].Count, numToTravel - alreadyTravelled));
                        Generation<Fox> resGen = new Generation<Fox>(0);
                        for (int fox = 0; fox < fromThisGen; fox++)
                        {
                            int index = random.Next(Cells[i][j].FoxesGenerations[gen].Count - 1);
                            Fox cur = Cells[i][j].FoxesGenerations[gen][index];
                            resGen.Animals.Add(cur);
                            Cells[i][j].FoxesGenerations[fox].Animals.RemoveAt(index);
                            alreadyTravelled++;
                        }
                        if(fromThisGen!=0)
                        destination.Add(resGen);
                        result[possibleDests[dest].Item1][possibleDests[dest].Item2].Merge(destination);
                    }
                    result[i][j].CopyMerge(Cells[i][j]);
                }
            }

        }

        #endregion
        public Grid(Tuple<int, int> size)

        {
            object o = new object();
            lock (o)
            {

            }


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
