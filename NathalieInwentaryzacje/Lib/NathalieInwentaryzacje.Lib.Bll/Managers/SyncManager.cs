using System;
using System.IO;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using SharpSvn;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class SyncManager : ISyncManager
    {
        private SettingsInfo SettingsInfo { get; set; }

        private static readonly string DataFolder = Path.Combine(Path.GetFullPath("."), "Data");

        internal SyncManager(SettingsInfo settings)
        {
            SettingsInfo = settings;
        }

        public void Synchronize()
        {
            CheckoutIfNoRepo();

        }

        public void CheckoutIfNoRepo()
        {
            if (Directory.Exists(DataFolder) && Directory.Exists(Path.Combine(DataFolder, ".svn"))) return;

            using (var client = new SvnClient())
            {
                client.Authentication.ForceCredentials(SettingsInfo.RepoUser, SettingsInfo.RepoPassword);
                client.CheckOut(new SvnUriTarget(new Uri(SettingsInfo.RepoAddress)), DataFolder);
            }
        }
    }
}
