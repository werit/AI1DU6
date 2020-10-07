using System.Collections.Generic;
using System.Text;
using MinesweeperSolverDemo.Lib.Objects;

namespace MinesweeperSolverDemo.Lib.CnfConverter.LogicalTree
{
    public class ImpliesNode : BinaryLogicalOperation,INode
    {
        public int ConvertNodeIntoStringBuilderRepresentation(StringBuilder sb, Dictionary<Panel, int> mapper)
        {
            throw new System.NotImplementedException();
        }

        public INode ThisNodeLogicalOr(ValueNode otherNode)
        {
            throw new System.NotImplementedException();
        }

        public ImpliesNode( INode leftNode,INode  rightNode) : base( leftNode,rightNode)
        {
        }

        public INode ConvertToCnf()
        {
            return RightNode.ThisNodeLogicalOr((ValueNode)LeftNode);
        }
    }
}