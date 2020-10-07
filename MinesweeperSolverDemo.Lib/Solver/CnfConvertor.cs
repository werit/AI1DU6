using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinesweeperSolverDemo.Lib.CnfConverter.LogicalTree;
using MinesweeperSolverDemo.Lib.Objects;

namespace MinesweeperSolverDemo.Lib.Solver
{
    public static class CnfConvertor
    {
        public static void ConvertToCnf(GameBoard gameBoard, Panel panel)
        {
            var resultTree = GenerateCnfTree(gameBoard, panel, out var allPanelsUsedAsTreeVariables);

            allPanelsUsedAsTreeVariables = allPanelsUsedAsTreeVariables.Distinct().ToList();
            
            
            var panelVariableDict = CreatePanelToNumberMapper(allPanelsUsedAsTreeVariables);

            SaveCnfOutput(resultTree,  panelVariableDict, allPanelsUsedAsTreeVariables);
        }

        private static INode GenerateCnfTree(GameBoard gameBoard, Panel panel, out List<Panel> allDist)
        {
            allDist = new List<Panel>();
            INode resultTree = new ValueNode(panel, false);
            var allRevealedNeighbours = gameBoard.GetNeighbors(panel.X, panel.Y).Where(nb => nb.IsRevealed);
            // we want whole component not just direct neighbours of 'panel'
            allRevealedNeighbours = GetRevealedComponent(gameBoard, allRevealedNeighbours.ToList());
            foreach (var neighbour in allRevealedNeighbours)
            {
                var nonRevealedNeighbours =
                    gameBoard.GetNeighbors(neighbour.X, neighbour.Y).Where(p => !p.IsRevealed).ToList();
                var flagged = nonRevealedNeighbours.Count(p => p.IsFlagged);
                // count of not flagged mines nearby
                var unknownMinesNearbyCount = neighbour.AdjacentMines - flagged;

                var nonRevNonFlagNb = nonRevealedNeighbours.Where(pan => !pan.IsFlagged).ToList();
                if (nonRevNonFlagNb.Count == 0)
                {
                    //this is neccessary because we check whole component with no non revealed neighbours
                    continue;
                }

                allDist.AddRange(nonRevNonFlagNb.Distinct());
                resultTree = new AndNode(Kn(unknownMinesNearbyCount, nonRevNonFlagNb.Count, nonRevNonFlagNb), resultTree);
            }

            return resultTree;
        }

        private static Dictionary<Panel, int> CreatePanelToNumberMapper(List<Panel> allDist)
        {
            var panelVariableDict = new Dictionary<Panel, int>();
            for (var i = 0; i < allDist.Count; i++)
            {
                panelVariableDict.Add(allDist[i], (i + 1));
            }

            return panelVariableDict;
        }

        private static void SaveCnfOutput(INode resultTree,Dictionary<Panel, int> panelVariableDict, List<Panel> allDist)
        {
            var sb = new StringBuilder();
            var clausesCount = resultTree.ConvertNodeIntoStringBuilderRepresentation(sb, panelVariableDict) + 1;
            FinishLastClause(sb);

            using (var file = new System.IO.StreamWriter(@".\inputForSolver.cnf"))
            {
                var firstLine = $"p cnf {allDist.Count} {clausesCount}";
                file.WriteLine(firstLine);
                file.WriteLine(sb.ToString());
            }
        }

        private static void FinishLastClause(StringBuilder sb)
        {
            sb.Append(" 0");
        }

        private static IEnumerable<Panel> GetRevealedComponent(GameBoard gameBoard,List<Panel> revealedTilesOrig)
        {
            var enrichedRevealedTiles = new List<Panel>(revealedTilesOrig);
            var stack = new Stack<Panel>(enrichedRevealedTiles);
            while (stack.Count>0)
            {
                var panel = stack.Pop();
                var potentialAdditionalPanelsOfComponent= gameBoard.GetNeighbors(panel.X, panel.Y).Where(p => p.IsRevealed);
                foreach (var potentialAdditionalPanel in potentialAdditionalPanelsOfComponent)
                {
                    if (enrichedRevealedTiles.Contains(potentialAdditionalPanel)) continue;
                    stack.Push(potentialAdditionalPanel);
                    enrichedRevealedTiles.Add(potentialAdditionalPanel);
                }
            }

            return enrichedRevealedTiles;
        }

        private static INode Kn(int mines, int freeSpaces, List<Panel> unrevealedTiles)
        {
            if (mines==0)
            {
                return GetAllConnectedByAnd(freeSpaces, unrevealedTiles,true);
            }

            if (mines == freeSpaces)
            {
                return GetAllConnectedByAnd(freeSpaces, unrevealedTiles,false);
            }

            return new AndNode(
                new ImpliesNode(new ValueNode(unrevealedTiles[freeSpaces-1], false),
                    Kn(mines - 1, freeSpaces - 1, unrevealedTiles)).ConvertToCnf(),
                new ImpliesNode(new ValueNode(unrevealedTiles[freeSpaces-1], true),
                    Kn(mines, freeSpaces - 1, unrevealedTiles)).ConvertToCnf());

        }

        private static INode GetAllConnectedByAnd(int freeSpaces, List<Panel> unrevealedTiles, bool isNegated)
        {
            if (freeSpaces == 1)
            {
                return new ValueNode(unrevealedTiles[0], isNegated);
            }

            var tmpResult = new AndNode(
                new ValueNode(unrevealedTiles[0], isNegated),
                new ValueNode(unrevealedTiles[1], isNegated));
            for (var i = 2; i < freeSpaces; i++)
            {
                tmpResult = new AndNode(new ValueNode(unrevealedTiles[i], isNegated), tmpResult);
            }

            return tmpResult;
        }
    }
}