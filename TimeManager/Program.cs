using Topshelf;

namespace TimeManager
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<TimeManager>(s =>
                {
                    s.ConstructUsing(name => new TimeManager());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("TimeManager App.");
                x.SetDisplayName("TimeManager");
                x.SetServiceName("TimeManager");
            });
        }
    }
}
