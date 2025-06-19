using System.IO;
using System.Threading.Tasks;

namespace FileMonitoringLib
{
    public class clsSource : clsPillars
    {
        public clsSource(string path, enType type)
            : base(path, type)
        {
            base.path = path;
        }

       
        /// <summary>
        /// Active a FileSystemWatcher for source folder
        /// </summary>
        /// <returns> Active a FileSystemWatcher for watching Source Folder</returns>
        public bool ActivatetheFileSystemWatcher()
        {
            return base.ActivatetheFileSystemWatcher(_OnSourceCreate,
                 _OnSourceRenamed, _OnSourceDeleted);
        }

        #region Events
        /// <summary>
        /// This raise When the Source folder created a new file or folder.
        /// </summary>
        /// <param name="source">the source of SourceFolder</param>
        /// <param name="e">object of FileSystemEventArgs</param>
         async void _OnSourceCreate(object source, FileSystemEventArgs e)
        {
            clsUtil.LogServicesEvent(clsGlobal.LogFilePath, $"File detected: [{e.FullPath}]");
            clsGlobal.SourceFile = new clsSource(e.FullPath, enType.File);

            //Step 1:
            clsPillars temp= clsGlobal.SourceFile;//Up cast
            clsUtil.RenamedTheNameOfFileFor
                (clsGlobal.LogFilePath,ref temp, clsGlobal.SourceFolder, clsGlobal.DestinationFolder);

            await Task.Delay(3000);

            //Step 2:
             clsUtil.Move(clsGlobal.LogFilePath, clsGlobal.SourceFile, clsGlobal.DestinationFolder);

           // await Task.Delay(1000);
        }
           

        /// <summary>
        /// This raise When the Source folder Rename a filename or move file to another directory Folder.
        /// </summary>
        /// <param name="source">the source of SourceFolder</param>
        /// <param name="e">object of RenamedEventArgs</param>
         void _OnSourceRenamed(object source, RenamedEventArgs e)
             =>  clsUtil.LogServicesEvent(clsGlobal.LogFilePath, $"Renamed File\n\t from: [{e.OldFullPath}], to: [{e.FullPath}]");

        /// <summary>
        /// This raise When the Source folder Deleted a file.
        /// </summary>
        /// <param name="source">the source of SourceFolder</param>
        /// <param name="e">object of FileSystemEventArgs</param>
        void _OnSourceDeleted(object source, FileSystemEventArgs e)
             => clsUtil.LogServicesEvent(clsGlobal.LogFilePath, $"File with this Path: [{e.FullPath}] \"is deleted\"");
        #endregion
    }
}
