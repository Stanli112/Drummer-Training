﻿using System;
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

namespace CoordinationTraining
{
    public partial class MainWindow : Window
    {
        const string lblMainBeat = "Beat creator";
        const string lblMainCoor = "Coordination training";
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
            settings.SaveTrainingColl(g_PlayListColl);
        }
        #endregion

        enum _page
        {
            beat,
            coord
        }

        /// <summary> Для запуска/остановки метронома </summary>
        bool MetronomUsed = false;


        private readonly int BitCount = 12;
        /// <summary> Количество тактов в одном промежутке (4) </summary>
        private readonly int TactCount = 4;

        /// <summary> Звук метронома - используется в тренеровке </summary>
        private SoundPlayer MetronomeSound;
        /// <summary> Хранит настройки проекта </summary>        
        private Settings settings = new Settings();
        /// <summary> Выбранное в списке задание </summary>        
        private CoordinationTask selCT = new CoordinationTask();

        private ObservableCollection<CoordinationTask> g_PlayListColl = new ObservableCollection<CoordinationTask>();


        public MainWindow()
        {
            InitializeComponent();

            btnMainBeatCreator.Content = lblMainBeat;
            btnMainCoordTraining.Content = lblMainCoor;

            settings.LoadSettings(ref settings);
            settings.LoadTrainingColl(ref g_PlayListColl);

            GetControlsData(settings);

            MetronomeSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\cut.wav");
            MetronomeSound.Load();


            LbTaskColl.ItemsSource = g_PlayListColl;
            GetFirstItemInMaOnWindow();

            ShowPage(_page.beat);
        }

        #region Coordination Training

        #region Центральная панель

