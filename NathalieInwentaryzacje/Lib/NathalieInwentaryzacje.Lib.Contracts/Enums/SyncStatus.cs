using System.ComponentModel;

namespace NathalieInwentaryzacje.Lib.Contracts.Enums
{
    public enum SyncStatus
    {
        [Description("Brak informacji")]
        Unknown = 0,
        [Description("Wymaga odświeżenia")]
        Modified = 1,
        [Description("Zsynchronizowany")]
        UpToDate = 2,

        [Description("Brak połączenia")]
        NotConnected=3
    }
}
