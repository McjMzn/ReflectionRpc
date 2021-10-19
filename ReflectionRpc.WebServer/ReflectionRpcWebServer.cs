namespace ReflectionRpc.WebServer
{
    public class ReflectionRpcWebServer
    {
        private WebApplication app;

        public ReflectionRpcWebServer(Action<WebApplicationBuilder> applicationBuilderAction = null, Action<WebApplication> applicationAction = null)
        {
            var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRazorPages();

            builder.Services.AddReflectionRpc();

            applicationBuilderAction?.Invoke(builder);

            app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGet("/", () => Results.LocalRedirect("/rpc/ui/hosts"));

            app.UseReflectionRpc();

            app.MapRazorPages();

            applicationAction?.Invoke(app);
        }

        public void RegisterAsRpcHost(object service, string tag)
        {
            app.RegisterAsRpcHost(service, tag);
        }

        public void Run(string url = null)
        {
            app.Run(url);
        }
    }
}
