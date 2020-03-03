using System.IO;
using System.Web;

namespace Presentation.Models
{
    /// <summary>
    /// View model for download file
    /// </summary>
    public class DownloadFileViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Virtual Path of the file to be downloaded.</param>
        public DownloadFileViewModel(string path)
        {
            FilePath = HttpContext.Current.Server.MapPath(path);
            FileName = Path.GetFileName(path);
        }

        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; private set; }
    }
}
