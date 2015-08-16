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
        /// The X,Y Coordinates of the Cell in the Grid
        /// </summary>
        public Tuple<int, int> Coordinates { get; set; }
        /// <summary>
        /// Generations Of rabbits that exist in the cell
        /// </summary>
        private List<Generation<Rabbit>> Rabbits { get; set; }
        /// <summary>
        /// Number of Rabbits in the Cell
        /// </summary>
        public int RabbitsCount { get; private set; }
        /// <summary>
        /// Generations of Foxes that exist in the cell
        /// </summary>
        private List<Generation<Fox>> Foxes { get; set; }
        /// <summary>
        /// Number of Foxes in the Cell
        /// </summary>
        public int FoxesCount { get; private set; }
        /// <summary>
        /// Number of Rabbits per Wolf in the Cell
        /// </summary>
        public int RabbitsDensity => RabbitsCount / FoxesCount;
        /// <summary>
        /// Vegetation Level in the Cell
        /// </summary>
        public int VegetationLevel { get; private set; }
        /// <summary>
        /// Date represented as the number Days since Simulation Began
        /// </summary>
        public int Date { get; private set; }
        public delegate List<Cell> PossibleDest(Cell sender);
        public PossibleDest PossibleRabbitsDest;
        public PossibleDest PossibleFoxesDest;
        /// <summary>
        /// Simulates the Multiplication Activity of Rabbits
        /// </summary>
        /// <returns>
        /// Number of Rabbits that were Born
        /// </returns>
        private int rabbitMultipliaction()
        {
            int num = Rabbit.BirthRate[RabbitsCount][VegetationLevel] * (RabbitsCount / 2);
            Generation<Rabbit> newBorn = new Generation<Rabbit>(num);
            Rabbits.Add(newBorn);
            return num;
        }
        /// <summary>
        /// Simulates the Multiplication Activity of Foxes
        /// </summary>
        /// <returns>
        /// Number of Foxes that were Born
        /// </returns>
        private int foxMultiplication()
        {
            int num = Fox.BirthRate[FoxesCount][RabbitsDensity] * (FoxesCount / 2);
            Generation<Fox> newBorn = new Generation<Fox>(num);
            Foxes.Add(newBorn);
            return num;
        }
        /// <summary>
        /// Simulates the Prey Activity of Foxes
        /// </summary>
        private void preyTheFoxes()
        {
            float prop = Fox.MinFoodProp;
            int AllowedWeekly = Fox.MinFoodAllowedWeekly;
            if (VegetationLevel > Fox.PlantationCoverRabbits)
            {
                prop = Fox.MaxFoodProp;
                AllowedWeekly = Fox.MaxFoodAllowedWeekly;
            }
            foreach (Generation<Fox> gen in Foxes)
            {
                foreach (Fox f in gen.Animals)
                {
                    int i = 0;
                    int e = 0;
                    Random r = new Random();
                    while (f.EatenThisWeek < AllowedWeekly && i < RabbitsCount)
                    {
                        double t = r.NextDouble();
                        if (t <= prop)
                        {
                            e++;
                            int rab = r.Next(Rabbits.Count);
                            Rabbits[rab].Animals.RemoveAt(r.Next(Rabbits[rab].Animals.Count));
                            if (Rabbits[rab].Animals.Count == 0)
                                Rabbits.RemoveAt(rab);
                            RabbitsCount--;
                        }
                        i++;
                    }
                    f.Eat(e, Date);
                }
            }
        }
        /// <summary>
        /// Does a One Day Simulation of the Life in Cell
        /// </summary>
        public void oneDayCourse()
        {
            int newFoxes = 0, newRabbits = 0;
            if (Date % Fox.MultiplicationInterval == 0)
                newFoxes = foxMultiplication();
            if (Date % Rabbit.MultiplicationInterval == 0)
                newRabbits = rabbitMultipliaction();
            RabbitsCount += newRabbits;
            FoxesCount += newFoxes;
            preyTheFoxes();
            
        }
    }
}
