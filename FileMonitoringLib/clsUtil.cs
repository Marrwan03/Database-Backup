using System;
using System.IO;
using static FileMonitoringLib.clsPillars;

namespace FileMonitoringLib
{
    public static class clsUtil
    {
        /// <summary>
        /// This Func for Generate a new Global unique identifier.
        /// </summary>
        /// <returns>Get Guid with 16 chars</returns>
       public static string GenerateGUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        /// <summary>
        /// For Replace filename to Guid with same Extension.
        /// </summary>
        /// <param name="SourceFile">We just want the Extension from "SourceFile"</param>
        /// <returns>A Guid with SourceFile`s Extension</returns>
        public static string ReplaceFileNameWithGuid(string SourceFile)
        {
            string filename = SourceFile;
            FileInfo fileInfo = new FileInfo(filename);
            string Extn = fileInfo.Extension;
            return GenerateGUID() + Extn;
        }

        /// <summary>
        /// This Procedure for logging a new message in LogFilePath.
        /// </summary>
        /// <param name="LogFilePath">the file path for add a new message</param>
        /// <param name="Message">that will be in LogFilePath.</param>
        public static void LogServicesEvent(string LogFilePath, string Message)
        {

            string LogMessage = $"\n[{DateTime.Now:yyyy-MMM-dd HH-mm-ss}] {Message}\n";
            File.AppendAllText(LogFilePath, LogMessage);
            if (Environment.UserInteractive)
            {
                Console.WriteLine(LogMessage);
            }

        }

        /// <summary>
        /// This Function will do two things [Rename, Move],
        /// When your [source & dest] path is in the same folder it is [Rename],
        /// When your [source & dest] path is different folders it is [Move].
        /// </summary>
        /// <param name="LogFilePath">the file path for add a new message.</param>
        /// <param name="sourceFilePath">The path of the file to [Move Or Rename].</param>
        /// <param name="destFilePath">The new path for the file</param>
        /// <returns>Result of operation</returns>
        public static bool RenameOrMoveFor(string LogFilePath, string sourceFilePath, string destFilePath)
        {
            bool Renamed = true;

            try
            {
                 File.Move(sourceFilePath, destFilePath);
            }
            catch (Exception ex)
            {
                clsUtil.LogServicesEvent(LogFilePath, $"Occur Error: {ex.Message}");
                Renamed = false;
            }
            
            return Renamed;
        }


        /// <summary>
        /// This Procedure for create a new [File, Floder].
        /// </summary>
        /// <param name="path">This is a path of [File, Folder].</param>
        /// <param name="type">Choice between enType to do specific operation.</param>
        public static bool CreateFor(string path, enType type)
        {
            bool Created = true;

            if (!string.IsNullOrWhiteSpace(path))
            {
                if (type == enType.Folder)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                else
                {
                    if (!File.Exists(path))
                    {
                        using (File.Create(path)) {  }
                        
                    }
                }
            }
            else
            {
                Created = false;
            }
             return Created;
        }



        /// <summary>
        /// Renamed the filename Using Guid.
        /// </summary>
        /// <param name="LogFilePath">the file path for add a new message.</param> 
        /// <param name="SourceFile">the source file for get some info from it.</param>
        /// <param name="SourceFolder">the source folder for get some info from it.</param>
        /// <param name="DestinationFolder">the destination folder for get some info from it.</param>
        public static void RenamedTheNameOfFileFor(string LogFilePath,ref clsPillars SourceFile, clsPillars SourceFolder, clsPillars DestinationFolder)
        {           
            string GuidFileName = ReplaceFileNameWithGuid(SourceFile.Name);
            string NewSourceFilePath = Path.Combine(SourceFolder.path, GuidFileName);

            if (! RenameOrMoveFor(LogFilePath,SourceFile.path, NewSourceFilePath))
                 clsUtil.LogServicesEvent(LogFilePath,$"Error in Rename FileName,\nSourceFileInfo: {SourceFile.ToString()},\nDestinationFolderInfo: {DestinationFolder.ToString()}," +
                    $"\nNewSourceFilePath: {NewSourceFilePath}");

            SourceFile.path =  NewSourceFilePath;
        }

        /// <summary>
        /// Move the Source file to Destination Folder.
        /// </summary>
        /// <param name="LogFilePath">the file path for add a new message.</param> 
        /// <param name="FromSourceFile">This file will move to the Destination Folder.</param>
        /// <param name="ToDestinationFolder">In this folder will be add it.</param>
        public static  void Move(string LogFilePath, clsPillars FromSourceFile, clsPillars ToDestinationFolder)
        {
            if (!clsUtil.RenameOrMoveFor(LogFilePath, FromSourceFile.path,
            Path.Combine(ToDestinationFolder.path, FromSourceFile.Name)))
            {
               clsUtil.LogServicesEvent(LogFilePath, $"Error in Move,\nSourceFileInfo: {FromSourceFile.ToString()},\nDestinationFolderInfo: {ToDestinationFolder.ToString()}");
            }
        }

    }
}
