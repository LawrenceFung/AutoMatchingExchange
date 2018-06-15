using ExchangeServer.Properties;
using System;
using System.Reflection;
using System.Threading;

namespace ExchangeServer
{
    class Program
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            logger.Info($"Start {Assembly.GetExecutingAssembly().GetName().Name} {Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine($"Start {Assembly.GetExecutingAssembly().GetName().Name} {Assembly.GetExecutingAssembly().GetName().Version}");

            using (var exitEvent = new ManualResetEvent(false))
            {
                Console.CancelKeyPress += (sender, e) =>
                {
                    e.Cancel = true;
                    exitEvent.Set();
                };
                try
                {
                    Server server = new Server(
                        Settings.Default.ServerEndpoint, 
                        Settings.Default.TimeoutMilliSeconds,
                        Settings.Default.ChannelEndpoint);
                    server.Start();
                    logger.Info("Server Started.");
                    Console.Write("Server Started.");
                    exitEvent.WaitOne();

                    server.Stop();
                    logger.Info("Server Stopped.");
                    Console.Write("Server Stopped.");
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                finally
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
