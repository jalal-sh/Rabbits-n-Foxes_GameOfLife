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
using System.Windows.Shapes;

namespace GameOfLife.View
{
    /// <summary>
    /// Interaction logic for CellInput.xaml
    /// </summary>
    public partial class CellInput : Window
    {
        public CellInput()
        {
            InitializeComponent();
        }
        public int Rabs, Fxes;
        public float Veg;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rabs = int.Parse(rabbit_textBox.Text);
                Fxes = int.Parse(fox_textBox.Text);
                Veg = float.Parse(veg_textBox.Text);
                if (Veg < 0.1 || Veg > 1.0)
                    throw new FormatException("Vegetation Level must be between 0.1 and 1.0");
                cancel = false;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        bool cancel = true;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = cancel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
