using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RestMediaServer.Controllers
{
    public class ImgStreamInfo
    {
        public String FullPath { get; set; }
        public long Size { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        public bool Exists { get; set; }
        public bool Error { get; set; }
        public string Exception { get; set; }
        public string Type { get; set; }

        public ImgStreamInfo(string id, string type)
        {
            try
            {
                FullPath = HttpContext.Current.Server.MapPath("~/image/" + id + "." + type );
                FileInfo f = new FileInfo(FullPath);
                Type = "image/"+type;
                Exists = f.Exists;
                if (f.Exists)
                {
                    Size = f.Length;
                    Start = 0;
                    End = Size - 1;
                }
            } catch (Exception ex)
            {
                Error = true;
                Exception = ex.Message;
            }
        }
    }

    public class MediaImgController : ApiController
    {
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(string id, string type)
        {
            var imgInfo = new ImgStreamInfo(id, type);
            if (imgInfo.Exists && !imgInfo.Error)
            {
                try
                {
                    FileStream ms = new FileStream(imgInfo.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(ms)
                    };
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imgInfo.Type);
                    return response;
                } catch(Exception ex)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = ex.Message
                    };
                    return response;
                }
            } else
            {
                if (imgInfo.Error)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = imgInfo.Exception
                    };
                    return response;
                }
                else if (!imgInfo.Exists)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = id + " NotFound"
                    };
                    return response;
                } else
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "Unknown Error"
                    };
                    return response;
                }
            }
        }
    }
}
