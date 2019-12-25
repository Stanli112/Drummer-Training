using System.IO;
using System.Windows.Media;

namespace CoordinationTraining.Classes
{
    public class Settings
    {
        ///// <summary> Имя файлв настроек </summary>
        //public string FileName{ get => fileName; }

        /// <summary> Путь к файлу списка с тренировками </summary>
        public string pathToTreningListFile { get; set; }

        /// <summary> Цвет подсветки букв, во время работы проги </summary>
        public SolidColorBrush colorIllumination { get; set; }
       
        /// <summary> Количество повторов выполняемого задания </summary>
        public int RepeatCount { get; set; }

        /// <summary> Количество повторов выполняемого задания </summary>
        public int Amplitude { get; set; }

        public Settings()
        {
            colorIllumination = Brushes.Green;
            RepeatCount = 2;
            Amplitude = 1 * 1000;
        }

        public Settings(SolidColorBrush brush, int repeate, int amplitude)
        {
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

            colorIllumination = brush;
            RepeatCount = repeate;
            Amplitude = amplitude * 1000;
        }
    }
}
