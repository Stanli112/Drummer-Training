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
        public bool bHiHat = true,
                    bSnare = false,
                    bMidTom = false,
                    bHightTom = false,
                    bFloorTom = false,
                    bBassDrum = false;

        public Fourth()
        {
            InitializeComponent();
            SetHelpText();
        }
        
        void SetHelpText()
        {
            string text = "";
            if (bHiHat)
                text += "HiHat\n";
            if (bSnare)
                text += "Snare\n";
            if (bMidTom)
                text += "MidTom\n";
            if (bHightTom)
                text += "HightTom\n";
            if (bFloorTom)
                text += "FloorTom\n";
            if (bBassDrum)
                text += "BassDrum";

            Help.Text = text;
        }

        private void HiHat_Click(object sender, RoutedEventArgs e)
        {
            bHiHat = !bHiHat;
            if (HiHat.Visibility == Visibility.Visible)
            {
                HiHat.Visibility = Visibility.Hidden;
            }
            else
            {
                HiHat.Visibility = Visibility.Visible;
            }
            SetHelpText();
        }

        private void HightTom_Click(object sender, RoutedEventArgs e)
        {
            bHightTom = !bHightTom;
            if (HightTom.Visibility == Visibility.Visible)
            {
                HightTom.Visibility = Visibility.Hidden;
            }
            else
            {
                HightTom.Visibility = Visibility.Visible;
            }
            SetHelpText();
        }

        private void MidTom_Click(object sender, RoutedEventArgs e)
        {
            bMidTom = !bMidTom;
            if (MidTom.Visibility == Visibility.Visible)
            {
                MidTom.Visibility = Visibility.Hidden;
            }
            else
            {
                MidTom.Visibility = Visibility.Visible;
            }
            SetHelpText();
        }

        private void Snare_Click(object sender, RoutedEventArgs e)
        {
            bSnare = !bSnare;
            if (Snare.Visibility == Visibility.Visible)
            {
                Snare.Visibility = Visibility.Hidden;
            }
            else
            {
                Snare.Visibility = Visibility.Visible;
            }
            SetHelpText();
        }

        private void FloorTom_Click(object sender, RoutedEventArgs e)
        {
            bFloorTom = !bFloorTom;
            if (FloorTom.Visibility == Visibility.Visible)
            {
                FloorTom.Visibility = Visibility.Hidden;
            }
            else
            {
                FloorTom.Visibility = Visibility.Visible;
            }
            SetHelpText();
        }

        private void BassDrum_Click(object sender, RoutedEventArgs e)
        {
            bBassDrum = !bBassDrum;
            if (BassDrum.Visibility == Visibility.Visible)
            {
                BassDrum.Visibility = Visibility.Hidden;
            }
            else
            {
                BassDrum.Visibility = Visibility.Visible;
            }
            SetHelpText();
        }
    }
}
