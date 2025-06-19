using DataAccess;
using FileMonitoringLib;
using System.IO;

namespace BusinessTier
{
    public class clsBackup : clsPillars
    {
        string _LogFilePath;
        public string LogFilePath { get { return _LogFilePath; } }
        public clsBackup(string LogFilePath, string path, enType type)
            : base(path, type)
        {
            _LogFilePath = LogFilePath;
        }

        /// <summary>
        /// This for backup the database with differential
        /// </summary>
        /// <param name="ToDiskPath">This is the path to save a backup</param>
        /// <returns>result of backup and the message if there is an error.</returns>
        public static (bool Result, string MessageError) BackUp(string ToDiskPath)
            => clsBackups.BackUp(ToDiskPath);

        public bool ActivatetheFileSystemWatcher()
            => base.ActivatetheFileSystemWatcher(_OnCreated, _OnRenamed, _OnDeleted);
        void _OnCreated(object sender, FileSystemEventArgs e)
        {
            clsUtil.LogServicesEvent(LogFilePath, $"Database backup successful: {e.FullPath}.");
        }
        void _OnRenamed(object sender, RenamedEventArgs e)
        {
            clsUtil.LogServicesEvent(LogFilePath, $"Renamed Backup name\n from: [{e.OldName}], to: [{e.Name}].");
        }

        void _OnDeleted(object sender, FileSystemEventArgs e)
        {
            clsUtil.LogServicesEvent(LogFilePath, $"This Backup[{e.FullPath}] is deleted.");
        }
    }
}
