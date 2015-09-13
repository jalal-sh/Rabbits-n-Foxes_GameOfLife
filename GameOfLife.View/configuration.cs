using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
namespace GameOfLife.View
{
    public class GameConfiguration
    {
        
        static List<List<GameCellVM>> cells;
       /* static GameGrid Source;
        public static void RefreshCells()
        {
            for (int i = 0; i < Source.Size.Item1; i++)
                for (int j = 0; j < Source.Size.Item2; j++)
                {
                    Color blue = new Color();
                    blue.R = 0;
                    blue.G = 0;
                    blue.A = 255;
                    blue.B = (byte)(((float)(Source.Cells[i][j].RabbitsCount) / Source.TotalRabbitsCount) * 255);
                    Color red = new Color();
                    red.B = 0;
                    red.G = 0;
                    red.A = 255;
                    red.R = (byte)(((float)(Source.Cells[i][j].RabbitsCount) / Source.TotalRabbitsCount) * 255);
                    Color green = new Color();
                    green.R = 0;
                    green.B = 0;
                    green.A = 255;
                    green.G = (byte)(((float)(Source.Cells[i][j].RabbitsCount) / Source.TotalRabbitsCount) * 255);
                    cells[i][j].Blue = new SolidColorBrush(blue);
                    cells[i][j].Green = new SolidColorBrush(green);
                    cells[i][j].Red = new SolidColorBrush(red);

                }
        }
        */
    }
}
