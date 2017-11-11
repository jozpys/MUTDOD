namespace MUTDOD.Common.ModuleBase.Metamodel.DataStructure
{
    public interface IMetaRelationship : IMetaProperty
    {
        IMetaObject MasterObject { get; set; }
        IMetaObject SlaveObject { get; set; }
    }
}
