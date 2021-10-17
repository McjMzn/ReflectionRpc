namespace ReflectionRpc.Asp
{
    public class ReflectionRpcWebServer
    {
        private WebApplication app;

        public ReflectionRpcWebServer()
        {
            var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRazorPages();

            builder.Services.AddReflectionRpc();
            
            this.app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGet("/", () => Results.LocalRedirect("/rpc/ui/hosts"));

            app.UseReflectionRpc();

            app.MapRazorPages();
        }

        public void AddHostedService(object service, string tag)
        {
            this.app.HostReflectionRpcService(service, tag);
        }

        public void Run(string url = null)
        {
            this.app.Run(url);
        }
    }
}
