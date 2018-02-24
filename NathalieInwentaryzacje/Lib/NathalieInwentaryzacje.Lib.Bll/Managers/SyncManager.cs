using System;
using System.Collections.ObjectModel;
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
            using (var client = new SvnClient())
            {
                client.Authentication.ForceCredentials(SettingsInfo.RepoUser, SettingsInfo.RepoPassword);

                if (!Directory.Exists(DataFolder) || !Directory.Exists(Path.Combine(DataFolder, ".svn")))
                {
                    client.CheckOut(new SvnUriTarget(new Uri(SettingsInfo.RepoAddress)), DataFolder);
                    return;
                }

                client.GetStatus(DataFolder, out var changedFiles);

                foreach (var svnStatusEventArgse in changedFiles)
                {
                    if (svnStatusEventArgse.LocalContentStatus == SvnStatus.Missing)
                    {
                        if (File.Exists(svnStatusEventArgse.Path))
                        {
                            var delArgs = new SvnDeleteArgs();
                            delArgs.KeepLocal = true;
                            client.Delete(svnStatusEventArgse.Path, delArgs);
                        }
                        else
                        {
                            client.Delete(svnStatusEventArgse.Path);
                        }
                    }

                    if (svnStatusEventArgse.LocalContentStatus == SvnStatus.NotVersioned)
                    {
                        client.Add(svnStatusEventArgse.Path);
                    }
                }

                var ca = new SvnCommitArgs {LogMessage = "Synchronizacja obiektów inwentaryzacji"};

                client.Commit(DataFolder, ca);
            }
        }
    }
}
