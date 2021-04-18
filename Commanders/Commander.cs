namespace ASP.NET_Worker_Reflection.Commanders
{
    using Serilog.Core;

    public abstract class Commander
    {
        public abstract void Execute(Logger logger);
    }
}