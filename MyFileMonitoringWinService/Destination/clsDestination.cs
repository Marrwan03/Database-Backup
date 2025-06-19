using System.IO;
namespace FileMonitoringLib
{
    public class clsDestination : clsPillars
    {
        public clsDestination(string path, enType type) : base(path, type)
        {
            base.path = path;
        }

        /// <summary>
        /// Active a FileSystemWatcher for Destination folder
        /// </summary>
        /// <returns> Active a FileSystemWatcher for watching Destination Folder</returns>
        public bool ActivatetheFileSystemWatcher()
        {
            return base.ActivatetheFileSystemWatcher(_OnDestinationCreate,
                _OnDestinationRenamed, _OnDestinationDeleted);
        }

        #region Events
        /// <summary>
        /// This raise When the Destination folder created a new file or folder.
        /// </summary>
        /// <param name="source">the source of DestinationFolder</param>
        /// <param name="e">object of FileSystemEventArgs</param>
         void _OnDestinationCreate(object source, FileSystemEventArgs e)
             =>  clsUtil.LogServicesEvent(clsGlobal.LogFilePath,$"File moved: [{clsGlobal.SourceFile.path}] -> [{e.FullPath}]");

        /// <summary>
        /// This raise When the Source folder Rename a filename or move file to another directory Folder.
        /// </summary>
        /// <param name="source">the source of SourceFolder</param>
        /// <param name="e">object of RenamedEventArgs</param>
        void _OnDestinationRenamed(object source, RenamedEventArgs e)
            => clsUtil.LogServicesEvent(clsGlobal.LogFilePath, $"Renamed File\n\t from: [{e.OldFullPath}], to: [{e.FullPath}]\n");

        /// <summary>
        /// This raise When the Source folder Deleted a file.
        /// </summary>
        /// <param name="source">the source of SourceFolder</param>
        /// <param name="e">object of FileSystemEventArgs</param>
        void _OnDestinationDeleted(object source, FileSystemEventArgs e)
             => clsUtil.LogServicesEvent(clsGlobal.LogFilePath, $"File with this Path: [{e.FullPath}] \"is deleted\"");

        #endregion
    }
}
