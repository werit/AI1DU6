using System.Collections.Generic;
using System.Text;
using MinesweeperSolverDemo.Lib.Objects;

namespace MinesweeperSolverDemo.Lib.CnfConverter.LogicalTree
{
    public interface INode
    {
        int ConvertNodeIntoStringBuilderRepresentation(StringBuilder sb, Dictionary<Panel, int> mapper);
        INode ThisNodeLogicalOr(ValueNode otherNode);
    }

    public abstract class BinaryLogicalOperation 
    {
        protected BinaryLogicalOperation(INode leftNode,INode rightNode)
        {
            RightNode = rightNode;
            LeftNode = leftNode;
        }

        protected INode LeftNode { get; set; }
        protected INode RightNode { get; set; }
    }
}