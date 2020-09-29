using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinesweeperSolverDemo.Lib.Objects;

namespace MinesweeperSolverDemo
{
    public class ConvertToSat
    {
        public static void Convert(GameBoard gameBoard, Panel panel)
        {
            var allDist = new List<Panel>();
            var allMainPanelNeighboursNeigbours = new List<List<List<Panel>>>();
            var allRevealedNeighbours = gameBoard.GetNeighbors(panel.X, panel.Y).Where(nb => nb.IsRevealed);
            foreach (var neighbour in allRevealedNeighbours)
            {
                var nonRevealedNeighbours =
                    gameBoard.GetNeighbors(neighbour.X, neighbour.Y).Where(p => !p.IsRevealed).ToList();
                var flagged = nonRevealedNeighbours.Count(p => p.IsFlagged);
                var unknownMinesNearbyCount = neighbour.AdjacentMines - flagged; // taketo n-tice musim spravit
                var nonRevNonFlagNb = nonRevealedNeighbours.Where(pan => !pan.IsFlagged && !pan.IsMine).ToList();
                allDist.AddRange(nonRevNonFlagNb.Distinct());
                allMainPanelNeighboursNeigbours.Add(GenerateNTuples(unknownMinesNearbyCount,
                    nonRevNonFlagNb));
            }

            allDist = allDist.Distinct().ToList();
            ConvertIt(allMainPanelNeighboursNeigbours,allDist);

        }

        private static void ConvertIt(List<List<List<Panel>>> allMainPanelNeighboursNeigbours,List<Panel> allDistinctPanels)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"p sat {allDistinctPanels.Count}");
            sb.Append("*("); // andy
            foreach (var listOfRevealedNb in allMainPanelNeighboursNeigbours)
            {
                sb.Append("+("); // ory
                foreach (var tuplesOfPotentialMines in listOfRevealedNb)
                {
                    
                }
                sb.Append(")");
            }
            sb.Append(")");
        }

        private List<List<Panel>> Generate1Tuples(List<Panel> panels)
        {
            var result = new List<List<Panel>>();
            for (var i = 0; i < panels.Count; i++)
            {
                result.Add(new List<Panel> {panels[i]});
            }

            return result;
        }

        private List<List<Panel>> Generate2Tuples(List<Panel> panels)
        {
            var result = new List<List<Panel>>();
            for (var i = 0; i < panels.Count; i++)
            {
                for (var j = i + 1; j < panels.Count; j++)
                {
                    result.Add(new List<Panel> {panels[i], panels[j]});
                }
            }

            return result;
        }

        private static List<List<Panel>> GenerateNTuples(int tupleSize, List<Panel> panels)
        {
            if (tupleSize>panels.Count)
            {
                throw new ArgumentOutOfRangeException(
                    $"Not enough elements to make a tuple., Number of elements is: {panels.Count} and tuple size should be: {tupleSize}.");
            }
            var result = new List<List<Panel>>();
            if (tupleSize > 0)
            {
                RecursiveGenerateNTuples(tupleSize, new List<int>(), panels, result);
            }

            return result;
        }

        private static void RecursiveGenerateNTuples(int tupleSize, List<int> indexList, List<Panel> panels,
            List<List<Panel>> result)
        {

            var start = indexList.Count;
            for (var i = start; i < panels.Count; i++)
            {
                indexList.Add(i);
                if (tupleSize > 1)
                {
                    RecursiveGenerateNTuples(tupleSize - 1, indexList, panels, result);
                }
                else
                {
                    result.Add(indexList.Select(index => panels[index]).ToList());
                }

                indexList.RemoveAt(indexList.Count - 1);
            }


        }
    }
}