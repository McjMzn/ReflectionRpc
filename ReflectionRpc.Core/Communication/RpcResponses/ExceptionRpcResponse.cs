namespace ReflectionRpc.Core.Communication.RpcResponses
{
    public record ExceptionRpcResponse : IRpcResponse
    {
        public string ExceptionTypeName { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }

        public ExceptionRpcResponse(Exception exception)
        {
            this.ExceptionTypeName = exception.GetType().Name;
            this.ExceptionMessage = exception.Message;
            this.StackTrace = exception.StackTrace;
        }
    }
}
