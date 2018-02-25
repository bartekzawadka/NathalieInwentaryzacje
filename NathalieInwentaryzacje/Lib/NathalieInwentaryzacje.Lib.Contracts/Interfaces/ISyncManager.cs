using System.Threading.Tasks;
using NathalieInwentaryzacje.Lib.Contracts.Enums;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface ISyncManager
    {
        Task Synchronize();

        SyncStatus GetStatus();
    }
}
