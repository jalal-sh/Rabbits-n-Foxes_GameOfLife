using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;
namespace GameOfLife.View
{
    /// <summary>
    /// Represents the Presentation Logic of each Cell
    /// Contains 3 Properties each representing each type of element in the Cell                 
    /// </summary>
    public class GameCellVM : INotifyPropertyChanged
    {
        public GameCellVM()
        {
            Red = new SolidColorBrush(Colors.Red);
            Blue = new SolidColorBrush(Colors.Blue);
            Green = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
        }
        private SolidColorBrush _red;
        /// <summary>
        /// Representing the Foxes in The Cell
        /// </summary>
        public SolidColorBrush Red
        {
            get { return _red; }
            set { _red = value; NotifyChange(nameof(Red)); }
        }
        private SolidColorBrush _blue;
        /// <summary>
        /// Representing the Rabbits in the Cell
        /// </summary>
        public SolidColorBrush Blue
        {
            get { return _blue; }
            set { _blue = value; NotifyChange(nameof(Blue)); }
        }
        private SolidColorBrush _green;
        /// <summary>
        /// Representing the Vegetation level in the Cell
        /// </summary>
        public SolidColorBrush Green
        {
            get { return _green; }
            set { _green = value; NotifyChange(nameof(Green)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChange(string name)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(name));

        }
        private string _tooltip;

        public string Tooltip
        {
            get { return _tooltip; }
            set { _tooltip = value; NotifyChange(nameof(Tooltip)); }
        }
        public int initRabbits { get;set; }
        public int initFoxes { get; set; }
        public float initVegetation { get; set; }
        void input(int r,int f,float v)
        {
            initFoxes = f;
            initRabbits = r;
            initVegetation = v;
        }
    }
}
