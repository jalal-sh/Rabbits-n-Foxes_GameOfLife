using System.Windows.Controls;
using System.Windows.Input;

namespace GameOfLife.View
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class GameCellControl : UserControl
    {
        public GameCellVM context
        {
            get { return GameCellLayout.GetValue(DataContextProperty) as GameCellVM; }


            set { GameCellLayout.SetValue(DataContextProperty, value); }
        }
        public GameCellControl()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CellInput ci = new CellInput();
            ci.ShowDialog();
            context.initFoxes = ci.Fxes;
            context.initRabbits = ci.Rabs;
            context.initVegetation = ci.Veg;
        }
    }
}
