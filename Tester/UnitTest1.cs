using System;
using GameOfLife;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
namespace GameOfLife.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void DistibutionTest()
        {
            Distribution<float, int> one_dim = new Distribution<float, int>();
            Distribution<int, float, int> tow_Dim = new Distribution<int, float, int>();
            Random r = new Random();
            one_dim[1f] = 4;
            tow_Dim[1] = new Distribution<float, int>();
            tow_Dim[1][2f] = 3;
            AreEqual(one_dim[0f], 4);
            AreEqual(tow_Dim[1][1f], 3);
        }
        void initAll()
        {
            Distribution<float, int> rabbitAge = new Distribution<float, int>();
            rabbitAge[0.15f] = 3 * 30;
            rabbitAge[0.25f] = 6 * 30;
            rabbitAge[0.35f] = 12 * 30;
            rabbitAge[1.00f] = 18 * 30;
            Distribution<int, float, int> rabbitBirths = new Distribution<int, float, int>();
            rabbitBirths[2] = new Distribution<float, int>();
            rabbitBirths[2][0.2f] = 0;
            rabbitBirths[2][0.5f] = 0;
            rabbitBirths[2][0.8f] = 0;
            rabbitBirths[2][1.0f] = 0;

            rabbitBirths[201] = new Distribution<float, int>();
            rabbitBirths[201][0.2f] = 3;
            rabbitBirths[201][0.5f] = 4;
            rabbitBirths[201][0.8f] = 6;
            rabbitBirths[201][1.0f] = 9;

            rabbitBirths[701] = new Distribution<float, int>();
            rabbitBirths[701][0.2f] = 3;
            rabbitBirths[701][0.5f] = 4;
            rabbitBirths[701][0.8f] = 5;
            rabbitBirths[701][1.0f] = 8;

            rabbitBirths[5001] = new Distribution<float, int>();
            rabbitBirths[5001][0.2f] = 2;
            rabbitBirths[5001][0.5f] = 3;
            rabbitBirths[5001][0.8f] = 4;
            rabbitBirths[5001][1.0f] = 7;

            rabbitBirths[int.MaxValue] = new Distribution<float, int>();
            rabbitBirths[int.MaxValue][0.2f] = 2;
            rabbitBirths[int.MaxValue][0.5f] = 3;
            rabbitBirths[int.MaxValue][0.8f] = 4;
            rabbitBirths[int.MaxValue][1.0f] = 5;
            Rabbit.init(14, rabbitAge, rabbitBirths, 0.2f, 1);


            Distribution<int, float, int> foxBirths = new Distribution<int, float, int>();
            foxBirths[2] = new Distribution<float, int>();
            foxBirths[2][0.3f] = 0;
            foxBirths[2][10f] = 0;
            foxBirths[2][40f] = 0;
            foxBirths[2][float.MaxValue] = 0;

            foxBirths[11] = new Distribution<float, int>();
            foxBirths[11][0.3f] = 2;
            foxBirths[11][10f] = 3;
            foxBirths[11][40f] = 4;
            foxBirths[11][float.MaxValue] = 5;

            foxBirths[51] = new Distribution<float, int>();
            foxBirths[51][0.3f] = 2;
            foxBirths[51][10f] = 3;
            foxBirths[51][40f] = 3;
            foxBirths[51][float.MaxValue] = 4;

            foxBirths[101] = new Distribution<float, int>();
            foxBirths[101][0.3f] = 1;
            foxBirths[101][10f] = 2;
            foxBirths[101][40f] = 3;
            foxBirths[101][float.MaxValue] = 3;

            foxBirths[int.MaxValue] = new Distribution<float, int>();
            foxBirths[int.MaxValue][0.3f] = 0;
            foxBirths[int.MaxValue][10f] = 1;
            foxBirths[int.MaxValue][40f] = 2;
            foxBirths[int.MaxValue][float.MaxValue] = 3;

            Fox.init(9 * 7, 4 * 365, foxBirths, 0.6f, ((float)2) / 7, ((float)4) / 7, 2, 4, 2, 0.1f, 1f, 2);
            GameCell.init(0.1f);
        }

        [TestMethod]
        public void GridTestOneDay()
        {
            initAll();
            GameGrid g = new GameGrid(Tuple.Create(8, 8));
            foreach (List<GameCell> cells in g.Cells)
            {
                for (int i = 0; i < 8; i++)
                {
                    cells.Add(new GameCell(0, 0, 1f));
                }
            }
            // g.Cells[0][0] = new Cell(2, 2, 1f);
            g.GridOneDaySim();
        }
        [TestMethod]
        public void OneCell()
        {
            initAll();
            GameCell c = new GameCell(20, 100, 1f);
            c.oneDayCourse(365 * 5);
            //Assert.Inconclusive(c.RabbitsCount + " " + c.FoxesCount);
        }
        [TestMethod]
        public void GridTenYear()
        {
            initAll();
            GameGrid g = new GameGrid(Tuple.Create(8, 8));
            foreach (List<GameCell> cells in g.Cells)
                for (int i = 0; i < 8; i++)
                {
                    cells.Add(new GameCell(100, 20, 1f));
                }
            g.GridSim(10 * 365, 1);
        }
    }
}
