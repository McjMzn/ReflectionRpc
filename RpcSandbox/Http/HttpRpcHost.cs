using Newtonsoft.Json;
using ReflectionRpc.Http.Requests;
using SimpleRpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionRpc.Http
{
    public class HttpRpcHost<T> : RpcHostBase<T>, IDisposable
    {
        private HttpListener httpListener;

        private Task httpListenerTask;

        public HttpRpcHost(T target, string bindingAddress)
            : base(target)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(bindingAddress);
            httpListenerTask = StartHttpListener();
        }

        private Task StartHttpListener()
        {
            httpListener.Start();

            return Task.Run(() =>
            {
                while (httpListener.IsListening)
                {
                    var context = httpListener.GetContext();

                    var rpcRequest = GetRpcRequest(context);
                    var responseObject = HandleRpcRequest(rpcRequest);

                    SendResponseData(context, responseObject);
                }
            });
        }

        private object HandleRpcRequest(IHttpRpcRequest request)
        {
            object responseObject = null;
            switch (request)
            {
                case HttpRpcMethodRequest methodRequest:
                    responseObject = ExecuteMethod(methodRequest.MethodName, methodRequest.Arguments);
                    break;

                case HttpRpcSetPropertyRequest setPropertyRequest:
                    SetPropertyValue(setPropertyRequest.PropertyName, setPropertyRequest.Value);
                    break;

                case HttpRpcGetPropertyRequest getPropertyRequest:
                    responseObject = GetPropertyValue(getPropertyRequest.PropertyName);
                    break;

                default:
                    throw new NotSupportedException("Not supported type of payload.");
            };

            return responseObject;
        }

        private void SendResponseData(HttpListenerContext context, object responseObject)
        {
            var responsePayload = JsonConvert.SerializeObject(responseObject);
            var responsePayloadBytes = Encoding.UTF8.GetBytes(responsePayload);

            context.Response.StatusCode = 200;
            context.Response.KeepAlive = false;
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.OutputStream.Write(responsePayloadBytes, 0, responsePayloadBytes.Length);
            context.Response.Close();
        }

        private IHttpRpcRequest GetRpcRequest(HttpListenerContext context)
        {
            using var streamReader = new StreamReader(context.Request.InputStream);
            var body = streamReader.ReadToEnd();
            return (IHttpRpcRequest)JsonConvert.DeserializeObject(body, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        public void Dispose()
        {
            httpListener.Stop();
            httpListenerTask.Wait();
        }
    }
}
