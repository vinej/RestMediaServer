using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestMediaServer.Controllers
{
    public class VideoStream
    {
        public String FullPath { get; set; }
        public long Size { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        public bool Exists { get; set; }
        public bool Error { get; set; }
        public string Exception { get; set; }
        public string Type { get; set; }

        public VideoStream(string id, string type)
        {
            FullPath = HttpContext.Current.Server.MapPath("~/video/" + id + "." + type);
            FileInfo f = new FileInfo(FullPath);
            Type = "video/" + type;
            Exists = f.Exists;
            if (f.Exists)
            {
                Size = f.Length;
                Start = 0;
                End = Size - 1;
            }
        }

        public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            _ = context;
            _ = content;
            try
            {
                var buffer = new byte[65536];

                using (var video = File.Open(this.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    video.Seek(Start, SeekOrigin.Begin);
                    var length = Size;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min((int)Size, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (HttpException ex)
            {
                Error = true;
                Exception = ex.Message;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }

    public class MediaVideoController : ApiController
    {
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(string id, string type)
        {
            var video = new VideoStream(id, type);
            var response = Request.CreateResponse();
            if (!video.Exists)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            /*
            if (Request.Headers.Range != null)
            {
                var range = Request.Headers.Range.ToString();
                // bytes=xx-xx
                range = range.Replace("bytes=", "");
                char[] sep = new char[1]; sep[0] = '-';
                // xx-xx
                var split = range.Split(sep);
                if (split.Length > 1)
                {
                    if (split.Length == 2)
                    {
                        // x-x
                        // x-
                        // -x
                        video.Start = (split[0] == "") ? 0 : long.Parse(split[0]);
                        video.End = (split[1] == "") ? video.Size - 1 : long.Parse(split[1]);
                        video.Size = video.End = video.Start + 1;
                    }
                }
            }
            */
            response.Content = new PushStreamContent(video.WriteToStream, new MediaTypeHeaderValue(video.Type));
            //if (video.Error)
            //{
            //    response.StatusCode = HttpStatusCode.BadRequest;
            //    response.ReasonPhrase = video.Exception;
            //}
            return response;
        }
    }
}
