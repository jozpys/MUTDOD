namespace MUTDOD.Common.ServerBase
{
    public interface IServerRunnable
    {
        string Name { get;}

        string Adress { get; }

        short Port { get; }

        void Run();

        void Stop();

        void Restart();
    }
}
