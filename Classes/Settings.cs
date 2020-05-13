using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;

namespace CoordinationTraining.Classes
{
    public class Settings
    {
        /// <summary> Имя файла с записями тренировок </summary>
        public readonly static string trainingCollFileName = "Training.json";
        /// <summary> Имя файла с настройками </summary>
        public readonly static string settingsFileName = "Settings.json";
        /// <summary> Дефолтный путь к папке с файлами ( ...exe/Files/... ) </summary>
        public readonly static string defaultPathFolder = SetDefaultPathFolder();

        ///// <summary> Имя файлв настроек </summary>
        //public string FileName{ get => fileName; }

        /// <summary> Путь к файлу списка с тренировками </summary>
        public string pathToTreningCollFile { get; set; }

        /// <summary> Цвет подсветки букв, во время работы проги </summary>
        public SolidColorBrush colorIllumination { get; set; }
       
        /// <summary> Количество повторов выполняемого задания </summary>
        public int RepeatCount { get; set; }

        /// <summary> Количество повторов выполняемого задания </summary>
        public int Amplitude { get; set; }
        public int BPS { get; set; }

        /// <summary> Использовать метроном </summary>
        public bool UseMetronom { get; set; }

        public Settings()
        {
            //pathToTreningCollFile = MainWindow.SetDefaultPathFolder();
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

        /// <summary> Возвращает дефолтный путь к папке с файлами </summary>
        public static string SetDefaultPathFolder()
        {
            string _path = Directory.GetCurrentDirectory() + "\\Files";
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            return _path;
        }
        
        /// <summary> Сохранение настроек из settings в файла </summary>
        public bool SaveSettings()
        {
            CheckDefaultFolderAndFile();
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText($"{defaultPathFolder}\\{settingsFileName}", json);
            return true;
        }

        /// <summary> Загрузка настроек из файла в settings </summary>
        public bool LoadSettings(ref Settings settings)
        {
            if (!CheckDefaultFolderAndFile())
            {
                settings = new Settings();
            }
            else
            {
                string _settings = File.ReadAllText($"{defaultPathFolder}\\{settingsFileName}");
                if (_settings.Length != 0)
                {
                    settings = JsonConvert.DeserializeObject<Settings>(_settings);
                }
                else
                {
                    settings = new Settings();
                }
            }

            return true;
        }

        /// <summary> Сохранение настроек из settings в файла </summary>
        public bool SaveTrainingColl(ObservableCollection<CoordinationTask> coordTasks)
        {
            CheckTrainingFile();
            string json = JsonConvert.SerializeObject(coordTasks);
            File.WriteAllText($"{this.pathToTreningCollFile}\\{trainingCollFileName}", json);
            return true;
        }

        /// <summary> Загрузка настроек из файла в settings </summary>
        public bool LoadTrainingColl(ref ObservableCollection<CoordinationTask> coll)
        {
            if (!CheckTrainingFile())
            {
                return false;
            }
            else
            {
                string _coll = File.ReadAllText($"{this.pathToTreningCollFile}\\{trainingCollFileName}");
                if (_coll.Length != 0)
                {
                    coll = JsonConvert.DeserializeObject<ObservableCollection<CoordinationTask>>(_coll);
                    
                    foreach(CoordinationTask ct in coll)
                    {
                        ct.SetArray();
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary> Проверяет наличие дефолтной папки с файлом настроек,
        ///  если таковых нет - они будут созданы </summary>
        bool CheckDefaultFolderAndFile()
        {
            bool result = true;
            if (!Directory.Exists(defaultPathFolder))
            {
                Directory.CreateDirectory(defaultPathFolder);
            }
            if (!File.Exists($"{defaultPathFolder}\\{settingsFileName}"))
            {
                File.Create($"{defaultPathFolder}\\{settingsFileName}");
                result = false;
            }
            return result;
        }
        
        /// <summary> Проверяет наличие файла с тренировками и папки, в которой они находятся </summary>
        bool CheckTrainingFile()
        {
            bool result = true;
            // есть ли настройки
            if (this == null)
            {
                return false;
            }
            if (this.pathToTreningCollFile == null || this.pathToTreningCollFile == "")
            {
                this.pathToTreningCollFile = SetDefaultPathFolder();
            }

            // проверяем пути
            if (!Directory.Exists(this.pathToTreningCollFile))
            {
                Directory.CreateDirectory(this.pathToTreningCollFile);
            }

            if (!File.Exists($"{this.pathToTreningCollFile}\\{trainingCollFileName}"))
            {
                File.Create($"{this.pathToTreningCollFile }\\{trainingCollFileName}");
                result = false;
            }
            return result;
        }
    }
}
