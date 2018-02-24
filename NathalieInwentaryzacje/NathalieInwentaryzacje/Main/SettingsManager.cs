using System.IO;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Properties;

namespace NathalieInwentaryzacje.Main
{
    internal class SettingsManager
    {
        public static void SaveSettings(SettingsInfo settings)
        {
            Settings.Default.NumberOfReportRecords =
                settings.NumberOfReportRows == 0 ? 40 : settings.NumberOfReportRows;
            Settings.Default.RepoAddress = settings.RepoAddress;
            Settings.Default.RepoPassword = settings.RepoPassword;
            Settings.Default.RepoUser = settings.RepoUser;

            Settings.Default.Save();
        }

        public static SettingsInfo GetSettings()
        {
            return new SettingsInfo
            {
                NumberOfReportRows = Settings.Default.NumberOfReportRecords,
                RepoAddress = Settings.Default.RepoAddress,
                RepoPassword = Settings.Default.RepoPassword,
                RepoUser = Settings.Default.RepoUser
            };
        }

        public static DataLocationInfo GetDataLocationInfo()
        {
            var dli = new DataLocationInfo
            {
                RecordsDirName = Settings.Default.RecordsDirName,
                TemplatesDirName = Settings.Default.TemplatesDirName
            };

            if (string.IsNullOrEmpty(Settings.Default.MainDataPath))
                dli.MainDirPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data");
            else if (!Path.IsPathRooted(Settings.Default.MainDataPath))
            {
                dli.MainDirPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    Settings.Default.MainDataPath);
            }
            else
            {
                dli.MainDirPath = Settings.Default.MainDataPath;
            }

            return dli;
        }
    }
}
