
using System;
using System.IO;
using System.Text.Json;

namespace Sidecab.IO
{
    public sealed class SettingsJson : Model.ISettingsFile
    {
        private static string FilePath => FolderPath + "Settings.json";

        //----------------------------------------------------------------------
        private static string FolderPath =>
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            + @"\Sidecab\";


        //----------------------------------------------------------------------
        private static readonly JsonSerializerOptions _options = new()
        {
            IncludeFields = false,
            IgnoreReadOnlyProperties = true,
            WriteIndented = true,
        };


        //----------------------------------------------------------------------
        public Model.Settings? Load()
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
            try
            {
                if (Directory.Exists(FolderPath) == false)
                {
                    Directory.CreateDirectory(FolderPath);
                }

                using FileStream stream = new(FilePath, FileMode.OpenOrCreate);
                JsonSerializer.Serialize<Model.Settings>(stream, settings, _options);
            }
            catch
            {
                return false;
            }

            return true;
        }
     }
}
