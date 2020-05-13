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
        public enum TactType
        {
            TT_FOURTH = 1,
            TT_EIGHTH,
            TT_THREEOL,
            TT_SIXTEEN,
            TT_THIRTYSECOND
        }

        public TactType     Type;
        public Fourth       _fourth;
        public Eighth       _eighth;
        public Sixtennth    _sixteenth;
        public ThirtySecond _thirtysecond;

        public CustomTact()
        {
            InitializeComponent();

            Type = TactType.TT_FOURTH;
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
                    Type = TactType.TT_FOURTH;
                    break;
                case 1:
                    GridNote.Children.Add(_eighth);
                    GridMain.Width = 90 * 2;
                    Type = TactType.TT_EIGHTH;
                    break;
                case 2:
                    GridNote.Children.Add(_sixteenth);
                    GridMain.Width = 90 * 4;
                    Type = TactType.TT_SIXTEEN;
                    break;
                case 3:
                    GridNote.Children.Add(_thirtysecond);
                    GridMain.Width = 90 * 4;
                    Type = TactType.TT_SIXTEEN;
                    break;
                default:
                    break;
            }
        }
    }
}
