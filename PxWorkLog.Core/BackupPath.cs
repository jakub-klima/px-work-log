using System;
using System.IO;
using System.Text.Json;

namespace PxWorkLog.Core
{
    public class BackupPath
    {
        private readonly string directory = LoadDirectory();

        private static string LoadDirectory()
        {
            try
            {
                string text = File.ReadAllText("settings.json");
                JsonDocument document = JsonDocument.Parse(text);
                JsonElement element = document.RootElement.GetProperty("backupDirectory");
                return element.GetString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string GetPath()
        {
            DateTime date = DateTime.Now;
            string prefix = $"log {date.Day}-{date.Month}";

            for (int i = 0; ; i++)
            {
                string suffix = i > 0 ? $" ({i})" : "";
                string result = Path.Combine(directory, $"{prefix}{suffix}.png");
                
                if (!File.Exists(result))
                {
                    return result;
                }
            }
        }
    }
}
