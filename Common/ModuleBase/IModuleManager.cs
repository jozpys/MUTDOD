namespace MUTDOD.Common.ModuleBase
{
    public interface IModuleManager
    {
        ICore Core { get; }

        void Init(ICore core);

        IModule RegisterModule(IModule module);
    }
}
