using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowTransformation;
using GameOfLife;
namespace ThreadingVisualizer
{
    public partial class Form1 : Form
    {
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
        public void Gridit(int threads)
        {
            GameGrid g;
            initAll();
            g = new GameGrid(Tuple.Create(16, 16));
            foreach (List<GameCell> cells in g.Cells)
                for (int i = 0; i < 16; i++)
                {
                    cells.Add(new GameCell(20, 500, 1f));
                }
            g.GridSim(10 * 365, (uint)threads);
        }
        int[] times;
        Gu gu;
        public Form1()
        {
            gu = new Gu();
            times = new int[10];
            bg.DoWork += Bg_DoWork;
            bg.RunWorkerCompleted += Bg_RunWorkerCompleted;
            InitializeComponent();
            g = panel1.CreateGraphics();
        }

        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            panel1.Refresh();
        }

        Graphics g;
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        int i = 10;
        int d = 11;
        private void DrawThreadPerformance()
        {
            while (i > 0)
            {
                DateTime x = DateTime.Now;
                Gridit(i);
                double t = (DateTime.Now - x).TotalMilliseconds;
                times[i - 1] = (int)t;
                d = i;
                i--;
            }

        }
        BackgroundWorker bg = new BackgroundWorker();
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (bg.IsBusy)
                return;
            Graphics g = panel1.CreateGraphics();
            gu.GuWindowView(0, 0, 11, 18000, 0, 0, 1360, 786);
            for (int i = 9; i >= d - 1; i--)
            {
                gu.GuText("*", i + 1 - 0.05, times[i], g);
                gu.GuText(times[i] + "ms", i + 1, times[i] + 60, g);
                if (i != 9)
                    gu.GuLine(i + 1, times[i], i * 1f + 2, times[i + 1], g);

            }
        }



        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            DrawThreadPerformance();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bg.RunWorkerAsync();

        }


    }
}
