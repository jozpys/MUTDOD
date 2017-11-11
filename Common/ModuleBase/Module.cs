
namespace MUTDOD.Common.ModuleBase
{
    public abstract class Module
    {
        public ModuleState State { get; protected set; }

        public ICore Core { get; protected set; }

        public virtual void Register(ICore core)
        {
            Core = core;
        }

        public virtual void Unregister()
        {
        }
    }
}
