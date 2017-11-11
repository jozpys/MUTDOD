using System.Collections.Generic;

namespace MUTDOD.Common
{
    public interface IQueryTree
    {
        string TokenName { get; }
        string TokenValue { get; }
        int TokenLine { get; }
        int TokenCol { get; }
        ISubTrees ProductionsList { get; }
    }

    public interface ISubTrees : IList<IQueryTree> { }
}
