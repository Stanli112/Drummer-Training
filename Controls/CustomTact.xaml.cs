using CoordinationTraining.Controls.MiniNotes;
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
        private Fourth          _fourth;
        private Eighth          _eighth;
        private Threeol         _threeol;
        private Sixtennth       _sixteenth;
        private ThirtySecond    _thirtysecond;

        public CustomTact()
        {
            InitializeComponent();

            Type = TactType.TT_FOURTH;
            _fourth         = new Fourth();
            _eighth         = new Eighth();
            _threeol        = new Threeol();
            _sixteenth      = new Sixtennth();
            _thirtysecond   = new ThirtySecond();

        }

        private void ccNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NoteChanged(ccNotes._type);
        }

        private void NoteChanged(TactType type)
        {
            if (GridNote == null)
            {
                return;
            }

            GridNote.Children.Remove(GridNote.Children[0]);

            switch (type)
            {
                case TactType.TT_FOURTH:
                    GridNote.Children.Add(_fourth);
                    Type = TactType.TT_FOURTH;
                    break;
                case TactType.TT_EIGHTH:
                    GridNote.Children.Add(_eighth);
                    Type = TactType.TT_EIGHTH;
                    break;
                case TactType.TT_THREEOL:
                    GridNote.Children.Add(_threeol);
                    Type = TactType.TT_THREEOL;
                    break;
                case TactType.TT_SIXTEEN:
                    GridNote.Children.Add(_sixteenth);
                    Type = TactType.TT_SIXTEEN;
                    break;
                case TactType.TT_THIRTYSECOND:
                    GridNote.Children.Add(_thirtysecond);
                    Type = TactType.TT_SIXTEEN;
                    break;
                default:
                    break;
            }
        }

    }
}
