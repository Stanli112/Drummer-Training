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

namespace CoordinationTraining.Controls
{
    public partial class Fourth : UserControl
    {
        public Fourth()
        {
            InitializeComponent();
        }

        private void HiHat_Click(object sender, RoutedEventArgs e)
        {
            if (HiHat.Visibility == Visibility.Visible)
            {
                HiHat.Visibility = Visibility.Hidden;
            }
            else
            {
                HiHat.Visibility = Visibility.Visible;
            }
        }

        private void HightTom_Click(object sender, RoutedEventArgs e)
        {
            if (HightTom.Visibility == Visibility.Visible)
            {
                HightTom.Visibility = Visibility.Hidden;
            }
            else
            {
                HightTom.Visibility = Visibility.Visible;
            }
        }

        private void MidTom_Click(object sender, RoutedEventArgs e)
        {
            if (MidTom.Visibility == Visibility.Visible)
            {
                MidTom.Visibility = Visibility.Hidden;
            }
            else
            {
                MidTom.Visibility = Visibility.Visible;
            }
        }

        private void Snare_Click(object sender, RoutedEventArgs e)
        {
            if (Snare.Visibility == Visibility.Visible)
            {
                Snare.Visibility = Visibility.Hidden;
            }
            else
            {
                Snare.Visibility = Visibility.Visible;
            }
        }

        private void FloorTom_Click(object sender, RoutedEventArgs e)
        {
            if (FloorTom.Visibility == Visibility.Visible)
            {
                FloorTom.Visibility = Visibility.Hidden;
            }
            else
            {
                FloorTom.Visibility = Visibility.Visible;
            }
        }

        private void BassDrum_Click(object sender, RoutedEventArgs e)
        {
            if (BassDrum.Visibility == Visibility.Visible)
            {
                BassDrum.Visibility = Visibility.Hidden;
            }
            else
            {
                BassDrum.Visibility = Visibility.Visible;
            }
        }
    }
}
