using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Rhythm : UserControl
    {
        ObservableCollection<Grid> GridEventsColl;
        ObservableCollection<(Grid, Grid, Grid, Grid, Grid)> GridEventsCol1l;
        public Rhythm()
        {
            InitializeComponent();
            InitColls();
        }

        void InitColls()
        {
            GridEventsColl = new ObservableCollection<Grid>();
            GridEventsColl.Add(Menu_0);
            GridEventsColl.Add(Menu_1);
        }

        private void HiHat_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            Grid sp = null;
            if (mnu != null)
            {
                sp = ((ContextMenu)mnu.Parent).PlacementTarget as Grid;
            }
            int i = Convert.ToInt32(sp.Name[sp.Name.Length - 1] - 48);
            
            if (GridEventsColl[i].Visibility == Visibility.Visible)
            {
                GridEventsColl[i].Visibility = Visibility.Hidden;
            }
            else
            {
                GridEventsColl[i].Visibility = Visibility.Visible;
            }
        }

        private void Snare_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Tom_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RotoTom_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FloorTom_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
