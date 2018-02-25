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
        }
    }
}
