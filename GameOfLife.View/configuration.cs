using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
namespace GameOfLife.View
{
    public static class GameConfiguration
    {

        public static List<List<GameCellVM>> cells;
        public static GameGrid Source;
        public static void initAll()
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

        public static void simulate(int Dur,Slider s)
        {
            allReady();
            RefreshCells();
            int stp = Dur / 100;
            int cnt = 100;
            if (stp == 0&&Dur!=0)
            {
                stp = 1;
                cnt = Dur;
            }
            for (int i = 0; i < cnt-1; i++)
            {
                DateTime d=DateTime.Now;
                
                Source.GridSim(stp, 1);
                int timeSpent = (int)((DateTime.Now - d).TotalMilliseconds);
                s.Value++;
                RefreshCells();
            }
            s.Value++;
            Source.GridSim(stp+Dur-stp*cnt, 1);
            RefreshCells();
        }
        public static void allReady()
        {
            List<List<GameCell>> contents = new List<List<GameCell>>();
            for (int i = 0; i < cells.Count; i++)
            {
                contents.Add(new List<GameCell>());
                for (int j = 0; j < cells[i].Count; j++)
                {
                    contents[i].Add(new GameCell(cells[i][j].initRabbits, cells[i][j].initFoxes, cells[i][j].initVegetation));
                }
            }
            Source = new GameGrid(contents);
        }
        public static void RefreshCells()
        {
            for (int i = 0; i < Source.Size.Item1; i++)
                for (int j = 0; j < Source.Size.Item2; j++)
                {
                    Color blue = new Color();
                    blue.R = 0;
                    blue.G = 0;
                    blue.B = 255;
                    blue.A = (byte)(((float)(Source.Cells[i][j].RabbitsCount*64) / Source.TotalRabbitsCount) * 255);
                    Color red = new Color();
                    red.B = 0;
                    red.G = 0;
                    red.R = 255;
                    red.A = (byte)(((float)(Source.Cells[i][j].FoxesCount*64) / Source.TotalFoxesCount) * 255);
                    Color green = new Color();
                    green.R = 0;
                    green.B = 0;
                    green.G = 255;
                    green.A = (byte)(Source.Cells[i][j].VegetationLevel * 255);
                    cells[i][j].Blue = new SolidColorBrush(blue);
                    cells[i][j].Green = new SolidColorBrush(green);
                    cells[i][j].Red = new SolidColorBrush(red);
                    cells[i][j].Tooltip = "Rabbits: \t\t" + Source.Cells[i][j].RabbitsCount + "\nFoxes: \t\t" + Source.Cells[i][j].FoxesCount + "\nVegetation: \t\t" + Source.Cells[i][j].VegetationLevel;
                }
        }

    }
}
