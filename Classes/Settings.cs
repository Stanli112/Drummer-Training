using System.IO;
using System.Windows.Media;

namespace CoordinationTraining.Classes
{
    public class Settings
    {
        private static Settings instance;

        /// <summary> Имя файлв настроек </summary>
        public string FileName{ get; set; }

        /// <summary> Путь к файлу настроек </summary>
        public string pathToSettingsFile { get; set; }

        /// <summary> Цвет подсветки букв, во время работы проги </summary>
        public SolidColorBrush colorIllumination { get; set; }
       
        /// <summary> Количество повторов выполняемого задания </summary>
        public int RepeatCount { get; set; }

        /// <summary> Количество повторов выполняемого задания </summary>
        public int Amplitude { get; set; }

        private Settings()
        {
            pathToSettingsFile = SetDefaultPathFolder();
            colorIllumination = Brushes.Green;
            RepeatCount = 2;
            Amplitude = 1 * 1000;
        }

        private Settings(string _path, SolidColorBrush brush, int repeate, int amplitude)
        {
            if(!Directory.Exists(_path))
            {
                _path = SetDefaultPathFolder();
            }
            if(brush == null)
            {
                brush = Brushes.Green;
            }
            if(repeate <= 0)
            {
                repeate = 2;
            }
            if (amplitude <= 0)
            {
                amplitude = 1;
            }

            pathToSettingsFile = _path;
            colorIllumination = brush;
            RepeatCount = repeate;
            Amplitude = amplitude * 1000;
        }

        /// <summary> Возвращает дефолтный путь к папке настроек </summary>
        string SetDefaultPathFolder()
        {
            string _path = Directory.GetCurrentDirectory() + "\\Settings";
            if(!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            return _path;
        }
    }
}
