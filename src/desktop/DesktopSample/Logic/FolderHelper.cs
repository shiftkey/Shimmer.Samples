using System.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;

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
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Multiselect = false;
            var dialogResult = dialog.ShowDialog();
            var result = new SelectFolderResult();

            if (dialogResult == CommonFileDialogResult.Ok)
            {
                result.Result = true;
                result.Folder = dialog.FileNames.FirstOrDefault();
            }

            return result;
        }
    }
}
