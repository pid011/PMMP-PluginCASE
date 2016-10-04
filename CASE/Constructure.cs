using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CASE
{
    public class Constructure
    {
        private const string CASE_FOLDER_NAME = "CASE";

        private string mainDir;
        private string srcDir;

        public string MainDirectory
        {
            get
            {
                return mainDir;
            }
        }

        public ConstructureInfo Info { get; }

        public event EventHandler CreateSuccessedEvent;

        public Constructure(ConstructureInfo info)
        {
            this.Info = info;
        }

        public async void Start()
        {
            await CreateFolderAsync();
            await Task.WhenAll(CreateYMLAsync(), CreatePHPAsync());

            this.CreateSuccessedEvent(this, null);
        }

        private async Task CreateFolderAsync()
        {
            await Task.Run(() =>
             {
                 string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                 string CASEDir = Path.Combine(myDocuments, CASE_FOLDER_NAME);

                 if (!Directory.Exists(CASEDir))
                 {
                     Directory.CreateDirectory(CASEDir);
                 }

                 mainDir = Path.Combine(CASEDir, Info.PluginName);
                 if (Directory.Exists(mainDir))
                 {
                     int i = 1;
                     while (true)
                     {
                         string ddir = Path.Combine(CASEDir, Info.PluginName + i);
                         if (!Directory.Exists(ddir))
                         {
                             mainDir = ddir;
                             break;
                         }
                         i++;
                     }
                 }
                 srcDir = Path.Combine(mainDir, "src");
                 srcDir = Path.Combine(srcDir, Info.Author, Info.PluginName);
                 Directory.CreateDirectory(srcDir);
             });
        }

        private async Task CreateYMLAsync()
        {
            await Task.Run(async () =>
            {
                string source = SourceGen.CreatePluginYML(this.Info);
                string path = Path.Combine(mainDir, "plugin.yml");
                using (StreamWriter writer = new StreamWriter(path))
                {
                    await writer.WriteAsync(source);
                }
            });
        }

        private async Task CreatePHPAsync()
        {
            await Task.Run(async () =>
            {
                string source = SourceGen.CreateMainPHP(this.Info);
                string path = Path.Combine(srcDir, $"{Info.PluginName}.php");
                using (StreamWriter writer = new StreamWriter(path))
                {
                    await writer.WriteAsync(source);
                }
            });
        }
    }

    public class ConstructureInfo
    {
        public string Author { get; }
        public string PluginName { get; }
        public string PluginVersion { get; }
        public List<string> API { get; }

        public ConstructureInfo(string author, string pluginName, string pluginVersion, List<string> api)
        {
            this.Author = author;
            this.PluginName = pluginName;
            this.PluginVersion = pluginVersion;
            this.API = api;
        }
    }
}