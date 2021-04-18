namespace ASP.NET_Worker_Reflection.Commanders
{
    using Serilog.Core;

    public sealed class TaskOneCommander : Commander
    {
        public override void Execute(Logger logger)
        {
            logger.Information("Implementação!");
        }
    }
}