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

namespace CoordinationTraining
{
    /// <summary>
    /// Придумать файл настроек
    /// Больше констант
    /// Добавить скрытие, развёртывание окна и ресайз
    /// Такт и кол-во повторов нужно добавить загрузгойиз файла настроек
    /// Нормальное дабовление записей + возможность добавлять текущие записи 
    /// И отдельный json в загрузку записей
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Title
        private void BtnWindowExtend_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }
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
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            settings.SaveSettings();
            settings.SaveTrainingColl(g_PlayListColl);
        }
        #endregion

        enum BitChoice
        {
            LEFT,
            RIGHT
        }


        private readonly int BitCount = 12;

        /// <summary> Звук метронома - используется в тренеровке </summary>
        private SoundPlayer MetronomeSound;
        /// <summary> Хранит настройки проекта </summary>        
        private Settings settings = new Settings();

        private ObservableCollection<CoordinationTask> g_PlayListColl = new ObservableCollection<CoordinationTask>();


        public MainWindow()
        {
            InitializeComponent();
            
            settings.LoadSettings(ref settings);
            settings.LoadTrainingColl(ref g_PlayListColl);

            GetControlsData(settings);

            MetronomeSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\cut.wav");
            MetronomeSound.Load();

            LbTaskColl.ItemsSource = g_PlayListColl;
        }



        #region Центральная панель

        private void Start()
        {
            int i = 0;

            for(int k = 0; k < settings.RepeatCount * BitCount; k++)
            {
                Thread.Sleep(settings.Amplitude);
                MetronomeSound.Play();
                Dispatcher.Invoke(() =>
                {
                    SetLabelColor(spHand, i, Brushes.Green);
                    SetLabelColor(spLeg, i, Brushes.Green);
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

                if (i == BitCount)
                {
                    i = 0;
                }
            }
            Dispatcher.Invoke(() => 
            { 
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
        #endregion

        #region Левая менюшка с Амплитудой и Повторами

        private void BtnSAmplitude_Click(object sender, RoutedEventArgs e)
        {
            double amp = Convert.ToDouble(txtAmplitude.Text );
            if (((Button)sender).Name == "BtnAmplitudeMinus")
            {
                if(amp > 0.3)
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
        
        private void BtnAddTaskInColl_Click(object sender, RoutedEventArgs e)
        {
            //string hand = TbAddTaskInColl_Hand.Text.Replace(" ", "");
            //string leg = TbAddTaskInColl_Hand.Text.Replace(" ", "");

            //if(hand.Length != 12 && leg.Length != 12)
            //{
            //    return;
            //}

            //g_PlayListColl.Add(new CoordinationTask(hand, leg));
        }

        #endregion



        #region Вспомогательные функции

        /// <summary> Задаёт в контролы значения из настроек </summary>
        void GetControlsData(Settings settings)
        {
            txtAmplitude.Text = (Convert.ToDouble(settings.Amplitude) / 1000).ToString();
            txtRepeat.Text = settings.RepeatCount.ToString();
        }
        
        /// <summary> Включает/Отключает кнопки при тренеровке </summary>
        private void isEnable(bool isEnable)
        {
            PlayStop.IsEnabled = isEnable;
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

    }
}
