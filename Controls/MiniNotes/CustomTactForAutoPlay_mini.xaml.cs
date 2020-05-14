using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
    public partial class CustomTactForAutoPlay_mini : UserControl
    {
        enum BTN
        {
            BTN_REPEAT_PLUS = 10,
            BTN_REPEAT_MINUS, 
            BTN_BPM_PLUS,
            BTN_BPM_MINUS
        }

        public TactType _type;
        public int _BPM;
        public int _repeat;

        /// <summary> Звук метронома - используется в тренеровке </summary>
        private SoundPlayer FirstBitSound;
        private SoundPlayer SecondBitSound;

        // не нравится мне это
        private Fourth_mini _fourth;
        private Eight_mini _eigth;
        private Threeol_mini _threeol;
        private Sixteenth_mini _sixteenth;
        private ThirtySecond_mini _thirtysecond;

        public CustomTactForAutoPlay_mini()
        {
            _type = TactType.TT_FOURTH;
            InitializeComponent();
            InitTT();

            FirstBitSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\firstBit.wav");
            FirstBitSound.Load();

            SecondBitSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\secondBit2.wav");
            SecondBitSound.Load();
        }

        /// я хер его знает, зачем он нужен
        public CustomTactForAutoPlay_mini(TactType type, SoundPlayer _fs, SoundPlayer _ss)
        {
            InitializeComponent();

            FirstBitSound = _fs;
            SecondBitSound = _ss;

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

        private void btnRepeatCount_Click(object sender, RoutedEventArgs e)
        {
            //bool heh = ((Button)sender).IsPressed;
            //while(btnMinusBPMCount.IsPressed)
            //{
            //    SumBPMCount(1);
            //}
            BTN id = (BTN)Convert.ToInt32(((Button)sender).Uid);
            switch (id)
            {
                case BTN.BTN_REPEAT_PLUS:
                    SumRepeatCount(1);
                    break;
                case BTN.BTN_REPEAT_MINUS:
                    SumRepeatCount(-1);
                    break;
                case BTN.BTN_BPM_PLUS:
                    SumBPMCount(1);
                    break;
                case BTN.BTN_BPM_MINUS:
                    SumBPMCount(-1);
                    break;
                default:
                    break;
            }
        }

        void SumRepeatCount(int _number)
        {
            int count = Convert.ToInt32(tbRepeateCount.Text);
            count += _number;
            if(count < 1)
            {
                count = 1;
            }
            if (count > 9)
            {
                count = 9;
            }
            _repeat = count;
            tbRepeateCount.Text = count.ToString();
        }

        void SumBPMCount(int _number)
        {
            int count = Convert.ToInt32(tbBPM.Text);
            count += _number;
            if (count < 30)
            {
                count = 30;
            }
            if (count > 300)
            {
                count = 300;
            }
            _BPM = count;
            tbBPM.Text = count.ToString();
        }

        private void SelectedNote_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectNoteType snt = new SelectNoteType(this);
            snt.ShowDialog();
            InitTT();
        }
        
        void InitTT()
        {
            ct.SetTact(_type);
        }

        public void Play()
        {            
            for(int i = 0; i < _repeat; i++)   // кол-во повторов
            {
                double _pause = 1000 / ((double)_BPM / 60 * (int)_type);
                Thread.Sleep((int)_pause);

                FirstBitSound.Play();

                for (int j = 0; j < (int)_type - 1; j++)
                {
                    //_pause = 1000 / ((double)_BPM / 60 * (int)_type);
                    Thread.Sleep((int)_pause);                    
                    SecondBitSound.Play();
                }
            }
        }
    }
}
