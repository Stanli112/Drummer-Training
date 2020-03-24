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
    
    public partial class Rhythm : UserControl
    {
        public Rhythm()
        {
            InitializeComponent();
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_FirstNote == null)
            {
                return;
            }

            _FirstNote.Children.Remove(_FirstNote.Children[0]);
            ComboBox cb = sender as ComboBox;

            switch (cb.SelectedIndex)
            {
                case 0:
                    _FirstNote.Children.Add(new Fourth());
                    break;
                case 1:
                    _FirstNote.Children.Add(new Eighth());
                    break;
                case 2:
                    _FirstNote.Children.Add(new Sixtennth());
                    break;
                case 3:
                    _FirstNote.Children.Add(new ThirtySecond());
                    break;
                default:
                    break;
            }
        }
    }
}
