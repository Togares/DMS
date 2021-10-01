using CommonTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.FileOpener
{
    public class FileOpener
    {
        public static void OpenFile( File file)
        {
            string path = @file.Qualifier;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = System.IO.Path.GetFileName(path);
            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
            psi.ErrorDialog = true;
            try
            {
                Process.Start(psi);
            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == 1223) // vom Benutzer abgebrochen
                    return;
                else throw;
            }
        }

        public static void ShowExplorer(File selectedFile)
        {
            Process.Start(@selectedFile.Path);
        }
    }
}
