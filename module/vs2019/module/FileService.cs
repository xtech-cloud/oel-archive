
using System.IO;
using System.Net;
using System.Text.Json;
using System.Collections.Generic;
using XTC.oelMVCS;

namespace oel.archive
{
    public class FileService: Service
    {
        public const string NAME = "oel.archive.FileService";
        private FileModel model = null;

        protected override void preSetup()
        {
            model = findModel(FileModel.NAME) as FileModel;
        }

        protected override void setup()
        {
            getLogger().Trace("setup oel.archive.FileService");
        }

        public void PostWrite(Proto.BlankRequest _request)
        {
            Dictionary<string, Any> paramMap = new Dictionary<string, Any>();

            post(string.Format("{0}/oel/archive/File/Write", getConfig().getField("domain").AsString()), paramMap, (_reply) =>
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new FieldConverter());
                var rsp = JsonSerializer.Deserialize<Proto.BlankResponse>(_reply, options);
                Model.Status reply = Model.Status.New<Model.Status>(rsp._status._code.AsInt(), rsp._status._message.AsString());
                model.Broadcast("/oel/archive/File/Write", reply);
            }, (_err) =>
            {
                getLogger().Error(_err.getMessage());
            }, null);
        }
        

        public void PostRead(Proto.BlankRequest _request)
        {
            Dictionary<string, Any> paramMap = new Dictionary<string, Any>();

            post(string.Format("{0}/oel/archive/File/Read", getConfig().getField("domain").AsString()), paramMap, (_reply) =>
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new FieldConverter());
                var rsp = JsonSerializer.Deserialize<Proto.BlankResponse>(_reply, options);
                Model.Status reply = Model.Status.New<Model.Status>(rsp._status._code.AsInt(), rsp._status._message.AsString());
                model.Broadcast("/oel/archive/File/Read", reply);
            }, (_err) =>
            {
                getLogger().Error(_err.getMessage());
            }, null);
        }
        


        protected override void asyncRequest(string _url, string _method, Dictionary<string, Any> _params, OnReplyCallback _onReply, OnErrorCallback _onError, Options _options)
        {
            string reply = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url);
                req.Method = _method;
                req.ContentType =
                "application/json;charset=utf-8";
                var options = new JsonSerializerOptions();
                options.Converters.Add(new AnyConverter());
                string json = System.Text.Json.JsonSerializer.Serialize(_params, options);
                byte[] data = System.Text.Encoding.UTF8.GetBytes(json);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                if (rsp == null)
                {
                    _onError(Error.NewNullErr("HttpWebResponse is null"));
                    return;
                }
                if (rsp.StatusCode != HttpStatusCode.OK)
                {
                    rsp.Close();
                    _onError(new Error(rsp.StatusCode.GetHashCode(), "HttpStatusCode != 200"));
                    return;
                }
                StreamReader sr;
                using (sr = new StreamReader(rsp.GetResponseStream()))
                {
                    reply = sr.ReadToEnd();
                }
                sr.Close();
            }
            catch (System.Exception ex)
            {
                _onError(Error.NewException(ex));
                return;
            }
            _onReply(reply);
        }
    }
}
