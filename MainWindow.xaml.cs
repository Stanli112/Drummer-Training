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
        #endregion

        enum BitChoice
        {
            LEFT,
            RIGHT
        }

        private const int BitCount = 12;
        private int Tact = 470;
        private int Repeate = 0;
        private SoundPlayer wuw;
               

        private ObservableCollection<CoordinationTask> g_PlayListColl = new ObservableCollection<CoordinationTask>();
        private ObservableCollection<OneBit> BitColl = new ObservableCollection<OneBit>();

        Settings settings;

        public MainWindow()
        {
            InitializeComponent();

            Tact = Convert.ToInt32( Convert.ToDouble(txtTact.Text) * 1000);
            Repeate = Convert.ToInt32(txtRepeat.Text);

            wuw = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\cut.wav");
            wuw.Load();

            //LbTaskColl.ItemsSource = g_PlayListColl;
        }


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

        private void SetLabelColor(StackPanel sp, int numb, SolidColorBrush color)
        {
            if(numb < 0)
            {
                return;
            }

            ((Label)sp.Children[numb]).Foreground = color;
        }
       
        private void Start()
        {
            int i = 0;

            for(int k = 0; k < Repeate * BitCount; k++)
            {
                Thread.Sleep(Tact);
                wuw.Play();
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

        private void txtTact_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(txtTact.Text);
                if (d > 0)
                {
                    Tact = Convert.ToInt32(d * 1000);
                    txtTact.Background = Brushes.White;
                }
            }
            catch
            {
                txtTact.Background = Brushes.Red;
            }
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
        private void isEnable(bool isEnable)
        {
            PlayStop.IsEnabled = isEnable;
            txtRepeat.IsEnabled = isEnable;
        }

        private void CreateCoordinationTasks()
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

            for ( int i = 0; i < 12; i++)
            {
                t.AddInOneBitColl(coll[rand.Next(coll.Count)]);
            }
            FillLabelOnMainWindow(t.GetOneBitColl());
        }

        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            CreateCoordinationTasks();
        }

        private void BtnAddTaskInColl_Click(object sender, RoutedEventArgs e)
        {
            //string hand = TbAddTaskInColl_Hand.Text.Replace(" ", "");
            //string leg = TbAddTaskInColl_Hand.Text.Replace(" ", "");

            //if(hand.Length != 12 && leg.Length != 12)
            //{
            //    return;
            //}

            //CoordinationTask cTask = new CoordinationTask(hand, leg);

            //g_PlayListColl.Add(cTask);
        }
        void FillLabelOnMainWindow(ObservableCollection<OneBit> arBits)
        {
            for (int i = 0; i < BitCount; i++)
            {
                ((Label)spHand.Children[i]).Content = arBits[i].hand;
                ((Label)spLeg.Children[i]).Content = arBits[i].leg;
            }
        }

        private void BtnSTakt_Click(object sender, RoutedEventArgs e)
        {
            double tact = Convert.ToDouble( txtTact.Text );
            if (((Button)sender).Name == "BtnTaktMinus")
            {
                if(tact > 0.3)
                {
                    tact -= 0.1;
                }
            }
            if (((Button)sender).Name == "BtnTaktPlus")
            {
                if (tact < 2.0)
                {
                    tact += 0.1;
                }
            }
            txtTact.Text = tact.ToString();
            Tact = Convert.ToInt32(tact * 1000);
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
            Repeate = rep;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
    }
}
