namespace MUTDOD.Common.ModuleBase.Metamodel.DataStructure
{
    public interface IMetaAttribute : IMetaProperty
    {
        IMetaType Owner { get; set; }
        IMetaType Type { get; set; }
    }
}
