using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace GameOfLife.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        GameCellControl[][] gcCtrls = new GameCellControl[8][];
        private void button_Click(object sender, RoutedEventArgs e)
        {
            int dur = 1;
          //  try
            {
                if (DateCombo.SelectedIndex < 0)
                    throw new Exception("Please Select a Date");
                int d = int.Parse(EndSim_textBox.Text);
                switch (DateCombo.SelectedIndex)
                {
                    case 0:
                        dur = d;
                        break;
                    case 1:
                        dur = d * 7;
                        break;
                    case 2:
                        dur = d * 30;
                        break;
                    case 3:
                        dur = d * 365;
                        break;
                    default:
                        break;
                }
                GameConfiguration.simulate(dur,this.CurDateSlider);
            }
         //   catch (Exception ex)
            {
         //       MessageBox.Show(ex.Message);
              
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameConfiguration.initAll();
            CellInput ci = new CellInput();
            ci.ShowDialog();
            GameConfiguration.cells = new List<List<GameCellVM>>();
            for (int i = 0; i < 8; i++)
            {
                gcCtrls[i] = new GameCellControl[8];
                GameConfiguration.cells.Add(new List<GameCellVM>());
                for (int j = 0; j < 8; j++)
                {
                    gcCtrls[i][j] = new GameCellControl();
                    GameConfiguration.cells[i].Add(new GameCellVM());
                    GameConfiguration.cells[i][j].initFoxes = ci.Fxes;
                    GameConfiguration.cells[i][j].initRabbits = ci.Rabs;
                    GameConfiguration.cells[i][j].initVegetation = ci.Veg;

                    gcCtrls[i][j].context = GameConfiguration.cells[i][j];
                    CellsLayoutContainer.Children.Add(gcCtrls[i][j]);
                }
            }
        }
    }
}
