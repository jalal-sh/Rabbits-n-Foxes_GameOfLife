using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{

    public class Cell
    {
        /// <summary>
        /// Generations Of rabbits that exist in the cell
        /// </summary>
        public List<Generation<Rabbit>> RabbitsGenerations { get; set; }
        /// <summary>
        /// Number of Rabbits in the Cell
        /// </summary>
        public int RabbitsCount { get; private set; }
        /// <summary>
        /// Generations of Foxes that exist in the cell
        /// </summary>
        public List<Generation<Fox>> FoxesGenerations { get; set; }
        /// <summary>
        /// Number of Foxes in the Cell
        /// </summary>
        public int FoxesCount { get; private set; }
        /// <summary>
        /// Number of Rabbits per Wolf in the Cell
        /// </summary>
        public int RabbitsDensity => (FoxesCount == 0) ? int.MaxValue : (RabbitsCount / FoxesCount);
        /// <summary>
        /// Vegetation Level in the Cell
        /// </summary>
        public float VegetationLevel { get; private set; }
        /// <summary>
        /// The Amount of Increase of Vegetation Per Day
        /// </summary>
        public static float VegetationGrowth { get; private set; }
        /// <summary>
        /// Simulates the Multiplication Activity of Rabbits
        /// </summary>
        /// <returns>
        /// Number of Rabbits that were Born
        /// </returns>
        private int rabbitMultipliaction(int Date)
        {
            int num = Rabbit.BirthRate[RabbitsCount][VegetationLevel] * (RabbitsCount / 2);
            Generation<Rabbit> newBorn = new Generation<Rabbit>(num);
            RabbitsGenerations.Add(newBorn);
            RabbitsGenerations.Sort();
            return num;
        }
        /// <summary>
        /// Simulates the Multiplication Activity of Foxes
        /// </summary>
        /// <returns>
        /// Number of Foxes that were Born
        /// </returns>
        private int foxMultiplication(int Date)
        {
            int num = Fox.BirthRate[FoxesCount][RabbitsDensity] * (FoxesCount / 2);
            Generation<Fox> newBorn = new Generation<Fox>(num);
            FoxesGenerations.Add(newBorn);
            FoxesGenerations.Sort();
            return num;
        }
        /// <summary>
        /// Merge The Current Lis tof Foxes Generation with another One
        /// </summary>
        /// <param name="o"> The List of Foxes To merge With</param>
        public void Merge(List<Generation<Fox>> o)
        {
            FoxesGenerations.Sort();
            foreach (Generation<Fox> g in o)
            {
                int t = FoxesGenerations.BinarySearch(g);
                if (t < 0)
                {
                    t = ~t;
                    FoxesGenerations.Insert(t, g);
                }
                else
                {
                    FoxesGenerations[t].Animals.AddRange(g.Animals);
                }
                FoxesCount += g.Count;
            }

        }
        /// <summary>
        /// Merge The Current Lis tof Rabbits Generation with another One
        /// </summary>
        /// <param name="o"> The List of Rabbits To merge With</param>
        public void Merge(List<Generation<Rabbit>> o)
        {
            RabbitsGenerations.Sort();
            foreach (Generation<Rabbit> g in o)
            {
                int t = RabbitsGenerations.BinarySearch(g);
                if (t < 0)
                {
                    t = ~t;
                    RabbitsGenerations.Insert(t, g);
                }
                else
                {
                    RabbitsGenerations[t].Animals.AddRange(g.Animals);
                }
                RabbitsCount += g.Count;
            }

        }
        /// <summary>
        /// Copy a Cell but Merges the current Contents with the new Ones
        /// </summary>
        /// <param name="c"></param>
        public void CopyMerge(Cell c)
        {
            VegetationLevel = c.VegetationLevel;
            RabbitsCount = c.RabbitsCount;
            FoxesCount = c.FoxesCount;
            Merge(c.FoxesGenerations);
            Merge(c.RabbitsGenerations);
        }
        /// <summary>
        /// Simulates the Prey Activity of Foxes
        /// </summary>
        private void preyTheFoxes(int Date)
        {
            float prop = Fox.MinFoodProp;
            int AllowedWeekly = Fox.MinFoodAllowedWeekly;
            if (VegetationLevel > Fox.PlantationCoverRabbits)
            {
                prop = Fox.MaxFoodProp;
                AllowedWeekly = Fox.MaxFoodAllowedWeekly;
            }
            foreach (Generation<Fox> gen in FoxesGenerations)
            {
                foreach (Fox f in gen.Animals)
                {
                    int i = 0;
                    int e = 0;
                    Random r = new Random();
                    while (f.EatenThisWeek + e < AllowedWeekly && i < RabbitsCount)
                    {
                        double t = r.NextDouble();
                        if (t <= prop)
                        {
                            e++;
                            int rab = r.Next(RabbitsGenerations.Count);
                            RabbitsGenerations[rab].Animals.RemoveAt(r.Next(RabbitsGenerations[rab].Count));
                            if (RabbitsGenerations[rab].Count == 0)
                                RabbitsGenerations.RemoveAt(rab);
                            RabbitsCount--;
                        }
                        i++;
                    }
                    f.Eat(e, Date);
                }
            }
        }
        /// <summary>
        /// Simulates The Death Event of Rabbits
        /// </summary>
        private void rabbitsDie(int Date)
        {

            List<Generation<Rabbit>> StillAlive = new List<Generation<Rabbit>>();
            foreach (Generation<Rabbit> rabs in RabbitsGenerations)
            {
                if (rabs.Age > Rabbit.MedianAge[VegetationLevel])
                {
                    Random r = new Random();
                    int numToDie = r.Next(rabs.Count);
                    RabbitsCount -= numToDie;
                    rabs.Animals.RemoveRange(r.Next(rabs.Count - numToDie), numToDie);
                    if (rabs.Count != 0)
                    {
                        foreach (Rabbit ra in rabs.Animals)
                        {
                            ra.AgeOneDay();
                        }
                        StillAlive.Add(rabs);
                    }
                }
                else
                {
                    foreach (Rabbit ra in rabs.Animals)
                    {
                        ra.AgeOneDay();
                    }
                    StillAlive.Add(rabs);
                }
            }
            RabbitsGenerations = StillAlive;
        }
        /// <summary>
        /// Simulates The Death Event of Foxes
        /// </summary>
        private void foxesDie(int Date)
        {
            List<Generation<Fox>> StillAlive = new List<Generation<Fox>>();
            foreach (Generation<Fox> fx in FoxesGenerations)
            {
                Random r = new Random();
                float deathprop = 0f;
                if (fx.Age > Fox.MedianAge)
                {
                    deathprop = (float)r.NextDouble();
                }
                List<Fox> alliveGen = new List<Fox>();
                for (int i = 0; i < fx.Count; i++)
                {
                    float t = (float)r.NextDouble();
                    if (fx[i].Hungry)
                        t += Fox.HungerDeathPropabilty;
                    if (t < deathprop)
                    {
                        Fox Allive = fx[i];
                        Allive.AgeOneDay();
                        alliveGen.Add(Allive);
                    }
                    else
                        FoxesCount--;
                }
                fx.Animals = alliveGen;
                if (fx.Count != 0)
                    StillAlive.Add(fx);
            }
            FoxesGenerations = StillAlive;
        }
        /// <summary>
        /// Changes The Vegetation Level based on thr Rules provided
        /// </summary>
        private void changeVegetationLevels()
        {
            float f = (1f + VegetationGrowth) * VegetationLevel - 0.001f * RabbitsCount;
            if (f > 1.0f)
                f = 1.0f;
            if (f < 0.1f)
                f = 0.1f;
            VegetationLevel = f;
        }
        /// <summary>
        /// Does a One Day Simulation of the Life in the Current Cell
        /// </summary>
        public void oneDayCourse(int Date)
        {
            int newFoxes = 0, newRabbits = 0;
            if (Date != 0)
            {
                if (Date % Fox.MultiplicationInterval == 0)
                    newFoxes = foxMultiplication(Date);
                if (Date % Rabbit.MultiplicationInterval == 0)
                    newRabbits = rabbitMultipliaction(Date);
            }
            RabbitsCount += newRabbits;
            FoxesCount += newFoxes;
            preyTheFoxes(Date);
            rabbitsDie(Date);
            foxesDie(Date);
            changeVegetationLevels();
        }
        /// <summary>
        /// Initialize the Static Variables in Cell
        /// </summary>
        /// <param name="VegetationGrowth">The Amount of Increase of Vegetation Per Day</param>
        public static void init(float VegetationGrowth)
        {
            Cell.VegetationGrowth = VegetationGrowth;
        }
        public Cell()
        {
            FoxesGenerations = new List<Generation<Fox>>();
            RabbitsGenerations = new List<Generation<Rabbit>>();
            RabbitsCount = FoxesCount = 0;
            VegetationLevel = 0.1f;
        }
        public Cell(int RabbitsCount, int FoxesCount, float Veg)
        {
            FoxesGenerations = new List<Generation<Fox>>();
            FoxesGenerations.Add(new Generation<Fox>(FoxesCount));
            RabbitsGenerations = new List<Generation<Rabbit>>();
            RabbitsGenerations.Add(new Generation<Rabbit>(RabbitsCount));
            this.RabbitsCount = RabbitsCount;
            this.FoxesCount = FoxesCount;
            VegetationLevel = Veg;
        }
        public Cell(Cell c)
        {
            RabbitsGenerations = new List<Generation<Rabbit>>();
            foreach (Generation<Rabbit> g in c.RabbitsGenerations)
                RabbitsGenerations.Add(new Generation<Rabbit>(g));
            FoxesGenerations = new List<Generation<Fox>>();
            foreach (Generation<Fox> g in c.FoxesGenerations)
                FoxesGenerations.Add(new Generation<Fox>(g));
            VegetationLevel = c.VegetationLevel;
            RabbitsCount = c.RabbitsCount;
            FoxesCount = c.FoxesCount;
        }
    }
}
