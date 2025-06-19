using System.Configuration;
using System.IO;

namespace FileMonitoringLib
{
    //C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe
    public static class clsGlobal
    {
        public static string LogFilePath =
            Path.Combine(ConfigurationManager.AppSettings["LogFolder"],
            ConfigurationManager.AppSettings["LogFileName"]);


        public static clsSource SourceFile { get; set; }
        public static clsSource SourceFolder {  get; set; }
        public static clsDestination DestinationFolder { get; set; }



    }
}
