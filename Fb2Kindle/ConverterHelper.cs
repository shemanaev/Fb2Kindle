using System;
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
            const string ExeName = "fb2c.exe";
            var configExts = new[] { ".toml", ".yaml", ".yml", ".json" };

            var paths = new[] {
                AppDomain.CurrentDomain.BaseDirectory,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fb2converter"),
            }.Where(p => Directory.Exists(p)).ToList();

            var configs = paths.SelectMany(p => Directory.EnumerateFiles(p, "*.*", SearchOption.TopDirectoryOnly)
                .Where(name => configExts.Any(ext => name.EndsWith(ext, StringComparison.OrdinalIgnoreCase))));
            _config = configs.FirstOrDefault();

            foreach (var p in paths)
            {
                var exe = Path.Combine(p, ExeName);
                if (File.Exists(exe) && _config != null)
                {
                    IsFound = true;
                    _path = exe;
                    break;
                }
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
