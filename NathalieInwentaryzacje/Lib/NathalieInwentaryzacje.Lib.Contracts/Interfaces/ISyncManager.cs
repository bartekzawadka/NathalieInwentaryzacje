using NathalieInwentaryzacje.Lib.Contracts.Enums;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface ISyncManager
    {
        void Synchronize();

        SyncStatus GetStatus();
    }
}
