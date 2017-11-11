namespace MUTDOD.Common.ModuleBase
{
    public interface IModule
    {
        string Name { get; }

        ModuleState State { get; }

        ICore Core { get; }

        void Register(ICore core);

        void Unregister();
    }
}
