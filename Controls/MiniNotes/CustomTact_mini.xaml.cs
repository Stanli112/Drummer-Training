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
using static CoordinationTraining.Controls.CustomTact;

namespace CoordinationTraining.Controls.MiniNotes
{
    public partial class CustomTact_mini : UserControl
    {
        public TactType _type;

        // не нравится мне это
        Fourth_mini _fourth;
        Eight_mini _eigth;
        Threeol_mini _threeol;
        Sixteenth_mini _sixteenth;
        ThirtySecond_mini _thirtysecond;

        public CustomTact_mini()
        {
            InitializeComponent();
        }

        public CustomTact_mini(TactType type)
        {
            InitializeComponent();

            _type = type;
            switch (type)
            {
                case TactType.TT_FOURTH:
                    {
                        _fourth = new Fourth_mini();
                        GridMain.Children.Add(_fourth);
                        GridMain.ToolTip = "Четверть";
                        break;
                    }
                case TactType.TT_EIGHTH:
                    {
                        _eigth = new Eight_mini();
                        GridMain.Children.Add(_eigth);
                        GridMain.ToolTip = "Восьмая";
                        break;
                    }
                case TactType.TT_THREEOL:
                    {
                        _threeol = new Threeol_mini();
                        GridMain.Children.Add(_threeol);
                        GridMain.ToolTip = "Триоль";
                        break;
                    }
                case TactType.TT_SIXTEEN:
                    {
                        _sixteenth = new Sixteenth_mini();
                        GridMain.Children.Add(_sixteenth);
                        GridMain.ToolTip = "Шестнадцатая";
                        break;
                    }
                case TactType.TT_THIRTYSECOND:
                    {
                        _thirtysecond = new ThirtySecond_mini();
                        GridMain.Children.Add(_thirtysecond);
                        GridMain.ToolTip = "Тридцать-вторая";
                        break;
                    }
                default:
                    {
                        _type = TactType.TT_FOURTH;
                        _fourth = new Fourth_mini();
                        GridMain.Children.Add(_fourth);
                        GridMain.ToolTip = "Четверть";
                        break;
                    }
            }

        }
    }
}
