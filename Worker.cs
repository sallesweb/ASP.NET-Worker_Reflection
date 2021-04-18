namespace ASP.NET_Worker_Reflection
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ASP.NET_Worker_Reflection.Commanders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(
            ILogger<Worker> logger,
            IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var serviceName = this._configuration["ServiceName"].ToString();
                // var path = System.Reflection.Assembly.GetExecutingAssembly().Location;

                using (var logger = new LoggerConfiguration().WriteTo.File($"/home/salles/Documents/Projects/ASP.NET-Worker_Reflection/Log/{serviceName}.log").CreateLogger())
                {
                    var commander = this._configuration["Commander"].ToString();
                    var commanderType = Type.GetType($"ASP.NET_Worker_Reflection.Commanders.{commander}Commander");

                    if (commanderType != null)
                    {
                        var commanderMethod = commanderType.GetMethod("Execute");

                        if (commanderMethod != null)
                        {
                            var commanderParameters = commanderMethod.GetParameters();
                            var commanderObject = (Commander)Activator.CreateInstance(commanderType, null);

                            if (commanderParameters.Length == 0)
                            {
                                commanderMethod.Invoke(commanderObject, null);
                            }
                            else
                            {
                                var parametersArray = new object[] { logger };
                                commanderMethod.Invoke(commanderObject, parametersArray);
                            }

                            logger.Information("Concluído!");
                        }
                        else
                            logger.Error("Método vazio!");
                    }
                    else
                        logger.Error("Tipo vazio!");
                }

                this._logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
