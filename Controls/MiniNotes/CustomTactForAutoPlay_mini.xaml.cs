using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <summary> Имя файла с записями тренировок </summary>
        public readonly static string trainingCollFileName = "MetroTraining.json";
        enum BTN
        {
            BTN_REPEAT_PLUS = 10,
            BTN_REPEAT_MINUS, 
            BTN_BPM_PLUS,
            BTN_BPM_MINUS
        }
        
        /// <summary> Для парсера </summary>
        private const char _separator = '|';

        public TactType _type = TactType.TT_FOURTH;
        public int _BPM = 60;
        public int _repeat = 3;

        /// <summary> Звук метронома - используется в тренеровке </summary>
        private SoundPlayer FirstBitSound;
        private SoundPlayer SecondBitSound;
        
        public CustomTactForAutoPlay_mini()
        {
            InitializeComponent();
            InitTT();

            FirstBitSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\firstBit.wav");
            FirstBitSound.Load();

            SecondBitSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\secondBit2.wav");
            SecondBitSound.Load();
        }
        
        public CustomTactForAutoPlay_mini(TactType type, int _bpm, int _rep)
        {
            _type = type;
            _BPM = _bpm;
            _repeat = _rep;

            InitializeComponent();
            InitTT();
            InitControls(_BPM, _repeat);

            FirstBitSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\firstBit.wav");
            FirstBitSound.Load();

            SecondBitSound = new SoundPlayer(Directory.GetCurrentDirectory() + "\\Resources\\secondBit2.wav");
            SecondBitSound.Load();
        }

        public CustomTactForAutoPlay_mini(TactType type, SoundPlayer _fs, SoundPlayer _ss)
        {
            InitializeComponent();

            FirstBitSound   = _fs;
            SecondBitSound  = _ss;
            _type           = type;
            _BPM            = 60;
            _repeat         = 4;

            InitTT();
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
        
        void InitControls(int _bpm, int _rep)
        {
            tbBPM.Text = _bpm.ToString();
            tbRepeateCount.Text = _rep.ToString();
        }

        public void Play()
        {
            // чем блочить все контроллы, буду перекрывать их
            Dispatcher.Invoke(() => 
            { 
                recEnable.Visibility = Visibility.Visible;
                GridHelper.Visibility = Visibility.Visible;
            });

            for (int i = 0; i < _repeat; i++)   // кол-во повторов
            {
                Dispatcher.Invoke(() =>
                {
                    tbHelper.Text = (i + 1).ToString();
                });
                double _pause = 1000 / ((double)_BPM / 60 * (int)_type);
                Thread.Sleep((int)_pause);

                FirstBitSound.Play();

                for (int j = 0; j < (int)_type - 1; j++)
                {                    
                    Thread.Sleep((int)_pause);                    
                    SecondBitSound.Play();
                }
            }

            Dispatcher.Invoke(() => 
            { 
                recEnable.Visibility = Visibility.Hidden;
                GridHelper.Visibility = Visibility.Hidden;
            });
        }

        /// <summary> Парсит элемент класса в строку </summary>
        /// <returns> Строка с основными параметрами (тип, ударов в сек, кол-во повторов) </returns>
        private string Parse()
        {
            return $"{Convert.ToInt32(this._type).ToString()}{_separator}{this._BPM}{_separator}{this._repeat}{_separator}";
        }
        
        /// <summary> Собирает из строки контролл </summary>
        /// <param name="str"> Строка с осн. параметрами (тип, ударов в сек, кол-во повторов) </param>
        /// <returns> возвращает контролл </returns>
        private CustomTactForAutoPlay_mini UnParse(string str)
        {
            //проверка на кол-во разделителей
            int findSep = 0;
            foreach (char c in str)
            {
                if(c == _separator)
                {
                    findSep++;
                }
            }
            if(findSep != 3)
            {
                return new CustomTactForAutoPlay_mini();
            }

            // исчим
            string[] _params = new string[3];   // храним свойства
            int i = 0;                          // счётчик, для перехода на след свойство

            foreach (char c in str)
            {
                if(c != _separator)
                {
                    _params[i] += c.ToString();
                }
                else
                {                    
                    i++;
                }
            }

            // возвращаем
            return new CustomTactForAutoPlay_mini
                (
                    (TactType)Convert.ToInt32(_params[0]), 
                    Convert.ToInt32(_params[1]), 
                    Convert.ToInt32(_params[2])
                );
        }

        /// <summary> Парсит коллецию класса в строку </summary>
        /// <param name="ParseCollection"> коллекция с контроллами </param>
        /// <returns> Ту самую строку </returns>
        private static string ParseRecord(ObservableCollection<CustomTactForAutoPlay_mini> ParseCollection)
        {
            ObservableCollection<string> saveColl = new ObservableCollection<string>();
            
            for (int i = 0; i < ParseCollection.Count; i++)
            {
                saveColl.Add(ParseCollection[i].Parse());
            }

            return JsonConvert.SerializeObject(saveColl);
        }

        /// <summary> Собирает из строки коллекцию контроллов </summary>
        /// <param name="str"> Строка с осн. параметрами (тип, ударов в сек, кол-во повторов) контроллов </param>
        /// <returns> возвращает коллекцию контроллов </returns>
        private static ObservableCollection<CustomTactForAutoPlay_mini> UnParseRecord(string recordStr)
        {
            ObservableCollection<CustomTactForAutoPlay_mini> ContrColl = new ObservableCollection<CustomTactForAutoPlay_mini>();
            ObservableCollection<string> paramsColl = new ObservableCollection<string>();
            paramsColl = JsonConvert.DeserializeObject<ObservableCollection<string>>(recordStr);                       

            foreach(string s in paramsColl)
            {
                ContrColl.Add(new CustomTactForAutoPlay_mini().UnParse(s));
            }

            return ContrColl;
        }
        
        /// <summary> Парсит коллекцию записей и записывает её в .json </summary>
        /// <param name="saveColl"> Коллекция, которую нужно запарсить </param>
        /// <returns> возвращает результат парсинга </returns>
        public static bool Save(ObservableCollection<ObservableCollection<CustomTactForAutoPlay_mini>> saveColl)
        {
            try
            {
                ObservableCollection<string> _res = new ObservableCollection<string>();
                foreach (ObservableCollection<CustomTactForAutoPlay_mini> item in saveColl)
                {
                    _res.Add(ParseRecord(item));
                }

                string json = JsonConvert.SerializeObject(_res);
                CheckTrainingFile();
                File.WriteAllText($"{SetDefaultPathFolder()}\\{trainingCollFileName}", json);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        /// <summary> Загружает записи из файла .jsone </summary>
        /// <returns> Возвращает коллекцию записей </returns>
        public static ObservableCollection<ObservableCollection<CustomTactForAutoPlay_mini>> Load()
        {
            CheckTrainingFile();
            string recordsCollStr = File.ReadAllText($"{SetDefaultPathFolder()}\\{trainingCollFileName}");
            ObservableCollection<ObservableCollection<CustomTactForAutoPlay_mini>> result 
                = new ObservableCollection<ObservableCollection<CustomTactForAutoPlay_mini>>();

            ObservableCollection<string> _str = JsonConvert.DeserializeObject<ObservableCollection<string>>(recordsCollStr);

            foreach(string s in _str)
            {
                result.Add(CustomTactForAutoPlay_mini.UnParseRecord(s));
            }

            return result;
        }

        private static bool CheckTrainingFile()
        {
            string path = SetDefaultPathFolder();

            // проверяем пути
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists($"{path}\\{trainingCollFileName}"))
            {
                File.Create($"{path }\\{trainingCollFileName}");
            }
            return true;
        }

        private static string SetDefaultPathFolder()
        {
            string _path = Directory.GetCurrentDirectory() + "\\Files";
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            return _path;
        }
    }
}
