﻿using System.Threading.Tasks;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Enums;

namespace NathalieInwentaryzacje.Lib.Contracts.Interfaces
{
    public interface ISyncManager
    {
        Task Synchronize(bool update = true);

        SyncStatus GetStatus();

        void UpdateSettings(SettingsInfo settings);
    }
}
