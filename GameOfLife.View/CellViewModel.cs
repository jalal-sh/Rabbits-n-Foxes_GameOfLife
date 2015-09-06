﻿using System;
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
    internal class CellViewModel : INotifyPropertyChanged
    {
        


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

        
    }
}
