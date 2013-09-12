using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Shimmer.DesktopDemo.Logic
{
    public class SelectFolderResult
    {
        public bool? Result { get; set; }
        public string Folder { get; set; }
    }

    public interface IFolderHelper
    {
        SelectFolderResult SelectFolder();
    }

    public class FolderHelper : IFolderHelper
    {
        public SelectFolderResult SelectFolder()
        {
            var fileDialog = new OpenFileDialog();
            var dialogResult = fileDialog.ShowDialog();
            var result = new SelectFolderResult();
            result.Result = dialogResult;

            if (dialogResult == true) {
                result.Folder = fileDialog.FileName;
            }

            return result;
        }
    }
}
