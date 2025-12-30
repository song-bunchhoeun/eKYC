using DGC.eKYC.Deeplink.Extension;
using Microsoft.Extensions.Hosting;

var hostBuilder = Host.CreateDefaultBuilder(args);
hostBuilder.ConfigureAppConfiguration(EnvConfigExtension.Default);
hostBuilder.ConfigureFunctionsWebApplication(HostBuilderExtension.AddDefault);
hostBuilder.ConfigureServices(ServicesExtension.AddDefault);

hostBuilder.Build().Run();
