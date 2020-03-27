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
    public partial class CustomTact : UserControl
    {
        enum TactType
        {
            TT_FOURTH,
            TT_EIGHTH,
            TT_SIXTEEN,
            TT_THIRTYSECOND
        }

        Fourth          _fourth;
        Eighth          _eighth;
        Sixtennth       _sixteenth;
        ThirtySecond    _thirtysecond;

        public CustomTact()
        {
            InitializeComponent();

            _fourth = new Fourth();
            _eighth = new Eighth();
            _sixteenth = new Sixtennth();
            _thirtysecond = new ThirtySecond();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridNote == null)
            {
                return;
            }

            GridNote.Children.Remove(GridNote.Children[0]);

            switch (NoteChoise.SelectedIndex)
            {
                case 0:
                    GridNote.Children.Add(_fourth);
                    GridMain.Width = 90;
                    break;
                case 1:
                    GridNote.Children.Add(_eighth);
                    GridMain.Width = 90 * 2;
                    break;
                case 2:
                    GridNote.Children.Add(_sixteenth);
                    GridMain.Width = 90 * 4;
                    break;
                case 3:
                    GridNote.Children.Add(_thirtysecond);
                    GridMain.Width = 90 * 4;
                    break;
                default:
                    break;
            }
        }
    }
}
