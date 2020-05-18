using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;

namespace CoordinationTraining.Classes
{
    public class Settings
    {
        /// <summary> Имя файла с настройками </summary>
        public readonly static string settingsFileName = "Settings.json";
        /// <summary> Дефолтный путь к папке с файлами ( ...exe/Files/... ) </summary>
        public readonly static string defaultPathFolder = SetDefaultPathFolder();     
        /// <summary> Количество повторов выполняемого задания </summary>
        public int BPM { get; set; }




        public Settings()
        {
            BPM = 60;
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
                    try
                    {
                        settings = JsonConvert.DeserializeObject<Settings>(_settings);
                    }
                    catch
                    {
                        settings = new Settings();
                    }                    
                }
                else
                {
                    settings = new Settings();
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
        
    }
}
