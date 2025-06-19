using BusinessTier;
using System.IO;

namespace DatabaseBackupWinService
{
    public static class clsGlobal
    {
        public static string LogFilePath()
        {
            if (!string.IsNullOrEmpty(LogFolder) && !string.IsNullOrEmpty(LogFileName))
                return Path.Combine(LogFolder, LogFileName);
            return null;
        }
        public static string ConnectionString;
        public static string LogFolder;
        public static string LogFileName;

        public static clsBackup BackupsFolder;
    }
}
