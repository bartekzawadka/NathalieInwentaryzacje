using System.IO;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public abstract class ManagerBase
    {
        protected static DataLocationInfo Paths { get; set; }

        protected ManagerBase(DataLocationInfo paths)
        {
            Paths = paths;

            if (!Directory.Exists(paths.MainDirPath))
                Directory.CreateDirectory(paths.MainDirPath);
            if (!Directory.Exists(paths.RecordsPath))
                Directory.CreateDirectory(paths.RecordsPath);
            if (!Directory.Exists(paths.TemplatesPath))
                Directory.CreateDirectory(paths.TemplatesPath);
        }
    }
}
