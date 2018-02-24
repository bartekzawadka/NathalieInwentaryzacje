using System.IO;

namespace NathalieInwentaryzacje.Lib.Contracts.Dto.Settings
{
    public class DataLocationInfo
    {
        public string MainDirPath { get; set; }

        public string RecordsDirName { get; set; }

        public string TemplatesDirName { get; set; }

        public string RecordsPath =>
            string.IsNullOrEmpty(MainDirPath) ? null : Path.Combine(MainDirPath, RecordsDirName);

        public string TemplatesPath =>
            string.IsNullOrEmpty(MainDirPath) ? null : Path.Combine(MainDirPath, TemplatesDirName);
    }
}
