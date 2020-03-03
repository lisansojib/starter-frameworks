using Presentation.Models;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Presentation.Extends
{
    /// <summary>
    /// This custom IHttpActionResult returns file
    /// </summary>
    public class FileActionResult : IHttpActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public FileActionResult(DownloadFileViewModel info)
        {
            DownloadFile = info;
        }

        /// <summary>
        /// 
        /// </summary>
        public DownloadFileViewModel DownloadFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new FileStream(DownloadFile.FilePath, FileMode.Open, FileAccess.Read))
            };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition.FileName = DownloadFile.FileName;

            // NOTE: Here I am just setting the result on the Task and not really doing any async stuff. 
            // But let's say you do stuff like contacting a File hosting service to get the file, then you would do 'async' stuff here.

            return Task.FromResult(response);
        }
    }
}