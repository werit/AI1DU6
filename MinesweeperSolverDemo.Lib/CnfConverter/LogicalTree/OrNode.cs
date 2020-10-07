using System.Collections.Generic;
using System.Text;
using MinesweeperSolverDemo.Lib.Objects;

namespace MinesweeperSolverDemo.Lib.CnfConverter.LogicalTree
{
    public class OrNode : BinaryLogicalOperation,INode
    {
        public  int ConvertNodeIntoStringBuilderRepresentation(StringBuilder sb, Dictionary<Panel, int> mapper)
        {
            LeftNode.ConvertNodeIntoStringBuilderRepresentation(sb, mapper);
            sb.Append(" ");
            RightNode.ConvertNodeIntoStringBuilderRepresentation(sb, mapper);
            
            return 0;
        }
        
        public INode ThisNodeLogicalOr(ValueNode otherNode)
        {
            return new OrNode(new ValueNode(otherNode.Value,!otherNode.IsNegated),this);
        }

        public OrNode(INode leftNode, INode  rightNode) : base(leftNode,  rightNode)
        {
        }
    }
}