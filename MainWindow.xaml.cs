using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Media;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using CoordinationTraining.Classes;
using System.Windows.Media.Animation;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoordinationTraining.Controls;
using static CoordinationTraining.Controls.CustomTact;
using CoordinationTraining.Controls.MiniNotes;

namespace CoordinationTraining
{
    public partial class MainWindow : Window
    {
        const string lblMainBeat = "Beat creator";
        const string lblMainCoor = "Metronom";
        
        #region Title
        /// <summary> Свернуть окно </summary>
        private void BtnWindowWrap_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        
        /// <summary> Меняет размеры окна из 'На весь экран' на поиеньше, при захвате мышкой </summary>
        private void TitleBarMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.BeginInit();
                    double adjustment = 40.0;
                    var mouse1 = e.MouseDevice.GetPosition(this);
                    var width1 = Math.Max(this.ActualWidth - 2 * adjustment, adjustment);
                    this.WindowState = WindowState.Normal;
                    var width2 = Math.Max(this.ActualWidth - 2 * adjustment, adjustment);
                    this.Left = (mouse1.X - adjustment) * (1 - width2 / width1);
                    this.Top = -7;
                    this.EndInit();
                    this.DragMove();
                }
            }
        }

        /// <summary> Развернуть окно </summary>
        private void BtnWindowExtend_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }
        
        /// <summary> Таскать окно </summary>
        private void WindowMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                BtnWindowExtend_Click(sender, e);
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
               
        /// <summary> Закрыть окно </summary>
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary> Перед закрытием окна </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            settings.SaveSettings();
        }
        #endregion

        enum _page
        {
            beat,
            coord
        }

        /// <summary> Для запуска/остановки метронома </summary>
        bool MetronomUsed = false;

        /// <summary> Повтор метронома </summary>
        bool RepeateMetronomList = false;

        /// <summary> Количество тактов в одном промежутке (4) </summary>
        private readonly int TactCount = 4;

        /// <summary> Звук метронома - используется в тренеровке </summary>
        private SoundPlayer MetronomeSound;

        /// <summary> Звук метронома - используется в тренеровке </summary>
        private SoundPlayer MetronomeFirstBitSound;
        
        /// <summary> Звук метронома - используется в тренеровке </summary>
        private SoundPlayer MetronomeSecondBitSound;

        /// <summary> Хранит настройки проекта </summary>        
        private Settings settings = new Settings();


        /// <summary> Нужно её переделать по текущую коллекцию и сделать ещё одну глобальную </summary>
        private ObservableCollection<CustomTactForAutoPlay_mini> selectedListMetroColl 
            = new ObservableCollection<CustomTactForAutoPlay_mini>();
        private ObservableCollection<ObservableCollection<CustomTactForAutoPlay_mini>> g_PlayListMetroColl
            = new ObservableCollection<ObservableCollection<CustomTactForAutoPlay_mini>>();

        public MainWindow()
        {
            InitializeComponent();

            btnMainBeatCreator.Content = lblMainBeat;
            btnMainCoordTraining.Content = lblMainCoor;

            settings.LoadSettings(ref settings);


            /// загрузка звуков
            MetronomeSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\cut.wav");
            MetronomeSound.Load();

            MetronomeFirstBitSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\firstBit.wav");
            MetronomeFirstBitSound.Load();

            MetronomeSecondBitSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\secondBit.wav");
            MetronomeSecondBitSound.Load();
            ///


            ShowPage(_page.coord);
                        
            SetRepeateMetronomListState(RepeateMetronomList);
        }







        #region Coordination Training

        #region Metronom

        /// <summary> Запускает и останавливает метроном </summary>
        private void PlayMetronom_Click(object sender, RoutedEventArgs e)
        {
            MetronomUsed = !MetronomUsed;
            Task t = Task.Run(() =>
            {
                while (MetronomUsed)
                {
                    MetronomStart(TactCount);
                    if (!MetronomUsed)
                    {
                        Dispatcher.Invoke(() => { lblTactCount.Content = ""; });
                    }
                }
            });
        }

        #endregion



        #region Левая менюшка с Амплитудой и Повторами


        private void BtnBPS_Click(object sender, RoutedEventArgs e)
        {
            int bps = Convert.ToInt32(txtBPM.Text);
            if (((Button)sender).Name == "BtnBPSMinus")
            {
                if (bps > 40)
                {
                    bps -= 1;
                }
            }
            if (((Button)sender).Name == "BtnBPSPlus")
            {
                if (bps < 200)
                {
                    bps += 1;
                }
            }
            txtBPM.Text = bps.ToString();
            settings.BPM = bps;
        }
        
        #endregion


        #region Правая менюшка

        
        #endregion


        #region Вспомогательные функции

        /// <summary> Проигрывает метроном с выстановленной амплитудой </summary>
        private void MetronomStart(int _bitCount)
        {
            for (int k = 0; k < _bitCount; k++)
            {
                double _pause = 1000 / ((double)settings.BPM / 60 * (int)ccNotes._type);
                Thread.Sleep( (int)_pause );
                MetronomeSound.Play();
                Dispatcher.Invoke(() =>
                {
                    lblTactCount.Content = (k + 1).ToString();
                });
            }
        }

        #endregion

        #endregion

        #region Global func

        private void ShowPage(_page page)
        {
            switch(page)
            {
                case _page.beat:
                    {
                        GridBarBeatCreator.Visibility = Visibility.Visible;
                        GridMainBeatCreator.Visibility = Visibility.Visible;

                        GridMainCoordTreaning.Visibility = Visibility.Hidden;
                        break;
                    }

                case _page.coord:
                    {
                        GridBarBeatCreator.Visibility = Visibility.Hidden;
                        GridMainBeatCreator.Visibility = Visibility.Hidden;

                        GridMainCoordTreaning.Visibility = Visibility.Visible;

                        break;
                    }
            }
        }

        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            switch(btn.Content)
            {
                case lblMainBeat:
                    ShowPage(_page.beat);
                    break;

                case lblMainCoor:
                    ShowPage(_page.coord);
                    break;

                default:
                    break;
            }
        }

        #endregion

        private void btnAddRythm_Click(object sender, RoutedEventArgs e)
        {
            AddRhythmToStackPanel(spRhythm);
        }
        
        void AddRhythmToStackPanel(StackPanel sp)
        {
            if (sp.Children.Count > 0)
            {
                sp.Children.Add(new Grid()
                {
                    Background = Brushes.White,
                    Margin = new Thickness(0, 100, 0, 100),
                    Width = 2
                });
            }
            sp.Children.Add(new Controls.Rhythm());
        }

        private void btnSaveRythm_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Rhythm> rColl = new ObservableCollection<Rhythm>();
            foreach (object child in spRhythm.Children)
            {
                if(child.GetType().Name == "Rhythm")
                {
                    rColl.Add((Rhythm)child);
                }
            }
            string str = JsonConvert.SerializeObject(rColl[0]._firstNote.Type);
        }

        private void BtnAddToCollMetr_Click(object sender, RoutedEventArgs e)
        {
            //if(spMetroColl.Children.Count > 6)
            //{
            //    return;
            //}

            CustomTactForAutoPlay_mini customTact 
                = new CustomTactForAutoPlay_mini(TactType.TT_EIGHTH, MetronomeFirstBitSound, MetronomeSecondBitSound);

            selectedListMetroColl.Add(customTact);
            spMetroColl.Children.Add(customTact);
        }

        private void btnPlayMetronomList_Click(object sender, RoutedEventArgs e)
        {
            int i;
            Task.Run(() => 
            {
                do
                {
                    i = 0;
                    foreach (CustomTactForAutoPlay_mini customTact in selectedListMetroColl)
                    {
                        customTact.Play();
                        Dispatcher.Invoke(() => 
                        {
                            svMetroColl.ScrollToHorizontalOffset(260 * i);
                        });
                        i++;
                    }
                } while (RepeateMetronomList);
            });            
        }

        private void BtnRepeateMetrList_Click(object sender, RoutedEventArgs e)
        {
            RepeateMetronomList = !RepeateMetronomList;
            SetRepeateMetronomListState(RepeateMetronomList);
        }
        /// <summary> меняем стиль у кнопки </summary>
        /// <param name="_state"> передавай переменную, отвечающую за повтор листа метронома </param>
        void SetRepeateMetronomListState(bool _state)
        {
            if(_state)
            {
                BtnRepeateMetrList.Style = (Style)BtnRepeateMetrList.FindResource("BtnCheckBox_True");   
            }
            else
            {
                BtnRepeateMetrList.Style = (Style)BtnRepeateMetrList.FindResource("BtnCheckBox_False");   
            }
        }

        private void btnSaveMetroColl_Click(object sender, RoutedEventArgs e)
        {
            g_PlayListMetroColl.Add(selectedListMetroColl);

            CustomTactForAutoPlay_mini.Save(g_PlayListMetroColl);

            selectedListMetroColl = new ObservableCollection<CustomTactForAutoPlay_mini>();
            spMetroColl.Children.Clear();
        }

        private void btnSaveMetroNew_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
