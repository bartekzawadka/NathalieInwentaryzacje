using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class SyncManagerFactory
    {
        public static ISyncManager Get(SettingsInfo settings)
        {
            return new SyncManager(settings);
        }
    }
}
