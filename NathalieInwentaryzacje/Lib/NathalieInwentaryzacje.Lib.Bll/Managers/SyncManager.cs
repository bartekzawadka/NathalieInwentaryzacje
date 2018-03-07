using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NathalieInwentaryzacje.Lib.Contracts.Dto.Settings;
using NathalieInwentaryzacje.Lib.Contracts.Enums;
using NathalieInwentaryzacje.Lib.Contracts.Interfaces;
using SharpSvn;

namespace NathalieInwentaryzacje.Lib.Bll.Managers
{
    public class SyncManager : ManagerBase, ISyncManager
    {
        private SettingsInfo SettingsInfo { get; set; }

        public SyncManager(SettingsInfo settings, DataLocationInfo paths) : base(paths)
        {
            SettingsInfo = settings;
        }

        public SyncStatus GetStatus()
        {
            return ExecuteSvnClient(client =>
            {
                if (!Directory.Exists(Path.Combine(Paths.MainDirPath, ".svn")))
                {
                    return SyncStatus.Unknown;
                }

                try
                {
                    client.GetStatus(Paths.MainDirPath, out var changedFiles);

                    return changedFiles.Any() ? SyncStatus.Modified : SyncStatus.UpToDate;
                }
                catch (Exception)
                {
                    return SyncStatus.NotConnected;
                }
            });
        }

        public void UpdateSettings(SettingsInfo settings)
        {
            SettingsInfo = settings;
        }

        public Task Synchronize(bool update = true)
        {
            return Task.Factory.StartNew(() =>
            {
                ExecuteSvnClient(client =>
                {
                    try
                    {
                        if (!Directory.Exists(Paths.MainDirPath) ||
                            !Directory.Exists(Path.Combine(Paths.MainDirPath, ".svn")))
                        {
                            client.CheckOut(new SvnUriTarget(new Uri(SettingsInfo.RepoAddress)), Paths.MainDirPath);
                        }

                        if (update)
                            client.Update(Paths.MainDirPath);
                    }
                    catch (SvnRepositoryIOException ex)
                    {
                        if (ex.InnerException != null &&
                            ex.InnerException.GetType() == typeof(SvnAuthenticationException))
                        {
                            throw new Exception(
                                "Dane logowania do repozytorium plików są nieprawidłowe. Proszę sprawdzić ustawienia");
                        }

                        throw;
                    }

                    if (!Directory.Exists(Paths.RecordsPath))
                        Directory.CreateDirectory(Paths.RecordsPath);
                    if (!Directory.Exists(Paths.TemplatesPath))
                        Directory.CreateDirectory(Paths.TemplatesPath);

                    client.GetStatus(Paths.MainDirPath, out var changedFiles);

                    foreach (var svnStatusEventArgse in changedFiles)
                    {
                        if (svnStatusEventArgse.LocalContentStatus == SvnStatus.Missing ||
                            svnStatusEventArgse.LocalContentStatus == SvnStatus.Deleted)
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

                    if (changedFiles.Any())
                        client.Commit(Paths.MainDirPath, ca);
                });
            });
        }

        private void ExecuteSvnClient(Action<SvnClient> action)
        {
            using (var client = new SvnClient())
            {
                client.Authentication.ForceCredentials(SettingsInfo.RepoUser, SettingsInfo.RepoPassword);
                action(client);
            }
        }

        private T ExecuteSvnClient<T>(Func<SvnClient, T> action)
        {
            using (var client = new SvnClient())
            {
                client.Authentication.ForceCredentials(SettingsInfo.RepoUser, SettingsInfo.RepoPassword);
                return action(client);
            }
        }
    }
}
