using System.IO;
using System.Linq;

namespace Fb2Kindle
{
    internal class KindleFinder
    {
        public string Mount { get; }

        public string Documents { get; }

        public bool IsFound { get { return !string.IsNullOrEmpty(Mount); } }

        public KindleFinder()
        {
            var driveList = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable);
            foreach (var drive in driveList)
            {
                var letter = drive.Name;
                var docsDir = Path.Combine(letter, "documents");
                var sysDir = Path.Combine(letter, "system");
                if (Directory.Exists(docsDir) && Directory.Exists(sysDir))
                {
                    // Kindle 4, 5
                    var kindle45File = Path.Combine(sysDir, "com.amazon.ebook.booklet.reader", "reader.pref");
                    if (File.Exists(kindle45File))
                    {
                        Mount = letter;
                        Documents = docsDir;
                    }

                    // Kindle Paperwhite, Voyage, Oasis
                    var thumbnailsDir = Path.Combine(sysDir, "thumbnails");
                    var versionFile = Path.Combine(sysDir, "version.txt");
                    if (Directory.Exists(thumbnailsDir) && File.Exists(versionFile))
                    {
                        Mount = letter;
                        Documents = docsDir;
                    }
                }
            }
        }
    }
}
