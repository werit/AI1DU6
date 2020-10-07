using System.Collections.Generic;
using System.Text;
using MinesweeperSolverDemo.Lib.Objects;

namespace MinesweeperSolverDemo.Lib.CnfConverter.LogicalTree
{
    public class AndNode : BinaryLogicalOperation,INode
    {
        public  int ConvertNodeIntoStringBuilderRepresentation(StringBuilder sb, Dictionary<Panel, int> mapper)
        {
            var first = LeftNode.ConvertNodeIntoStringBuilderRepresentation(sb, mapper);
            sb.AppendLine(" 0");
            var second = RightNode.ConvertNodeIntoStringBuilderRepresentation(sb, mapper);

            return first + second + 1;
        }

        public  INode ThisNodeLogicalOr(ValueNode otherNode)
        {
            LeftNode = LeftNode.ThisNodeLogicalOr(otherNode);
            RightNode = RightNode.ThisNodeLogicalOr(otherNode);

            return this;
        }

        public AndNode(INode leftNode, INode rightNode) : base(leftNode, rightNode)
        {
        }
    }
}