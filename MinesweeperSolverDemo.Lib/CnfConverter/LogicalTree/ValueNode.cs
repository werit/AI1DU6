using System.Collections.Generic;
using System.Text;
using MinesweeperSolverDemo.Lib.Objects;

namespace MinesweeperSolverDemo.Lib.CnfConverter.LogicalTree
{
    public class ValueNode : INode
    {
        public ValueNode(Panel value,bool isNegated)
        {
            Value = value;
            IsNegated = isNegated;
        }

        public Panel Value { get;}
        public bool IsNegated { get; set; }

        public virtual int ConvertNodeIntoStringBuilderRepresentation(StringBuilder sb, Dictionary<Panel, int> mapper)
        {
            sb.Append(mapper[Value] * (IsNegated ? -1 : 1));
            return 0;
        }

        public INode ThisNodeLogicalOr(ValueNode otherNode)
        {
            return new OrNode(new ValueNode(otherNode.Value,!otherNode.IsNegated),this);
        }
    }
}