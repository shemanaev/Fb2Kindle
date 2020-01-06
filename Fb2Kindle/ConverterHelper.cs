using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Fb2Kindle
{
    internal class ConverterHelper
    {
        private readonly string _path;
        private readonly string _config;

        public bool IsFound { get; }
        public string Log { get; private set; }

        public ConverterHelper()
        {
            var fb2mobi1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fb2c.exe");
            var fb2mobi2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fb2converter", "fb2c.exe");

            var ext = new List<string> { "toml", "yaml", "yml", "json" };
            var configs = Directory
                .EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.*", SearchOption.AllDirectories)
                .Where(s => ext.Contains(Path.GetExtension(s).Substring(1).ToLowerInvariant()));
            _config = configs.FirstOrDefault();

            if (File.Exists(fb2mobi1) && _config != null)
            {
                IsFound = true;
                _path = fb2mobi1;
            }
            if (File.Exists(fb2mobi2) && _config != null)
            {
                IsFound = true;
                _path = fb2mobi2;
            }
        }

        public bool Run(string source, string destination)
        {
            if (!IsFound) return false;

            var isEpub = source.ToLowerInvariant().EndsWith(".epub");
            var action = isEpub ? "transfer" : "convert";

            var log = new StringBuilder(1000);
            var fb2mobi = new Process();
            fb2mobi.StartInfo.CreateNoWindow = true;
            fb2mobi.StartInfo.UseShellExecute = false;
            fb2mobi.StartInfo.RedirectStandardOutput = true;
            fb2mobi.StartInfo.RedirectStandardError = true;
            fb2mobi.StartInfo.FileName = _path;
            fb2mobi.StartInfo.WorkingDirectory = Path.GetDirectoryName(_path);
            fb2mobi.StartInfo.Arguments = $@"-c ""{_config}"" {action} --nodirs --ow --to mobi ""{source}"" ""{destination}""";
            fb2mobi.OutputDataReceived += (sender, args) => log.AppendLine(args.Data);
            fb2mobi.ErrorDataReceived += (sender, args) => log.AppendLine(args.Data);

            fb2mobi.Start();
            fb2mobi.BeginOutputReadLine();
            fb2mobi.BeginErrorReadLine();

            fb2mobi.WaitForExit();
            Log = log.ToString();

            return (fb2mobi.ExitCode == 0) && !Log.Contains("ERROR");
        }
    }
}
