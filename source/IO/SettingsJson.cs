
using System;
using System.IO;
using System.Text.Json;

namespace Sidecab.IO
{
    public sealed class SettingsJson : Model.ISettingsFile
    {
        private static string FilePath => FolderPath + "Settings.json";

        private static string FolderPath =>
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + @"\Sidecab\";


        //----------------------------------------------------------------------
        public Model.Settings Load()
        {
            try
            {
                using var stream = new FileStream(FilePath, FileMode.Open);
                return JsonSerializer.Deserialize<Model.Settings>(stream);
            }
            catch
            {
                return null;
            }
        }

        //----------------------------------------------------------------------
        public bool Save(Model.Settings settings)
        {
            if (Directory.Exists(FolderPath) == false)
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                }
                catch
                {
                    return false;
                }
            }

            var serializerOptions = new JsonSerializerOptions()
            {
                IncludeFields = false,
                IgnoreReadOnlyProperties = true,
                WriteIndented = true,
            };

            try
            {
                using var stream = new FileStream(FilePath, FileMode.OpenOrCreate);
                JsonSerializer.Serialize<Model.Settings>(stream, settings, serializerOptions);
            }
            catch
            {
                return false;
            }

            return true;
        }
     }
}
