﻿using System;
using System.IO;
using System.Linq;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Enums;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using SharpSvn;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class SyncManager : ManagerBase, ISyncManager
    {
        private SettingsInfo SettingsInfo { get; }

        public SyncManager(SettingsInfo settings, DataLocationInfo paths) : base(paths)
        {
            SettingsInfo = settings;
        }

        public SyncStatus GetStatus()
        {
            using (var client = new SvnClient())
            {
                client.Authentication.ForceCredentials(SettingsInfo.RepoUser, SettingsInfo.RepoPassword);

                if (!Directory.Exists(Paths.MainDirPath) || !Directory.Exists(Path.Combine(Paths.MainDirPath, ".svn")))
                {
                    return SyncStatus.Unknown;
                }

                try
                {
                    client.GetStatus(Paths.MainDirPath, out var changedFiles);

                    return changedFiles.Any() ? SyncStatus.Modified : SyncStatus.UpToDate;
                }
                catch (Exception ex)
                {
                    return SyncStatus.NotConnected;
                }
            }
        }

        public void Synchronize()
        {
            using (var client = new SvnClient())
            {
                client.Authentication.ForceCredentials(SettingsInfo.RepoUser, SettingsInfo.RepoPassword);

                if (!Directory.Exists(Paths.MainDirPath) || !Directory.Exists(Path.Combine(Paths.MainDirPath, ".svn")) || !Directory.Exists(Paths.RecordsPath) || !Directory.Exists(Paths.TemplatesPath))
                {
                    client.CheckOut(new SvnUriTarget(new Uri(SettingsInfo.RepoAddress)), Paths.MainDirPath);
                    return;
                }

                client.GetStatus(Paths.MainDirPath, out var changedFiles);

                foreach (var svnStatusEventArgse in changedFiles)
                {
                    if (svnStatusEventArgse.LocalContentStatus == SvnStatus.Missing || svnStatusEventArgse.LocalContentStatus == SvnStatus.Deleted)
                    {
                        if (File.Exists(svnStatusEventArgse.Path) || Directory.Exists(svnStatusEventArgse.Path))
                        {
                            var delArgs = new SvnDeleteArgs { KeepLocal = true };
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

                    if (svnStatusEventArgse.LocalContentStatus == SvnStatus.Conflicted)
                    {
                        client.Resolve(svnStatusEventArgse.Path, SvnAccept.MineFull);
                    }
                }

                var ca = new SvnCommitArgs { LogMessage = "Synchronizacja obiektów inwentaryzacji" };

                client.Commit(Paths.MainDirPath, ca);
            }
        }
    }
}
