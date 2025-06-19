using System.IO;

namespace FileMonitoringLib
{
    public class clsPillars : clsPillarsFormat
    {
        public enum enType { Folder, File }
        public enType Type { get; }
        string _path;
        public string path { get { return _path; } set { _path = value; _Name = Path.GetFileName(value); } }
        private string _Name;
        public string Name { get { return _Name; } }


        public override string ToString()
        {
            return $"Path: {path}, Name: {Name}, Type: {(Type == enType.File ? "File" : "Folder")}";
        }

        public clsPillars(string path, enType type)
        {
            this.path = path;
            Type = type;
        }

       public FileSystemWatcher WatcherFolder = new FileSystemWatcher();
        /// <summary>
        /// Active a FileSystemWatcher for Specific folder
        /// </summary>
        /// <returns> Active a FileSystemWatcher for watching Specific Folder</returns>
        public bool ActivatetheFileSystemWatcher(FileSystemEventHandler Created,
           RenamedEventHandler Renamed,FileSystemEventHandler Deleted)
        {
            bool Actived = false;
            
            if (Type == enType.Folder)
            {
                if (string.IsNullOrEmpty(WatcherFolder.Path))
                {
                    WatcherFolder = new FileSystemWatcher
                    {
                        Path = path,
                        Filter = "*.*",
                        EnableRaisingEvents = true,
                        InternalBufferSize = 65536,
                        IncludeSubdirectories = false
                    };
                    if (Created != null)
                        WatcherFolder.Created += Created;
                    if (Renamed != null)
                        WatcherFolder.Renamed += Renamed;
                    if(Deleted != null)
                        WatcherFolder.Deleted += Deleted;


                    Actived = true;
                }
            }
            return Actived;
        }



        /// <summary>
        /// DeActive a FileSystemWatcher for WatcherFolder
        /// </summary>
        public void DeActivatetheFileSystemWatcher()
        {
            WatcherFolder.EnableRaisingEvents = false;
            WatcherFolder.Dispose();
        }


    }
}