        private void Start()
        {
            int i = 0;
            int tc = 0;

            if (settings.UseMetronom)
            {
                MetronomStart(TactCount);
            }            

            for (int k = 0; k < settings.RepeatCount * BitCount; k++)
            {
                tc++;
                Thread.Sleep(settings.Amplitude);
                MetronomeSound.Play();
                Dispatcher.Invoke(() =>
                {
                    lblTactCount.Content = tc.ToString();

                    SetLabelColor(spHand, i, settings.colorIllumination);
                    SetLabelColor(spLeg, i, settings.colorIllumination);
                    if (i == 0)
                    {
                        SetLabelColor(spHand, spHand.Children.Count - 1, Brushes.White);
                        SetLabelColor(spLeg, spLeg.Children.Count - 1, Brushes.White);
                    }
                    else
                    {
                        SetLabelColor(spHand, i - 1, Brushes.White);
                        SetLabelColor(spLeg, i - 1, Brushes.White);
                    }                    
                });

                i++;
                if (tc == TactCount)
                {
                    tc = 0;
                }
                if (i == BitCount)
                {
                    i = 0;
                }
            }
            Dispatcher.Invoke(() =>
            {
                lblTactCount.Content = "";

                isEnable(true);
                SetLabelColor(spHand, spHand.Children.Count - 1, Brushes.White);
                SetLabelColor(spLeg, spLeg.Children.Count - 1, Brushes.White);
            });
        }
        private void TbAddTaskInColl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "F" && e.Text != "L")
            {
                e.Handled = true;
            }
        }
        private void PlayStop_Click(object sender, RoutedEventArgs e)
        {
            isEnable(false);
            Task.Run(() => Start());
        }
        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            CreateRandomCoordinationTasks();
        }
        
        /// <summary> Добавляет в глобальную коллекцию тренеровку с панелей </summary>
        private void BtnAddToColl_Click(object sender, RoutedEventArgs e)
        {
            g_PlayListColl.Add(new CoordinationTask(GetLabelOnMainWindow()));
        }

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

        private void BtnSAmplitude_Click(object sender, RoutedEventArgs e)
        {
            double amp = Convert.ToDouble(txtAmplitude.Text );
            if (((Button)sender).Name == "BtnAmplitudeMinus")
            {
                if(amp > 0.4)
                {
                    amp -= 0.1;
                }
            }
            if (((Button)sender).Name == "BtnAmplitudePlus")
            {
                if (amp < 2.0)
                {
                    amp += 0.1;
                }
            }
            txtAmplitude.Text = amp.ToString();
            settings.Amplitude = Convert.ToInt32(amp * 1000);
        }

        private void BtnSRepeat_Click(object sender, RoutedEventArgs e)
        {
            int rep = Convert.ToInt32(txtRepeat.Text);
            if (((Button)sender).Name == "BtnRepeatMinus")
            {
                if (rep > 1 )
                {
                    rep -= 1;
                }
            }
            if (((Button)sender).Name == "BtnRepeatPlus")
            {
                if (rep < 5.0)
                {
                    rep += 1;
                }
            }
            txtRepeat.Text = rep.ToString();
            settings.RepeatCount = rep;
        }

        #endregion


        #region Правая менюшка

        private void RightMenu_Click(object sender, RoutedEventArgs e)
        {
            if (GridRightMenu.Width > 0)
            {
                BeginStoryboard((Storyboard)this.FindResource("CloseTaskColl"));
            }
            else
            {
                BeginStoryboard((Storyboard)this.FindResource("OpenTaskColl"));
            }            
        }
        
        private void cpIllumination_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            settings.colorIllumination = new SolidColorBrush(cpIllumination.SelectedColor.Value);
        }

        private void cbMetronom_Click(object sender, RoutedEventArgs e)
        {
            settings.UseMetronom = (bool)cbMetronom.IsChecked;
        }
        
        private void BtnRemoveTaskFromColl_Click(object sender, RoutedEventArgs e)
        {
            selCT = (CoordinationTask)((sender as Button).Parent as Grid).DataContext;
            g_PlayListColl.Remove(selCT);

            if(LbTaskColl.Items.Count != 0)
            {
                LbTaskColl.SelectedItem = LbTaskColl.Items[0];
            }            
        }

        private void LbTaskColl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selCT = (CoordinationTask)LbTaskColl.SelectedItem;
            if(selCT != null)
            {
                SetLabelOnMainWindow(selCT.GetOneBitColl());
            }            
        }
        #endregion


        #region Вспомогательные функции

        /// <summary> Проигрывает метроном с выстановленной амплитудой </summary>
        private void MetronomStart(int _bitCount)
        {
            for (int k = 0; k < _bitCount; k++)
            {
                Thread.Sleep(settings.Amplitude);
                MetronomeSound.Play();
                Dispatcher.Invoke(() =>
                {
                    lblTactCount.Content = (k + 1).ToString();
                });
            }
        }

        /// <summary> Задаёт первую запись из коллекции </summary>
        void GetFirstItemInMaOnWindow()
        {
            if (g_PlayListColl.Count != 0)
            {
                CoordinationTask ct = g_PlayListColl[0];
                for (int i = 0; i < BitCount; i++)
                {
                    ((Label)spHand.Children[i]).Content = ct.AllHand.Replace(" ", "")[i];
                    ((Label)spLeg.Children[i]).Content = ct.AllLeg.Replace(" ", "")[i];
                }
            }
        }

        /// <summary> Задаёт в контролы значения из настроек </summary>
        void GetControlsData(Settings settings)
        {
            txtAmplitude.Text = (Convert.ToDouble(settings.Amplitude) / 1000).ToString();
            txtRepeat.Text = settings.RepeatCount.ToString();
            cpIllumination.Background = settings.colorIllumination;
            cbMetronom.IsChecked = settings.UseMetronom;
        }
        
        /// <summary> Включает/Отключает кнопки при тренеровке </summary>
        private void isEnable(bool isEnable)
        {
            PlayStop.IsEnabled = isEnable;
            BtnRepeatMinus.IsEnabled = isEnable;
            PlayMetronom.IsEnabled = isEnable;
        }

        /// <summary> Задаёт цвет конкретного Label на панели (StackPanel) </summary>
        /// <param name="sp"> Панель - на которой расположены Label'ы </param>
        /// <param name="numb"> Номер детёныша (Label) в панели </param>
        /// <param name="color"> Цвет, который нужно задать </param>
        private void SetLabelColor(StackPanel sp, int numb, SolidColorBrush color)
        {
            if (numb < 0)
            {
                return;
            }

            ((Label)sp.Children[numb]).Foreground = color;
        }
        
        /// <summary> Задаёт цвет всех Label на панели (StackPanel) </summary>
        /// <param name="sp"> Панель - на которой расположены Label'ы </param>
        /// <param name="color"> Цвет, который нужно задать </param>
        private void SetForegroundColor(StackPanel sp, SolidColorBrush color)
        {
            Label selLbl;

            for (int i = 0; i < sp.Children.Count; i++)
            {
                if (sp.Children[i].GetType().Name == "Label")
                {
                    selLbl = (Label)sp.Children[i];
                    selLbl.Foreground = color;
                }
            }
        }

        /// <summary> Задает лейблам, находящимся на панелях, контент (R or L) </summary>
        /// <param name="arBits"> Коллекция с элементами (R or L) </param>
        void SetLabelOnMainWindow(ObservableCollection<OneBit> arBits)
        {
            for (int i = 0; i < BitCount; i++)
            {
                ((Label)spHand.Children[i]).Content = arBits[i].hand;
                ((Label)spLeg.Children[i]).Content = arBits[i].leg;
            }
        }

        /// <summary> Возвращает лейблы из панелей в виде коллекции </summary>
        ObservableCollection<OneBit> GetLabelOnMainWindow()
        {
            ObservableCollection<OneBit> coll = new ObservableCollection<OneBit>();
            for (int i = 0; i < BitCount; i++)
            {
                OneBit ob = new OneBit() {
                    hand = ((Label)spHand.Children[i]).Content.ToString(), 
                    leg = ((Label)spLeg.Children[i]).Content.ToString()
                };
                coll.Add(ob);
            }

            return coll;
        }
        
        /// <summary> Задаёт рандомный список битов и заполняет ею панели </summary>
        private void CreateRandomCoordinationTasks()
        {
            CoordinationTask t = new CoordinationTask();
            var rand = new Random();
            OneBit ob1 = new OneBit() { hand = "L", leg = "R" };
            OneBit ob2 = new OneBit() { hand = "L", leg = "L" };
            OneBit ob3 = new OneBit() { hand = "R", leg = "L" };
            OneBit ob4 = new OneBit() { hand = "R", leg = "R" };
            ObservableCollection<OneBit> coll = new ObservableCollection<OneBit>();
            coll.Add(ob1);
            coll.Add(ob2);
            coll.Add(ob3);
            coll.Add(ob4);

            for (int i = 0; i < 12; i++)
            {
                t.AddInOneBitColl(coll[rand.Next(coll.Count)]);
            }
            SetLabelOnMainWindow(t.GetOneBitColl());
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

                        GridBarCoordTrening.Visibility = Visibility.Hidden;
                        GridMainCoordTreaning.Visibility = Visibility.Hidden;
                        break;
                    }

                case _page.coord:
                    {
                        GridBarBeatCreator.Visibility = Visibility.Hidden;
                        GridMainBeatCreator.Visibility = Visibility.Hidden;

                        GridBarCoordTrening.Visibility = Visibility.Visible;
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
    }
}
